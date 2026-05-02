using System.Text.Json.Serialization;
namespace PostService.Domain.Entities
{
    public class ToolContent
    {
        public Guid Id { get; private set; }
        public Guid ToolId { get; private set; }
        [JsonIgnore]
        public Tool Tool { get; private set; } 
        public Guid LanguageId {get; private set;}
        public Language Language { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string? Content { get; private set; }
        public List<string> ImagesUrls { get; private set; }
        public string Slug { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public ToolContent(Guid toolId, Guid languageId, string name, string description, string content, string slug)
        {
            this.ToolId = toolId;
            this.LanguageId = languageId;
            this.Name = name;
            this.Description = description;
            this.Content = content;
            this.ImagesUrls = new List<string>();
            this.Slug = slug;
            this.CreatedAt = DateTime.UtcNow;
            this.UpdatedAt = DateTime.UtcNow;
        }
        public void Update(Guid languageId, string name, string description, string content, string slug)
        {
            this.LanguageId = languageId;
            this.Name = name;
            this.Description = description;
            this.Content = content;
            this.Slug = slug;
            this.UpdatedAt = DateTime.UtcNow;
        }
        public void SetImagesUrls(List<string> imagesUrls){
            if(this.ImagesUrls == null)
                throw new Exception("Lista de imagens envolvendo o conteúdo  não foi inicializada.");
            this.ImagesUrls = imagesUrls;
        }
        public void AddImagesUrls(string imagesUrl){
            if(this.ImagesUrls == null)
                throw new Exception("Lista de imagens envolvendo o conteúdo  não foi inicializada.");
            this.ImagesUrls.Add(imagesUrl);
        }
        public List<string> ValidateContentImages(){
            if(this.ImagesUrls == null)
                throw new Exception("Lista de imagens envolvendo o conteúdo  não foi inicializada.");
            if(string.IsNullOrEmpty(this.Content))
                throw new Exception("O conteúdo da ferramenta não pode ser vazio.");
            return this.ImagesUrls
                .Where(x => !this.Content.Contains(x))
                .ToList();
        }
    }
}