using System;
using System.Collections.Generic;

namespace ABKS_project.Areas.Product.Models
{
    public partial class StockDisplayModelDto
    {
        public int StockDtoid { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; } = null!;
    }
}
