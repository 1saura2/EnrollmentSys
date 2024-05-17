using System;
using System.Collections.Generic;

namespace ABKS_project.Models
{
    public partial class User
    {
        public User()
        {
            Attendances = new HashSet<Attendance>();
            Credentials = new HashSet<Credential>();
            UserBatches = new HashSet<UserBatch>();
        }

        public Guid UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; } = null!;
        public int? Age { get; set; }
        public string? ContactNumber { get; set; }
        public string? Education { get; set; }
        public string? Citizenship { get; set; }
        public bool? IsVerified { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<Credential> Credentials { get; set; }
        public virtual ICollection<UserBatch> UserBatches { get; set; }
    }
}
