namespace ScrumMaster.Identity.Infrastructure.DTO
{
    public class AuthDTO
    {
        public string jwtToken { get; set; }
        public string refreshToken { get; set; }
        public string userName { get; set; }
    }
}
