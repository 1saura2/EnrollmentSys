using System;
using System.Collections.Generic;

namespace ABKS_project.Models
{
    public partial class ShoppingCart
    {
        public int ShoppingCartId { get; set; }
        public int UserId { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
