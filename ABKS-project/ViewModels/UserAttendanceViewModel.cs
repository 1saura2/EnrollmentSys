namespace ABKS_project.ViewModels
{
    public class UserAttendanceViewModel
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsPresent { get; set; }
    }
}
