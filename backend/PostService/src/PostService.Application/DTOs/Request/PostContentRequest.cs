namespace PostService.Application.DTOs.Request
{
    public class PostContentRequest
    {
        public Guid? Id { get; set;}
        public Guid LanguageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Slug { get; set;}
    }
}