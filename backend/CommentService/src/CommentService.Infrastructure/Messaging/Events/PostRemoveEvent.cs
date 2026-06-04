namespace CommentService.Infrastructure.Messaging.Events
{
    public class PostRemoveEvent
    {
        public Guid PostId { get; set; }
    }
}