using PostService.Domain.Enums;
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
        public Post(string imgUrl, Status status)
        {
            this.CreatedAt = DateTime.UtcNow;
            this.UpdatedAt = DateTime.UtcNow;
            this.ImgUrl = imgUrl;
            this.Status = status;
            this.Categories = new List<Category>();
            this.Likes = new List<Like>();
            this.Tools = new List<Tool>();
            this.PostContents = new List<PostContent>();
        }
        public void Update(string imgUrl, Status status)
        {
            this.UpdatedAt = DateTime.UtcNow;
            this.Status = status;
            this.ImgUrl = imgUrl;
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
        public void ValidateTools(IEnumerable<Guid> toolIds)
        {
            if(this.Tools == null)
                throw new Exception("Lista de ferramentas não inicializada.");
            var ids = toolIds.ToHashSet();
            var toRemove = this.Tools
                .Where(t=> !ids.Contains(t.Id))
                .ToList();
            foreach(var tool in toRemove)
                this.Tools.Remove(tool);
        }
        public void ValidateCategories(IEnumerable<Guid> categoriesIds)
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
        public void ValidatePostContents(IEnumerable<Guid> postContentIds)
        {
            if(this.PostContents == null)
                throw new Exception("Lista de conteúdo do post não inicializada.");
            var ids = postContentIds.ToHashSet();
            var toRemove = this.PostContents
                .Where(pt => !ids.Contains(pt.Id))
                .ToList();
            foreach(var postContent in toRemove)
                this.PostContents.Remove(postContent);
        }
    }
}