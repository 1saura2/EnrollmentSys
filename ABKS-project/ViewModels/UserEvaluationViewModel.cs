using System;
using System.Collections.Generic;

namespace ABKS_project.ViewModels
{
    public class UserEvaluationViewModel
    {
        public IEnumerable<WeeklyEvaluationData> WeeklyEvaluations { get; set; }
        public IEnumerable<WeeklyAttendanceData> WeeklyAttendances { get; set; }
        public IEnumerable<string> Labels { get; set; } // Labels for the chart
    }

    public class WeeklyEvaluationData
    {
        public int Week { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal AverageDisciplineTest { get; set; }
        public decimal AverageFitnessTest { get; set; }
        public decimal AverageWriteTest { get; set; }
    }

    public class WeeklyAttendanceData
    {
        public int Week { get; set; }
        public double AttendancePercentage { get; set; }
    }
}
