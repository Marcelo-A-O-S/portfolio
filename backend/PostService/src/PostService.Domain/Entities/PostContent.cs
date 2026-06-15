using System.Text.Json.Serialization;
namespace PostService.Domain.Entities
{
    public class PostContent : PostContentBase
    {
        public Guid PostId { get; private set; }
        [JsonIgnore]
        public Post Post { get; private set; }
        public PostContent(Guid postId, Guid languageId, string title, string description, string content, string slug)
        {
            this.PostId = postId;
            this.LanguageId = languageId;
            this.Title = title;
            this.Description = description;
            this.Content = content;
            this.Images = new List<MediaProjection>();
            this.Slug = slug;
            this.CreatedAt = DateTime.UtcNow;
            this.UpdatedAt = DateTime.UtcNow;
        }
        public void Update(Guid languageId, string title, string description, string content, string slug)
        {
            this.LanguageId = languageId;
            this.Title = title;
            this.Description = description;
            this.Content = content;
            this.Slug = slug;
            this.UpdatedAt = DateTime.UtcNow;
        }
    }
}