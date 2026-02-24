namespace PostService.Domain.Entities
{
    public class PostCategory
    {
        public Guid Id { get; private set;}
        public Guid PostId { get; set; }
        public Guid CategoryId { get; set;}
    }
}