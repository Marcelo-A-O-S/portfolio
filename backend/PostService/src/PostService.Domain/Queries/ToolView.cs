namespace PostService.Domain.Queries
{
    public class ToolView
    {
        public Guid Id { get; set; }
        public ICollection<ToolContentView> ToolContents { get; set; }
    }
}