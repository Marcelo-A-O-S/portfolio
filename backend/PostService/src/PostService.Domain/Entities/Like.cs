namespace PostService.Domain.Entities
{
    public class Like
    {
        public Guid Id { get; private set; }
        public Guid PostId { get; private set;}
        public Post Post { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Like(Guid userId, Guid postId)
        {
            this.UserId = userId;
            this.PostId = postId;
            this.CreatedAt = DateTime.UtcNow;
        }
    }
}