using System;
using System.Collections.Generic;

namespace ABKS_project.Areas.Product.Models
{
    public partial class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string? ProductName { get; set; }
        public int OrderId { get; set; }
    }
}
