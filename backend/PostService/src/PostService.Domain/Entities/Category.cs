namespace PostService.Domain.Entities
{
    public class Category
    {
        public Guid Id { get; private set; }
        public ICollection<CategoryContent> CategoryContents { get; private set;}
        public DateTime CreatedAt { get; private set; }
        public Category()
        {
            this.Id = Guid.NewGuid();
            this.CategoryContents = new List<CategoryContent>();
            this.CreatedAt =  DateTime.UtcNow;
        }
        public void AddCategoryContent(CategoryContent categoryContent)
        {
            if(this.CategoryContents == null)
                throw new Exception("Lista de conteudo da categoria não inicializada.");
            this.CategoryContents.Add(categoryContent);
        }
    }
}