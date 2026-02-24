namespace AuthService.Application.DTOs.Response
{
    public class AuthResponse
    {
        public string AccessToken { get;set;}
        public string RefreshToken { get; set;}
    }
}