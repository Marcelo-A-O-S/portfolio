namespace MediaService.Application.DTOs.Responses
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public int ExpireIn { get; set; }
    }
}