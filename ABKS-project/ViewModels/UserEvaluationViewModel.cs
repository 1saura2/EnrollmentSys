namespace ABKS_project.ViewModels
{
    public class UserEvaluationViewModel
    {
        public List<WeeklyData> WeeklyData { get; set; } = new();
        public List<string> Labels { get; set; } = new();
        public List<double> AverageScores { get; set; }
    }

    public class WeeklyData
    {
        public DateTime EvaluationDate { get; set; }
        public decimal DisciplineTest { get; set; }
        public decimal FitnessTest { get; set; }
        public decimal WriteTest { get; set; }
        public double AttendancePercentage { get; set; }
    }
}
