﻿using System;
using System.Collections.Generic;

namespace ABKS_project.Areas.Product.Models
{
    public partial class UpdateOrderStatusModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
