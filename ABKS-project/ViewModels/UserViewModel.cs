using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ABKS_project.ViewModels
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression("^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Citizenship PDF is required")]
        [AllowedExtensions(new string[] { ".pdf" }, ErrorMessage = "Only PDF files are allowed.")]
        [MaxFileSize(10 * 1024 * 1024, ErrorMessage = "Maximum allowed file size is 10 MB.")]
        public IFormFile CitizenshipPdf { get; set; }

        [RegularExpression("^\\d{10}$", ErrorMessage = "Please provide valid phone number")]
        [Required(ErrorMessage = "Contact Number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Education is required")]
        public string Education { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(16, 21, ErrorMessage = "Age must be between 16 and 21")]
        public int? Age { get; set; }
    }
}
