using System;
using System.Collections.Generic;

namespace ABKS_project.Models
{
    public partial class CartDetail
    {
        public int CartId { get; set; }
        public int ShoppingCartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
