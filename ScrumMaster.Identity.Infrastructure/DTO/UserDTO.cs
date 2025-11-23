namespace ScrumMaster.Identity.Infrastructure.DTO
{
    public class UserDTO
    {
        public Guid id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public DateTime registerAt { get; set; }
        public DateTime lastLoginAt { get; set; }
    }
}
