using System.Text.Json.Serialization;
namespace PostService.Domain.Entities
{
    public class CategoryContent
    {
        public Guid Id { get; private set; }
        public Guid CategoryId { get; private set; }
        [JsonIgnore]
        public Category Category { get; private set; }
        public Guid LanguageId {get; private set;}
        public Language Language { get; private set; }
        public string Name { get; private set; }
        public string Slug { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdateAt { get; private set; }
        public CategoryContent(){}
        public CategoryContent(Guid categoryId, Guid languageId, string name, string slug)
        {
            this.CategoryId = categoryId;
            this.LanguageId = languageId;
            this.Name = name;
            this.Slug = slug;
            this.CreatedAt = DateTime.UtcNow;
            this.UpdateAt = DateTime.UtcNow;
        }
        public void Update(Guid languageId, string name, string slug)
        {
            this.LanguageId = languageId;
            this.Name = name;
            this.Slug = slug;
            this.UpdateAt = DateTime.UtcNow;
        }
        
    }
}