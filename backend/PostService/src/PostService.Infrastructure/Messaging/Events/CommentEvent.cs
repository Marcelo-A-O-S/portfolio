using PostService.Domain.Enums;
namespace PostService.Infrastructure.Messaging.Events
{
    public class CommentEvent
    {
        public Guid LikeId { get; set; }
        public Guid TargetId { get; set; }
        public Guid UserId { get; set; }
        public CommentType Type { get; set; }
    }
}