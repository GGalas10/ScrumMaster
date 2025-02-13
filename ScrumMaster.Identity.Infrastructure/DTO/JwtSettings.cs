namespace ScrumMaster.Identity.Infrastructure.DTO
{
    public class JwtSettings
    {
        public string secret { get; set; }
        public int ExpirationMinutes { get; set; }
    }
}
