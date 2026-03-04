namespace CommentService.Domain.Entities
{
    public class Answer
    {
        public Guid Id { get; private set;}
        public Guid UserId { get; private set;}
        public Guid CommentId { get; private set;}
        public string Content { get; private set;}
    }
}