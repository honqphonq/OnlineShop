using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace OnlineShop.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(int? page)
        {
            return View(_context.Products.Include(c => c.ProductTypes).Include(c=>c.SpecialTag).ToList().ToPagedList(page??1,6));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var product = _context.Products.Include(p => p.ProductTypes).FirstOrDefault(p=>p.Id == id);
            if(product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ActionName("Details")]
        public ActionResult ProductDetails(int? id)
        {
            List<Products> products = new List<Products>();
            if (id == null)
            {
                return NotFound();
            }
            var product = _context.Products.Include(p => p.ProductTypes).FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            products = HttpContext.Session.Get<List<Products>>("products");
            if(products == null)
            {
                products = new List<Products>();    
            }
            products.Add(product);
            HttpContext.Session.Set("products", products);
            return View(product);
        }

        //GET Remove action method
        [ActionName("Remove")]
        public IActionResult RemoveToCart(int? id)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction(nameof(Cart));
        }


        [HttpPost]
        public IActionResult Remove(int? id)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if(products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if(product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Cart()
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if(products == null)
            {
                products = new List<Products>();
            }
            return View(products);
        }
    }
}
