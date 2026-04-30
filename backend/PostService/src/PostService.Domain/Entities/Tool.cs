using PostService.Domain.Enums;
namespace PostService.Domain.Entities
{
    public class Tool
    {
        public Guid Id { get; private set; }
        public string ImgUrl { get; private set; } 
        public ICollection<ToolContent> ToolContents { get; private set;} 
        public ICollection<Category> Categories { get; private set;}
        public DateTime CreatedAt { get; private set; }
        public Status Status { get; private set; }
        public Tool( Status status)
        {
            this.CreatedAt = DateTime.UtcNow;
            this.ToolContents = new List<ToolContent>();
            this.Categories = new List<Category>();
            this.ImgUrl = "";
            this.Status = status;
        }
        public void Update(string imgUrl, Status status)
        {
            this.ImgUrl = imgUrl;
            this.Status = status;
        }
        public void AddImgUrl(string imgUrl)
        {
            this.ImgUrl = imgUrl;
        }
        public void AddCategory(Category category)
        {
            if(this.Categories == null)
                throw new Exception("Lista de categorias não inicializada.");
            this.Categories.Add(category);
        }
        public void AddToolContent(ToolContent toolContent)
        {
            if(this.ToolContents == null)
                throw new Exception("Lista de conteudo não inicializada.");
            this.ToolContents.Add(toolContent);
        }
        public void ValidateCategories(IEnumerable<Guid> categoryIds)
        {
            if(this.Categories == null)
                throw new Exception("Lista de categorias não inicializada.");
            var ids = categoryIds.ToHashSet();
            var toRemove = this.Categories
                .Where(c => !ids.Contains(c.Id))
                .ToList();
            foreach(var category in toRemove)
                this.Categories.Remove(category);
        }
        public void ValidateToolContents(IEnumerable<Guid> toolContentIds)
        {
            if(this.ToolContents == null)
                throw new Exception("Lista de conteudo não inicializada.");
            var ids = toolContentIds.ToHashSet();
            var toRemove = this.ToolContents
                .Where(tc => !ids.Contains(tc.Id))
                .ToList();
            foreach(var toolContent in toRemove)
                this.ToolContents.Remove(toolContent);
        }
    }
}