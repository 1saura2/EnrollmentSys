namespace ABKS_project.ViewModels
{
    public class UserViewModelForTempData
    {

        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int? Age { get; set; }
        public string? ContactNumber { get; set; }
        public string? Education { get; set; }
        public string? CitizenshipPhoto { get; set; }
    }
}
