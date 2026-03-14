namespace PostService.Application.DTOs.Request
{
    public class CategoryRequest
    {
        public Guid? Id { get; private set; }
        public List<CategoryContentRequest> CategoryContents { get; set; }
    }
}