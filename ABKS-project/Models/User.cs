using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ABKS_project.Models
{
    public partial class User
    {
        public User()
        {
            UserBatches = new HashSet<UserBatch>();
        }

        public int UserId { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression("^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Citizenship Photo is required")]
        public string CitizenshipPhoto { get; set; }

        [Required(ErrorMessage = "Contact Number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Education is required")]
        public string Education { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(16, 21, ErrorMessage = "Age must be between 16 and 21")]
        public int Age { get; set; }
        public bool? IsVerified { get; set; }

        public virtual ICollection<UserBatch> UserBatches { get; set; }
    }
}
