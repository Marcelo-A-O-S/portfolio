namespace PostService.Domain.Entities
{
    public class Like
    {
        public Guid Id { get; private set; }
        public Guid PostId { get; private set;}
        public Post Post { get; private set; }
        public Guid UserId { get; private set; }
    }
}