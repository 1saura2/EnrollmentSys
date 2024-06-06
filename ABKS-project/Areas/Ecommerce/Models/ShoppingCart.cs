using System;
using System.Collections.Generic;

namespace ABKS_project.Areas.Ecommerce.Models
{
    public partial class ShoppingCart
    {
        public ShoppingCart()
        {
            CartDetails = new HashSet<CartDetail>();
        }

        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public bool IsDeleted { get; set; }

        public virtual ICollection<CartDetail> CartDetails { get; set; }
    }
}
