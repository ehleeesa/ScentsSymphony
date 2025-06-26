using Microsoft.AspNetCore.Mvc;
using ScentsSymphonyWeb.Data;
using ScentsSymphonyWeb.Models;
using System.Collections.Generic;
using System.Linq;

namespace ScentsSymphonyWeb.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ShopController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Index(string? brand, decimal? pretMin, decimal? pretMax, string? nisa, string? cantitateDisponibila)
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

            if (!string.IsNullOrEmpty(nisa))
                query = query.Where(p => p.Nisa == nisa);

            if (!string.IsNullOrEmpty(cantitateDisponibila))
                query = query.Where(p => p.CantitateDisponibila == cantitateDisponibila);

            var viewModel = new ParfumuriFilterViewModel
            {
                Parfumuri = query.ToList(),
                Branduri = _db.Perfume.Select(p => p.Brand).Distinct().ToList(),
                NiseDisponibile = _db.Perfume.Select(p => p.Nisa).Distinct().ToList(),
                CantitatiDisponibile = _db.Perfume.Select(p => p.CantitateDisponibila).Distinct().ToList(),

                Brand = brand,
                PretMin = pretMin,
                PretMax = pretMax,
                Nisa = nisa,
                CantitateDisponibila = cantitateDisponibila
            };

            return View(viewModel);
        }

        public IActionResult Details(int id)
        {
            var parfum = _db.Perfume.FirstOrDefault(p => p.ProductID == id);
            if (parfum == null)
            {
                return NotFound();
            }

            return View(parfum);
        }
    }
}
