using System;
using System.Collections.Generic;

namespace ABKS_project.Areas.Product.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
            ShoppingCarts = new HashSet<ShoppingCart>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal ProductPrice { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductImg { get; set; }
        public int? ProductCategoryId { get; set; }
        public int? InStock { get; set; }

        public virtual ProductCategory? ProductCategory { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}
