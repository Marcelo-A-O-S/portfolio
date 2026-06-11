using PostService.Domain.Enums;

namespace PostService.Infrastructure.Messaging.Events
{
    public class LikedEvent
    {
        public Guid LikeId { get; set; }
        public Guid TargetId { get; set; }
        public Guid UserId { get; set; }
        public LikeType Type { get; set; }
    }
}