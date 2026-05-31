namespace CommentService.Domain.Entities
{
    public class Like
    {
        public Guid Id { get; private set; }
        public Guid CommentId { get; private set; }
        public Comment Comment {get; private set; }
        public Guid UserId { get; private set; }
        public DateTime CreatedAt {get; private set;}
        public Like(Guid commentId, Guid userId)
        {
            this.CommentId = commentId;
            this.UserId = userId;
            this.CreatedAt = DateTime.UtcNow;
        }
    }
}