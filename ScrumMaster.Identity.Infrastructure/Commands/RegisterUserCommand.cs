namespace ScrumMaster.Identity.Infrastructure.Commands
{
    public class RegisterUserCommand
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string confirmPassword { get; set; }
    }
}
