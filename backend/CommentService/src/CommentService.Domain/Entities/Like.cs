namespace CommentService.Domain.Entities
{
    public class Like
    {
        public Guid Id { get; private set;}
        public Guid CommentId { get; private set;}
        public Guid AnswerId { get; private set;}
        public Guid UserId { get; private set;}
    }
}