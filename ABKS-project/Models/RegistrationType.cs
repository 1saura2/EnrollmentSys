using System;
using System.Collections.Generic;

namespace ABKS_project.Models
{
    public partial class RegistrationType
    {
        public RegistrationType()
        {
            UserRegistrationTypes = new HashSet<UserRegistrationType>();
        }

        public int RegistrationTypeId { get; set; }
        public string TypeName { get; set; } = null!;

        public virtual ICollection<UserRegistrationType> UserRegistrationTypes { get; set; }
    }
}
