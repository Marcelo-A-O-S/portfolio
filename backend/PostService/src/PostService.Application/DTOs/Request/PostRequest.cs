namespace PostService.Application.DTOs.Request
{
    public class PostRequest : BaseRequest
    {
        public List<PostContentRequest> PostContents { get; set; }
        public List<ToolRequest> Tools { get; set; }
    }
}