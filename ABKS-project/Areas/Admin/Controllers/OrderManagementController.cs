using ABKS_project.Areas.Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ABKS_project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderManagementController : Controller
    {
        private readonly productContext _context;
        public OrderManagementController(productContext context) {
            _context = context;
        }
        public IActionResult Index()
        {
            var orders = _context.Orders.Include(o => o.OrderStatus).ToList();
            var orderStatuses = _context.OrderStatuses.ToList(); // Ensure OrderStatuses are loaded

            ViewBag.OrderStatuses = orderStatuses;

            return View(orders);
        }

        // POST: /AdminOperations/UpdateStatus
        [HttpPost]
        public IActionResult UpdateStatus(int orderId, int orderStatusId)
        {
            var order = _context.Orders.Find(orderId);
            if (order == null)
            {
                return NotFound();
            }

            order.OrderStatusId = orderStatusId;
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // POST: /AdminOperations/UpdatePayment
        [HttpPost]
        public IActionResult UpdatePayment(int orderId, bool isPaid)
        {
            var order = _context.Orders.Find(orderId);
            if (order == null)
            {
                return NotFound();
            }

            order.IsPaid = isPaid;
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}

