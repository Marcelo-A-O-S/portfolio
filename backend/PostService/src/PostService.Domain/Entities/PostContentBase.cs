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
        public ICollection<MediaProjection> Images { get; protected set; }
        public string Slug { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }

        public void SetImagesUrls(List<MediaProjection> imagesUrls)
        {
            if(this.Images == null)
                throw new Exception("Lista de imagens envolvendo o conteúdo não foi inicializada.");
            this.Images = imagesUrls;
            this.UpdatedAt = DateTime.UtcNow;
        }
        public void AddImageUrl(MediaProjection imageUrl)
        {
            if(this.Images == null)
                throw new Exception("Lista de imagens envolvendo o conteúdo  não foi inicializada.");
            this.Images.Add(imageUrl);
        }
        public List<MediaProjection> ValidateContentImages()
        {
            if(this.Images == null)
                throw new Exception("Lista de imagens envolvendo o conteúdo  não foi inicializada.");
            if(string.IsNullOrEmpty(this.Content))
                throw new Exception("O conteúdo da ferramenta não pode ser vazio.");
            return this.Images
                .Where(x => !this.Content.Contains(x.Url))
                .ToList();
        }
    }
}