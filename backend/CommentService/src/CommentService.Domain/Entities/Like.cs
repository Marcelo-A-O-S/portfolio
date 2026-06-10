using CommentService.Domain.Enums;
namespace CommentService.Domain.Entities
{
    public class Like
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public Guid TargetId { get; private set; }
        public LikeType Type { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Like(Guid targetId, LikeType type, Guid userId)
        {
            this.TargetId = targetId;
            this.Type = type;
            this.UserId = userId;
            this.CreatedAt = DateTime.UtcNow;
        }
    }
}