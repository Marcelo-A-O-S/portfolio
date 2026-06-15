using System.Text.Json.Serialization;
namespace PostService.Domain.Entities
{
    public class ToolContent : PostContentBase
    {
        public Guid ToolId { get; private set; }
        [JsonIgnore]
        public Tool Tool { get; private set; } 
        public string Name { get; private set; }
        public ToolContent(Guid toolId, Guid languageId, string name, string title, string description, string content, string slug)
        {
            this.ToolId = toolId;
            this.LanguageId = languageId;
            this.Name = name;
            this.Title = title;
            this.Description = description;
            this.Content = content;
            this.Images = new List<MediaProjection>();
            this.Slug = slug;
            this.CreatedAt = DateTime.UtcNow;
            this.UpdatedAt = DateTime.UtcNow;
        }
        public void Update(Guid languageId, string name, string title, string description, string content, string slug)
        {
            this.LanguageId = languageId;
            this.Name = name;
            this.Title = title;
            this.Description = description;
            this.Content = content;
            this.Slug = slug;
            this.UpdatedAt = DateTime.UtcNow;
        }
    }
}