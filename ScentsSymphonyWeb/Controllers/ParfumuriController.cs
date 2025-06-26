using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScentsSymphonyWeb.Data;
using ScentsSymphonyWeb.Models;
using System.Linq;

namespace ScentsSymphonyWeb.Controllers
{
    public class ParfumuriController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ParfumuriController(ApplicationDbContext db)
        {
            _db = db;
        }

        // =====================
        // ADMIN: LISTARE CRUD
        // =====================

        [Authorize(Roles = "Admin")]
        public IActionResult Index(string searchTerm = "")
        {
            var model = string.IsNullOrEmpty(searchTerm)
                ? _db.Perfume.ToList()
                : _db.Perfume.Where(p => p.Name.Contains(searchTerm)).ToList();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Parfumuri parfum)
        {
            if (ModelState.IsValid)
            {
                _db.Perfume.Add(parfum);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(parfum);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var parfum = _db.Perfume.Find(id);
            if (parfum == null)
            {
                return NotFound();
            }
            return View(parfum);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Parfumuri parfum)
        {
            if (ModelState.IsValid)
            {
                _db.Perfume.Update(parfum);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(parfum);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var parfum = _db.Perfume.Find(id);
            if (parfum == null)
            {
                return NotFound();
            }
            return View(parfum);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var parfum = _db.Perfume.Find(id);
            if (parfum != null)
            {
                _db.Perfume.Remove(parfum);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        // =====================
        // PUBLIC: Detalii + Căutare rapidă
        // =====================

        [AllowAnonymous]
        public IActionResult Search(string searchTerm)
        {
            var results = _db.Perfume
                .Where(p => p.Name.Contains(searchTerm))
                .ToList();

            return View("SearchResults", results);
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var parfum = _db.Perfume.FirstOrDefault(p => p.ProductID == id);
            if (parfum == null)
            {
                return NotFound();
            }
            return View(parfum);
        }

        // =====================
        // PUBLIC: SHOP + FILTRE (brand & preț)
        // =====================

        [AllowAnonymous]
        public IActionResult Shop(string? brand, decimal? pretMin, decimal? pretMax)
        {
            var query = _db.Perfume.AsQueryable();

            if (!string.IsNullOrEmpty(brand))
                query = query.Where(p => p.Brand == brand);

            if (pretMin.HasValue)
                query = query.Where(p =>
                    (p.Price50ml.HasValue && p.Price50ml.Value >= pretMin) ||
                    (p.Price100ml.HasValue && p.Price100ml.Value >= pretMin));

            if (pretMax.HasValue)
                query = query.Where(p =>
                    (p.Price50ml.HasValue && p.Price50ml.Value <= pretMax) ||
                    (p.Price100ml.HasValue && p.Price100ml.Value <= pretMax));

            var viewModel = new ParfumuriFilterViewModel
            {
                Parfumuri = query.ToList(),
                Branduri = _db.Perfume.Select(p => p.Brand).Distinct().ToList(),
                Brand = brand,
                PretMin = pretMin,
                PretMax = pretMax
            };

            return View(viewModel);
        }
    }
}
