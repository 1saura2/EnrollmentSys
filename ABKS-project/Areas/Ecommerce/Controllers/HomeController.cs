using ABKS_project.Areas.Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABKS_project.Areas.Ecommerce.Controllers
{
    [Area("Ecommerce")]
    public class HomeController : Controller
    {
        private readonly productContext _context;

        public HomeController(productContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ListProduct()
        {
            var product = _context.Products.ToList();
            return View(product);
        }


    }
}
