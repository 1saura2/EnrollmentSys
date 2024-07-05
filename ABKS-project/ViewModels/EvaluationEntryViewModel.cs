using System;
using System.ComponentModel.DataAnnotations;

namespace ABKS_project.ViewModels
{
    public class EvaluationEntryViewModel
    {
        public int EvaluationId { get; set; }
        public int? UserBatchId { get; set; }

        [Display(Name = "Evaluation Date")]
        [Required(ErrorMessage = "Evaluation Date is required.")]
        public DateTime? EvaluationDate { get; set; }

        [Display(Name = "Write Test")]
        [Range(0, 100, ErrorMessage = "Write Test must be between 0 and 100.")]
        public decimal? WriteTest { get; set; }

        [Display(Name = "Discipline Test")]
        [Range(0, 100, ErrorMessage = "Discipline Test must be between 0 and 100.")]
        public decimal? DisciplineTest { get; set; }

        [Display(Name = "Fitness Test")]
        [Range(0, 100, ErrorMessage = "Fitness Test must be between 0 and 100.")]
        public decimal? FitnessTest { get; set; }
    }
}
