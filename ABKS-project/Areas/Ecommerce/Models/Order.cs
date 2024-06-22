using System;
using System.Collections.Generic;

namespace ABKS_project.Areas.Ecommerce.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public int OrderStatusId { get; set; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string MobileNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PaymentMethod { get; set; } = null!;
        public bool IsPaid { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual OrderStatus OrderStatus { get; set; } = null!;
    }
}
