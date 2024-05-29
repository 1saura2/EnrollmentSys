using System;
using System.Collections.Generic;

namespace ABKS_project.Areas.Ecommerce.Models
{
    public partial class UserCart
    {
        public int UserCartId { get; set; }
        public Guid UserId { get; set; }
    }
}
