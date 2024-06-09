using System;
using System.Collections.Generic;

namespace ABKS_project.Areas.Ecommerce.Models
{
    public partial class OrderStatus
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; } = null!;
    }
}
