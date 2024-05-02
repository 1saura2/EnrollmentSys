using System;
using System.Collections.Generic;

namespace ABKS_project.Models
{
    public partial class Credential
    {
        public int CredentialId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int? RoleId { get; set; }
        public string? Token { get; set; }

        public virtual Role? Role { get; set; }
    }
}
