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
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            var ordersQuery = _context.Orders.Include(o => o.OrderStatus).OrderByDescending(o => o.CreateDate).AsQueryable();

            var totalCount = await ordersQuery.CountAsync();
            var orders = await ordersQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var orderStatuses = await _context.OrderStatuses.ToListAsync(); // Ensure OrderStatuses are loaded

            ViewBag.OrderStatuses = orderStatuses;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);

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
        [HttpPost]
        public IActionResult DeleteOrder(int orderId)
        {
            // Find the order and related order details
            var order = _context.Orders.Include(o => o.OrderDetails).FirstOrDefault(o => o.Id == orderId);

            if (order != null)
            {
                // Remove related order details first
                foreach (var orderDetail in order.OrderDetails)
                {
                    _context.OrderDetails.Remove(orderDetail);
                }

                // Remove the order
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }

            return RedirectToAction("Index"); // Redirect back to the orders list
        }


    }
}

