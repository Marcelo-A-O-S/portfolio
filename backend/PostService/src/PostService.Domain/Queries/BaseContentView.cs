namespace PostService.Domain.Queries
{
    public class BaseContentView
    {
        public Guid Id { get; set; }
        public LanguageView Language { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public List<MediaView> Images { get; set; }
        public string Slug { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}