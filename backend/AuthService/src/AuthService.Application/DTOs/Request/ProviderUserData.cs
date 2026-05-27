namespace AuthService.Application.DTOs.Request
{
    public class ProviderUserData
    {
        public string ProviderId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string? PictureUrl { get; set; }
        public bool VerifiedAccount { get; set; }
    }
}