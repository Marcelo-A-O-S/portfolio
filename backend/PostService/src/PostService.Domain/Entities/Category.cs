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
            this.CreatedAt =  DateTime.UtcNow;
        }
    }
}