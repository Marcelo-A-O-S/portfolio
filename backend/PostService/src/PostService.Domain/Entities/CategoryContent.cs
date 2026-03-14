using System.Text.Json.Serialization;
namespace PostService.Domain.Entities
{
    public class CategoryContent
    {
        public Guid Id { get; private set; }
        public Guid CategoryId { get; private set; }
        [JsonIgnore]
        public Category Category { get; private set; }
        public string Language { get; private set; }
        public string Name { get; private set; }
        public string Slug { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdateAt { get; private set; }
        public CategoryContent()
        {
        }
        public CategoryContent(Guid categoryId, string language, string name, string slug)
        {
            this.Id = Guid.NewGuid();
            this.CategoryId = categoryId;
            this.Language = language;
            this.Name = name;
            this.Slug = slug;
            this.CreatedAt = DateTime.UtcNow;
            this.UpdateAt = DateTime.UtcNow;
        }
        public void Update(string language, string name, string slug)
        {
            this.Language = language;
            this.Name = name;
            this.Slug = slug;
            this.UpdateAt = DateTime.UtcNow;
        }
        
    }
}