using System.ComponentModel.DataAnnotations;

namespace ABKS_project.Models.DTOs
{
    public class CheckoutModel
    {
        [Required]
        [MaxLength(30)]
        public string? OrderName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(30)]
        public string? OrderEmail { get; set; }
        [Required]
        public string? OrderMobileNumber { get; set; }
        [Required]
        [MaxLength(200)]
        public string? OrderAddress { get; set; }

        [Required]
        public string? OrderPaymentMethod { get; set; }
    }
}
