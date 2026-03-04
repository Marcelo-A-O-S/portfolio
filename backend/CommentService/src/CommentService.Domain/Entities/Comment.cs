namespace CommentService.Domain.Entities
{
    public class Comment
    {
        public Guid Id { get; private set;}
        public Guid UserId { get; private set;}
        public Guid PostId { get; private set;}
        public string Content { get; private set;}
        public ICollection<Answer> Answers { get; private set;}
    }
}