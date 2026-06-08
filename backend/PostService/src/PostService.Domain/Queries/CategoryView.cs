namespace PostService.Domain.Queries
{
    public class CategoryView
    {
        public Guid Id { get; set; }
        public ICollection<CategoryContentView> CategoryContents { get; set; }
    }
}