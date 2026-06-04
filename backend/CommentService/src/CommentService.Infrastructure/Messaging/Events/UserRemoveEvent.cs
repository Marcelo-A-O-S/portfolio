namespace CommentService.Infrastructure.Messaging.Events
{
    public class UserRemoveEvent
    {
        public Guid UserId { get; set; }
    }
}