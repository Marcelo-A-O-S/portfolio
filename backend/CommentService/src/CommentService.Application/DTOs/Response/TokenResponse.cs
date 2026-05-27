namespace CommentService.Application.DTOs.Response
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public int ExpireIn { get; set; }
    }
}