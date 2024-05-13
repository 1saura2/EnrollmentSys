using System;
using System.Collections.Generic;

namespace ABKS_project.Models
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public int OrderStatusId { get; set; }
        public bool? IsDeleted { get; set; }
        public string? OrderName { get; set; }
        public string? OrderEmail { get; set; }
        public string? OrderMobNumber { get; set; }
        public string? OrderAddress { get; set; }
        public string? OrderPaymentMethod { get; set; }
        public bool? IsPaid { get; set; }
    }
}
