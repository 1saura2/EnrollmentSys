using System;
using System.Collections.Generic;

namespace ABKS_project.Models
{
    public partial class UserRegistrationType
    {
        public UserRegistrationType()
        {
            Evaluations = new HashSet<Evaluation>();
        }

        public int UserRegistrationTypeId { get; set; }
        public int? UserId { get; set; }
        public int? RegistrationTypeId { get; set; }

        public virtual RegistrationType? RegistrationType { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Evaluation> Evaluations { get; set; }
    }
}
