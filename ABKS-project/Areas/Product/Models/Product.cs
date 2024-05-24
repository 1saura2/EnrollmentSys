using System;
using System.Collections.Generic;

namespace ABKS_project.Areas.Product.Models
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal ProductPrice { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductImage { get; set; }
        public int ProductCategoryId { get; set; }
        public string? Category { get; set; }
        public int? Stock { get; set; }
        public string? CategoryName { get; set; }
        public int? Quantity { get; set; }
    }
}
