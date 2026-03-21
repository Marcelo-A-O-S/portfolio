namespace PostService.Domain.Entities
{
    public class Tool
    {
        public Guid Id { get; private set; }
        public string ImgUrl { get; private set; } 
        public ICollection<ToolContent> ToolContents { get; private set;} 
        public ICollection<Category> Categories { get; private set;}
        public DateTime CreatedAt { get; private set; }
        public Tool(string imgUrl)
        {
            this.Id = Guid.NewGuid();
            this.CreatedAt = DateTime.UtcNow;
            this.ToolContents = new List<ToolContent>();
            this.Categories = new List<Category>();
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
    }
}