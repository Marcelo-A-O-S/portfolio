namespace PostService.Domain.Queries
{
    public class CategoryContentView
    {
        public Guid Id { get;  set; }
        public LanguageView Language { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}