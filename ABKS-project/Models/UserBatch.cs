using System;
using System.Collections.Generic;

namespace ABKS_project.Models
{
    public partial class UserBatch
    {
        public UserBatch()
        {
            Evaluations = new HashSet<Evaluation>();
        }

        public int UserBatchId { get; set; }
        public Guid? UserId { get; set; }
        public int? BatchId { get; set; }

        public virtual Batch? Batch { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Evaluation> Evaluations { get; set; }
    }
}
