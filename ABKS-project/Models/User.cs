using System;
using System.Collections.Generic;

namespace ABKS_project.Models
{
    public partial class User
    {
        public User()
        {
            UserRegistrationTypes = new HashSet<UserRegistrationType>();
        }

        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int? Age { get; set; }
        public string? ContactNumber { get; set; }
        public string? Education { get; set; }
        public string? CitizenshipPhoto { get; set; }
        public bool? IsVerified { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<UserRegistrationType> UserRegistrationTypes { get; set; }
    }
}
