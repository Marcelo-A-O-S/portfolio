namespace PostService.Domain.Entities
{
    public class Language
    {
        public Guid Id { get; private set; }
        public string Code {get; private set;}
        public string Name { get; private set;}
        public DateTime CreatedAt {get; private set;}
        public DateTime UpdatedAt {get; private set;}
        public Language(string code, string name)
        {
            this.Code = code;
            this.Name = name;
            this.CreatedAt = DateTime.UtcNow;
            this.UpdatedAt = DateTime.UtcNow;
        }
        public void Update(string code, string name)
        {
            this.Code = code;
            this.Name = name;
            this.UpdatedAt = DateTime.UtcNow;
        }
    }
}