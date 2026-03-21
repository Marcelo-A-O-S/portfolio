using PostService.Domain.Enums;
using System.Collections.Generic;
namespace PostService.Domain.Entities
{
    public class Post
    {
        public Guid Id { get; private set; }
        public string ImgUrl { get; private set; }
        public ICollection<Category> Categories { get; private set; }
        public ICollection<Like> Likes { get; private set; }
        public ICollection<Tool> Tools { get; private set; }
        public ICollection<PostContent> PostContents { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public Status Status { get; private set; }
        public Post(Status status)
        {
            this.Id = Guid.NewGuid();
            this.CreatedAt = DateTime.UtcNow;
            this.UpdatedAt = DateTime.UtcNow;
            this.Status = status;
            this.Categories = new List<Category>();
            this.Likes = new List<Like>();
            this.Tools = new List<Tool>();
            this.PostContents = new List<PostContent>();
        }
        public void Update(Status status)
        {
            this.UpdatedAt = DateTime.UtcNow;
            this.Status = status;
        }
        public void AddTool(Tool tool)
        {
            if (this.Tools == null)
                throw new Exception("Lista de ferramentas não inicializada.");
            this.Tools.Add(tool);
        }
        public void AddCategory(Category category)
        {
            if (this.Categories == null)
                throw new Exception("Lista de categorias não inicializada.");
            this.Categories.Add(category);
        }
        public void AddPostContent(PostContent postContent)
        {
            if (this.PostContents == null)
                throw new Exception("Lista de conteudo não inicializada.");
            this.PostContents.Add(postContent);
        }
    }
}