using System.ComponentModel.DataAnnotations;

public class ResetPasswordViewModel
{
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "New Password is required.")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "Confirm Password is required.")]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }
}
