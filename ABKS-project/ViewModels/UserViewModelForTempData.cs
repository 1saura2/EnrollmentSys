namespace ABKS_project.ViewModels
{
    public class UserViewModelForTempData
    {

        public Guid UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; } = null!;
        public int? Age { get; set; }
        public string? ContactNumber { get; set; }
        public string? Education { get; set; }
        public string? Citizenship { get; set; }
    }
}
