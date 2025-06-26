using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScentsSymphonyWeb.Data;
using ScentsSymphonyWeb.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ScentsSymphonyWeb.Controllers
{
    [Authorize]
    public class WishlistController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public WishlistController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var wishlist = _db.WishlistItems
                .Include(w => w.Product)
                .Where(w => w.UserID == user.Id)
                .ToList();

            return View(wishlist);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int productId)
        {
            var user = await _userManager.GetUserAsync(User);

            var exists = _db.WishlistItems.Any(w => w.UserID == user.Id && w.ProductID == productId);
            if (!exists)
            {
                var item = new WishlistItem
                {
                    UserID = user.Id,
                    ProductID = productId
                };
                _db.WishlistItems.Add(item);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Shop", new { id = productId });
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int wishlistItemId)
        {
            var item = _db.WishlistItems.FirstOrDefault(w => w.WishlistItemID == wishlistItemId);
            if (item != null)
            {
                _db.WishlistItems.Remove(item);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
