using System.ComponentModel.DataAnnotations;

namespace ABKS_project.Models.MetaData
{
    public class CredentialMetaData
    {
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression("^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }

    
}
