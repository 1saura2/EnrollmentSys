using System;
using System.Collections.Generic;

namespace ABKS_project.Areas.Product.Models
{
    public partial class ShoppingCart
    {
        public int ShoppingCartId { get; set; }
        public Guid UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}
