namespace PostService.Domain.Queries
{
    public class ToolContentView
    {
        public Guid Id { get; set; }
        public LanguageView Language { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Content { get; set; }
        public List<string> ImagesUrls { get; set; }
        public string Slug { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}