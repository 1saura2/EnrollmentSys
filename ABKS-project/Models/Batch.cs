using System;
using System.Collections.Generic;

namespace ABKS_project.Models
{
    public partial class Batch
    {
        public int BatchId { get; set; }
        public string BatchName { get; set; } = null!;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
