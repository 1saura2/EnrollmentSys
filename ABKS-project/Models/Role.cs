using System;
using System.Collections.Generic;

namespace ABKS_project.Models
{
    public partial class Role
    {
        public Role()
        {
            Credentials = new HashSet<Credential>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;

        public virtual ICollection<Credential> Credentials { get; set; }
    }
}
