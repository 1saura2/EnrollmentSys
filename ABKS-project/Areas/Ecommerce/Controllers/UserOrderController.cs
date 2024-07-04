using ABKS_project.Areas.Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ABKS_project.Areas.Ecommerce.Controllers
{
    [Area("Ecommerce")]
    public class UserOrderController : Controller
    {
    
     
        private readonly productContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserOrderController(productContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("orders")]
        public async Task<IActionResult> GetOrdersByUserId()
        {
            string userId = GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Include(o => o.OrderStatus)
                .OrderByDescending(o => o.CreateDate) // Sort by CreateDate in descending order
                .ToListAsync();

            return View(orders);
        }




        private string GetUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue("UserId");
        }

    }
}
