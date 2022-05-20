using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductTypesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            var data = _context.ProductTypes.ToList();
            return View(data);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductTypes productTypes)
        {
            if (ModelState.IsValid)
            {
                _context.ProductTypes.Add(productTypes);
                await _context.SaveChangesAsync();
                TempData["save"] = "Product type has been saved";
                return RedirectToAction("Index");
            }
            return View(productTypes);
        }

        public ActionResult Edit(int id)
        {
            var product = _context.ProductTypes.SingleOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductTypes productTypes)
        {
            if (ModelState.IsValid)
            {
                _context.ProductTypes.Update(productTypes);
                await _context.SaveChangesAsync();
                TempData["edit"] = "Product type has been edited";
                return RedirectToAction("Index");
            }
            return View(productTypes);
        }

        public ActionResult Details(int id)
        {
            var product = _context.ProductTypes.SingleOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(ProductTypes productTypes)
        {
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var product = _context.ProductTypes.SingleOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ProductTypes product)
        {
            if (ModelState.IsValid)
            {
                _context.ProductTypes.Remove(product);
                await _context.SaveChangesAsync();
                TempData["delete"] = "Product type has been deleted";
                return RedirectToAction("Index");
            }
            return View(product);
        }
    }
}
