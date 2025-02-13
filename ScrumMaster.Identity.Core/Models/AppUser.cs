using Microsoft.AspNetCore.Identity;

namespace ScrumMaster.Identity.Core.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegisterAt { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime LastLoginAt { get; set; }
    }
}
