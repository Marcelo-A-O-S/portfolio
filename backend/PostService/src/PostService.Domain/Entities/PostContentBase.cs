using System.Collections.Generic;
namespace PostService.Domain.Entities
{
    public abstract class PostContentBase
    {
        public Guid Id { get; protected set; }
        public Guid LanguageId { get; protected set; }
        public Language Language { get; protected set; }
        public string Title { get; protected set; }
        public string Description { get; protected set; }
        public string Content { get; protected set; }
        public List<string> ImagesUrls { get; protected set; }
        public string Slug { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }

        public void SetImagesUrls(List<string> imagesUrls)
        {
            if(this.ImagesUrls == null)
                throw new Exception("Lista de imagens envolvendo o conteúdo não foi inicializada.");
            this.ImagesUrls = imagesUrls;
            this.UpdatedAt = DateTime.UtcNow;
        }
        public void AddImageUrl(string imageUrl)
        {
            if(this.ImagesUrls == null)
                throw new Exception("Lista de imagens envolvendo o conteúdo  não foi inicializada.");
            this.ImagesUrls.Add(imageUrl);
        }
        public List<string> ValidateContentImages()
        {
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