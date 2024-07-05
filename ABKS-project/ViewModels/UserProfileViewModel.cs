namespace ABKS_project.ViewModels
{
    public class UserProfileViewModel
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;
        public string Education { get; set; } = null!;
        public string Citizenship { get; set; }
        public ICollection<Batch> Batches { get; set; } = new List<Batch>();
    }
}
