using PostService.Domain.Enums;
using System.ComponentModel.DataAnnotations;

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
        public void AddCategory(Category category)
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
        public void AddLike()
        {
            this.LikeCount++;
        }
        public void RemoveLike()
        {
            if(this.LikeCount == 0)
                throw new ValidationException("Não é possivel realizar a remoção da curtida");
            this.LikeCount--;
        }
        public void AddCommentCount()
        {
            this.CommentCount++;
        }
        public void RemoveCommentCount()
        {
            if(this.CommentCount == 0)
                throw new ValidationException("Não é possivel realizar a remoção do comentário");
            this.CommentCount--;
        }
    }
}