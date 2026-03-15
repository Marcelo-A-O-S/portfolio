namespace PostService.Domain.Entities
{
    public class Tool
    {
        public Guid Id { get; private set; }
        public ICollection<ToolContent> ToolContents { get; private set;} 
        public ICollection<Category> Categories { get; private set;}
        public DateTime CreatedAt { get; private set; }
        public Tool()
        {
            this.Id = Guid.NewGuid();
            this.CreatedAt = DateTime.UtcNow;
            this.ToolContents = new List<ToolContent>();
            this.Categories = new List<Category>();
        }
    }
}