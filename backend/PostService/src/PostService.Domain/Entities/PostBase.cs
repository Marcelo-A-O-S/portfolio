using PostService.Domain.Enums;

namespace PostService.Domain.Entities
{
    public abstract class PostBase
    {
        public Guid Id { get; protected set; }
        public string ImgUrl { get; protected set;}
        public int LikeCount { get; protected set; }
        public int CommentCount { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }
        public Status Status { get; protected set; }
        public ICollection<Category> Categories { get; protected set; }
        protected void Update(string imgUrl, Status status)
        {
            this.UpdatedAt = DateTime.UtcNow;
            this.Status = status;
            this.ImgUrl = imgUrl;
        }
        protected void AddImgUrl(string imgUrl)
        {
            this.ImgUrl = imgUrl;
        }
        protected void AddCategory(Category category)
        {
            if (this.Categories == null)
                throw new Exception("Lista de categorias não inicializada.");
            this.Categories.Add(category);
        }
        protected void ValidateCategories(IEnumerable<Guid> categoriesIds)
        {
            if(this.Categories == null)
                throw new Exception("Lista de categorias não inicializada.");
            var ids = categoriesIds.ToHashSet();
            var toRemove = this.Categories
                .Where(c=> !ids.Contains(c.Id))
                .ToList();
            foreach(var category in toRemove)
                this.Categories.Remove(category);
        }
    }
}