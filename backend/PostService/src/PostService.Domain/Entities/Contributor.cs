namespace PostService.Domain.Entities
{
    public class Contributor
    {
        public Guid Id { get; private set; }
        public Guid? UserId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string ProfileUrl { get; private set;} 
    }
}