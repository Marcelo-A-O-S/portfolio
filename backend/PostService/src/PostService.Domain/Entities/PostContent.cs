using System.Text.Json.Serialization;
namespace PostService.Domain.Entities
{
    public class PostContent
    {
        public Guid Id { get; private set; }
        public Guid PostId { get; private set; }
        [JsonIgnore]
        public Post Post { get; private set; }
        public string Language { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Content { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public PostContent(Guid postId, string language, string title, string description, string content)
        {
            this.Id = Guid.NewGuid();
            this.PostId = postId;
            this.Language = language;
            this.Title = title;
            this.Description = description;
            this.Content = content;
            this.CreatedAt = DateTime.UtcNow;
            this.UpdatedAt = DateTime.UtcNow;
        }
    }
}