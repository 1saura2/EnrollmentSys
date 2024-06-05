using ABKS_project.Areas.Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "User not logged in" });
            }

            // Here, you can add code to add the product to the user's cart
            // For example, you might save the product ID and user ID to a database

            return Json(new { success = true, userId });
        }

    }
}

