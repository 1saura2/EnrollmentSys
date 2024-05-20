﻿using System;
using System.Collections.Generic;

namespace ABKS_project.Areas.Product.Models
{
    public partial class CheckoutModel
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? ContactNumber { get; set; }
        public string? Address { get; set; }
        public string? PaymentMethod { get; set; }
    }
}
