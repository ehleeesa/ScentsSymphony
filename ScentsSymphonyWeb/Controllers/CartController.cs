using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ScentsSymphonyWeb.Data;
using ScentsSymphonyWeb.Helpers;
using ScentsSymphonyWeb.Models;
using ScentsSymphonyWeb.Services;
using Stripe.Checkout;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stripe;

namespace ScentsSymphonyWeb.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly StripeSettings _stripeSettings;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IInvoiceService _invoiceService;
        private readonly IEmailService _emailService;

        public CartController(
            ApplicationDbContext db,
            IOptions<StripeSettings> stripeSettings,
            UserManager<IdentityUser> userManager,
            IInvoiceService invoiceService,
            IEmailService emailService)
        {
            _db = db;
            _stripeSettings = stripeSettings.Value;
            _userManager = userManager;
            _invoiceService = invoiceService;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart") ?? new List<CartItem>();
            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, string selectedQuantity)
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart") ?? new List<CartItem>();
            var product = _db.Perfume.FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                var cartItem = cart.FirstOrDefault(c => c.Product.ProductID == productId && c.SelectedQuantity == selectedQuantity);
                if (cartItem == null)
                {
                    cart.Add(new CartItem { Product = product, Quantity = 1, SelectedQuantity = selectedQuantity });
                }
                else
                {
                    cartItem.Quantity++;
                }

                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }

            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int productId, string selectedQuantity) 
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            var cartItem = cart?.FirstOrDefault(c => c.Product.ProductID == productId && c.SelectedQuantity == selectedQuantity);

            if (cartItem != null)
            {
                cart.Remove(cartItem);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Shipping");
        }

        [HttpGet]
        public IActionResult Shipping()
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Shipping(ShippingInfo info)
        {
            if (!ModelState.IsValid)
            {
                return View(info);
            }

            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index");
            }

            var user = await _userManager.GetUserAsync(User);

            int GetPriceForCartItem(CartItem item)
            {
                return item.SelectedQuantity == "100ml"
                    ? (item.Product.Price100ml ?? 0)
                    : (item.Product.Price50ml ?? 0);
            }

            var order = new Order
            {
                UserID = user.Id,
                OrderDate = DateTime.Now,
                TotalAmount = cart.Sum(item => GetPriceForCartItem(item) * item.Quantity),
                FullName = info.FullName,
                Address = info.Address,
                County = info.County,
                City = info.City,
                PostalCode = info.PostalCode,
                Phone = info.Phone,
                DeliveryMethod = info.DeliveryMethod,
                OrderItems = cart.Select(item => new OrderItem
                {
                    ProductID = item.Product.ProductID,
                    Quantity = item.Quantity,
                    Price = GetPriceForCartItem(item)
                }).ToList()
            };

            _db.Orders.Add(order);
            _db.SaveChanges();

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = cart.Select(item => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(GetPriceForCartItem(item) * 100),
                        Currency = "ron",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name
                        }
                    },
                    Quantity = item.Quantity
                }).ToList(),
                Mode = "payment",
                SuccessUrl = Url.Action("StripeSuccess", "Cart", new { orderId = order.OrderID }, Request.Scheme),
                CancelUrl = Url.Action("Index", "Cart", null, Request.Scheme)
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        public async Task<IActionResult> StripeSuccess(int orderId)
        {
            var order = _db.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefault(o => o.OrderID == orderId);

            if (order == null)
            {
                return RedirectToAction("Index");
            }

            var user = await _userManager.GetUserAsync(User);

            var pdf = _invoiceService.GenerateInvoice(order);

            await _emailService.SendEmailAsync(
                user.Email,
                order.FullName ?? user.Email,
                "Factura Scents Symphony",
                "<p>Îți mulțumim pentru comandă! Atașat găsești factura ta în format PDF.</p>",
                attachment: pdf,
                attachmentName: $"Factura_{order.OrderID}.pdf"
            );

            HttpContext.Session.Remove("cart");

            return View("OrderConfirmationStripe");
        }

        public IActionResult OrderConfirmation(int orderId)
        {
            var order = _db.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefault(o => o.OrderID == orderId);

            if (order == null)
            {
                return RedirectToAction("Index");
            }

            return View(order);
        }
    }
}
