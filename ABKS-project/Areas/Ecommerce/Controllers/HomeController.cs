using ABKS_project.Areas.Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using ABKS_project.Repositories;
using System.Threading.Tasks;

namespace ABKS_project.Areas.Ecommerce.Controllers
{
    [Area("Ecommerce")]
    [Route("Ecommerce/[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly productContext _context;
        private readonly ICartRepository _cartRepo;

        public HomeController(productContext context, ICartRepository cartRepo)
        {
            _context = context;
            _cartRepo = cartRepo;
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

        [HttpGet]
        public async Task<IActionResult> AddItem(int productId, int qty = 1, int redirect = 0)
        {
            var cartCount = await _cartRepo.AddItem(productId, qty);
            if (redirect == 0)
                return Ok(cartCount);
            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> RemoveItem(int productId)
        {
            var cartCount = await _cartRepo.RemoveItem(productId);
            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepo.GetUserCart();
            return View(cart);
        }

        public async Task<IActionResult> GetTotalItemInCart()
        {
            int cartItem = await _cartRepo.GetCartItemCount();
            return Ok(cartItem);
        }
    }
}
