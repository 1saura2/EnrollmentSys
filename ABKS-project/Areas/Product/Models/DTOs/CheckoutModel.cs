using System;
using System.Collections.Generic;

namespace ABKS_project.Areas.Product.Models.DTOs
{
    public partial class CheckoutModel
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? MobileNumber { get; set; }
        public string? Address { get; set; }
        public string? PaymentMethod { get; set; }
    }
}
