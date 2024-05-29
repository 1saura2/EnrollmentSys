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

        public int OrderId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public int? OrderStatusId { get; set; }
        public bool? IsDeleted { get; set; }
        public string? Address { get; set; }
        public string? PaymentMethod { get; set; }
        public bool? IsPaid { get; set; }

        public virtual OrderStatus? OrderStatus { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
