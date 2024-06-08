using System;
using System.Collections.Generic;

namespace ABKS_project.Areas.Ecommerce.Models
{
    public partial class Checkout
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string MobileNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PaymentMethod { get; set; } = null!;
    }
}
