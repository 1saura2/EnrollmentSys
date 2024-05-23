using System;
using System.Collections.Generic;

namespace ABKS_project.Areas.Product.Models.DTOs
{
    public partial class ProductDto
    {
        public int ProductDtoid { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal ProductPrice { get; set; }
        public string? ProductImage { get; set; }
        public int ProductCategoryId { get; set; }
    }
}
