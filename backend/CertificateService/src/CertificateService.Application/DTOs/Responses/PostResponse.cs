namespace CertificateService.Application.DTOs.Responses
{
    public class PostResponse
    {
        public Guid Id { get;  set; }
        public MediaResponse Media { get; set; }
        public List<PostContentResponse> PostContents { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
    }
}