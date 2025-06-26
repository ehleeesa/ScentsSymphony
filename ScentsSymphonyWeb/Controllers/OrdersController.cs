using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScentsSymphonyWeb.Data;
using ScentsSymphonyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScentsSymphonyWeb.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OrdersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            var orders = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .AsQueryable();

            if (!isAdmin)
            {
                var userId = _userManager.GetUserId(User);
                orders = orders.Where(o => o.UserID == userId);
            }

            var orderList = await orders.OrderByDescending(o => o.OrderDate).ToListAsync();

            return View("~/Views/Comenzi/Index.cshtml", orderList);
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderID == id);

            if (order == null)
            {
                return NotFound();
            }

            if (!isAdmin && order.UserID != _userManager.GetUserId(User))
            {
                return Forbid();
            }

            return View("~/Views/Comenzi/Details.cshtml", order);
        }

        [HttpPost]
        public async Task<IActionResult> Create(decimal totalAmount, List<int> productIds, List<string> selectedQuantities)
        {
            var userId = _userManager.GetUserId(User);

            var orderItems = new List<OrderItem>();

            for (int i = 0; i < productIds.Count; i++)
            {
                var product = await _context.Perfume.FindAsync(productIds[i]);
                if (product == null)
                {
                    continue; 
                }

                int price = 0;

                if (i < selectedQuantities.Count)
                {
                    var quantitySelected = selectedQuantities[i];
                    if (quantitySelected == "100ml")
                    {
                        price = product.Price100ml ?? 0;
                    }
                    else
                    {
                        price = product.Price50ml ?? 0;
                    }
                }
                else
                {
                    price = product.Price50ml ?? 0;
                }

                orderItems.Add(new OrderItem
                {
                    ProductID = productIds[i],
                    Quantity = 1,
                    Price = price
                });
            }

            var order = new Order
            {
                UserID = userId,
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                OrderItems = orderItems
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
