namespace AuthService.Application.Configurations
{
    public class JwtBearerOptions
    {
        public string Secret { get ; set; }
        public string Issuer { get; set; }
        public int ExpirationMinutes { get; set; }
    }
}