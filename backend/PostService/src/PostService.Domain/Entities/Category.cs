namespace PostService.Domain.Entities
{
    public class Category
    {
        public Guid Id { get; private set; }
        public string Name {get; private set;}
        public Category(string name)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
        }
        public void Update(string name)
        {
            this.Name = name;
        }
    }
}