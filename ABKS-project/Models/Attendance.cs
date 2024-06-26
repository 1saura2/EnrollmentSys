using System;
using System.Collections.Generic;

namespace ABKS_project.Models
{
    public partial class Attendance
    {
        public int AttendanceId { get; set; }
        public int? UserBatchId { get; set; }
        public DateTime? AttendanceDate { get; set; }
        public bool? IsPresent { get; set; }

        public virtual UserBatch? UserBatch { get; set; }
    }
}
