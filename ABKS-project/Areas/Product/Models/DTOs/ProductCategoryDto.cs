using System;
using System.Collections.Generic;

namespace ABKS_project.Areas.Product.Models.DTOs
{
    public partial class ProductCategoryDto
    {
        public int ProductCategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
    }
}
