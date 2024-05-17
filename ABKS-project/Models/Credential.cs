using System;
using System.Collections.Generic;

namespace ABKS_project.Models
{
    public partial class Credential
    {
        public int CredentialId { get; set; }
        public Guid UserId { get; set; }
        public string Password { get; set; } = null!;
        public int? RoleId { get; set; }
        public string? Token { get; set; }

        public virtual Role? Role { get; set; }
        public virtual User User { get; set; } = null!;
    }
}
