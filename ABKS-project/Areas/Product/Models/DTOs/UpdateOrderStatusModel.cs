using System;
using System.Collections.Generic;

namespace ABKS_project.Areas.Product.Models.DTOs
{
    public partial class UpdateOrderStatusModel
    {
        public int OrderId { get; set; }
        public int OrderStatusId { get; set; }
    }
}
