namespace PostService.Application.DTOs.Request
{
    public class ToolRequest : BaseRequest
    {
        
        public List<ToolContentRequest> ToolContents { get; set; }
    }
}