namespace AuthService.Application.Configurations
{
    public class InternalJWTOptions
    {
        public string Secret { get ; set; }
        public string Issuer { get; set; }
        public int InternalExpirationMinutes { get; set; }
    }
}