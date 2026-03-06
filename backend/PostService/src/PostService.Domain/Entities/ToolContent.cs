namespace PostService.Domain.Entities
{
    public class ToolContent
    {
        public Guid Id { get; private set; }
        public Guid ToolId { get; private set; }
        public Tool Tool { get; private set; }  
        public string Language { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string? Content { get; private set; }
        public string Slug { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public ToolContent(Guid toolId, string language, string name, string description, string content, string slug)
        {
            this.Id = Guid.NewGuid();
            this.ToolId = toolId;
            this.Language = language;
            this.Name = name;
            this.Description = description;
            this.Content = content;
            this.Slug = slug;
            this.CreatedAt = DateTime.UtcNow;
            this.UpdatedAt = DateTime.UtcNow;
        }
        public void Update(string language, string name, string description, string content, string slug)
        {
            this.Language = language;
            this.Name = name;
            this.Description = description;
            this.Content = content;
            this.Slug = slug;
            this.UpdatedAt = DateTime.UtcNow;
        }
    }
}