using System;
using System.Collections.Generic;

namespace ABKS_project.Models
{
    public partial class OrderStatus
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; } = null!;
    }
}
