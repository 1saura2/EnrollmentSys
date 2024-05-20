using System;
using System.Collections.Generic;

namespace ABKS_project.Models
{
    public partial class Evaluation
    {
        public int EvaluationId { get; set; }
        public int? UserBatchId { get; set; }
        public DateTime? EvaluationDate { get; set; }
        public decimal? WriteTest { get; set; }
        public decimal? DisciplineTest { get; set; }
        public decimal? FitnessTest { get; set; }

        public virtual UserBatch? UserBatch { get; set; }
    }
}
