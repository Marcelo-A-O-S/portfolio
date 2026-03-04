namespace PostService.Domain.Entities
{
    public class Tool
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string? Content { get; private set; }
        public string Slug { get; private set; }
        public Tool(string name, string description, string content, string slug)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.Description = description;
            this.Content = content;
            this.Slug = slug;
        }
        public void Update(string name, string description, string content, string slug)
        {
            this.Name = name;
            this.Description = description;
            this.Content = content;
            this.Slug = slug;
        }
    }
}