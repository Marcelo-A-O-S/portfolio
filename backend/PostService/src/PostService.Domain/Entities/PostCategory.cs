namespace PostService.Domain.Entities
{
    public class PostCategory
    {
        public Guid Id { get; set;}
        public Guid PostId { get;  set; }
        public Post Post { get;  set;}
        public Guid CategoryId { get;  set;}
        public Category Category { get;  set;}
    }
}