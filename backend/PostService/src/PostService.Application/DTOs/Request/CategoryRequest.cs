namespace PostService.Application.DTOs.Request
{
    public class CategoryRequest
    {
        public Guid? Id { get; set; }
        public List<CategoryContentRequest> CategoryContents { get; set; }
    }
}