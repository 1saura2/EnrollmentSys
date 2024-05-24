using System;
using System.Collections.Generic;

namespace ABKS_project.Areas.Product.Models
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public int OrderStatusId { get; set; }
        public bool? IsDeleted { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? MobileNumber { get; set; }
        public string? Address { get; set; }
        public string? PaymentMethod { get; set; }
        public bool? IsPaid { get; set; }
    }
}
