namespace AuthService.Application.DTOs.Response
{
    public class AuthResponse
    {
        public Guid UserId { get; set; }
        public string AccessToken { get; set; }
        public Guid RefreshTokenId { get; set; }
        public string RefreshToken { get; set; }
        public int ExpireIn { get; set; }
        public string Role { get; set; }
    }
}