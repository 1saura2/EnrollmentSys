using ABKS_project.Areas.Ecommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ABKS_project.Repositories
{
    public class UserOrderRepository : IUserOrderRepository

    {
        private readonly productContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserOrderRepository(productContext db,IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }
        public Task ChangeOrderStatus()
        {
            throw new NotImplementedException();
        }

        public async Task<Order?> GetOrderById(int id)
        {
            return await _db.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<OrderStatus>> GetOrderStatuses()
        {
            return await _db.OrderStatuses.ToListAsync();
        }

            public async Task TogglePaymentStatus(int orderId)
            {
                var order = await _db.Orders.FindAsync(orderId);
                if (order == null)
                {
                    throw new InvalidOperationException($"order withi id:{orderId} does not found");
                }
                order.IsPaid = !order.IsPaid;
                await _db.SaveChangesAsync();
            }

        public async Task<IEnumerable<Order>> UserOrders(bool getAll = false)
        {
            var orders = _db.Orders
                           .Include(x => x.OrderStatus)
                           .Include(x => x.OrderDetails)
                           .ThenInclude(x => x.Product)
                           .ThenInclude(x => x.ProductCategory).AsQueryable();
            if (!getAll)
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged-in");
                orders = orders.Where(a => a.UserId == userId);
                return await orders.ToListAsync();
            }

            return await orders.ToListAsync();
        }
        private string GetUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue("UserId");
        }
    }
}
