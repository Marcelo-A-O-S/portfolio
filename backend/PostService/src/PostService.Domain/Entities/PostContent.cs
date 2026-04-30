using System.Text.Json.Serialization;
namespace PostService.Domain.Entities
{
    public class PostContent
    {
        public Guid Id { get; private set; }
        public Guid PostId { get; private set; }
        [JsonIgnore]
        public Post Post { get; private set; }
        public Guid LanguageId { get; private set;}
        public Language Language { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Content { get; private set; }
        public List<string> ImagesUrls { get; private set; }
        public string Slug { get; private set;}
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public PostContent(Guid postId, Guid languageId, string title, string description, string content, string slug)
        {
            this.PostId = postId;
            this.LanguageId = languageId;
            this.Title = title;
            this.Description = description;
            this.Content = content;
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
        public void ValidateImagesUrls()
        {
            
        }
    }
}