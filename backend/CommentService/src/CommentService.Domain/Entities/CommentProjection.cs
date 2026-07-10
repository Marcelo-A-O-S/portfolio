namespace CommentService.Domain.Entities
{
    public class CommentProjection
    {
        public Guid Id { get; private set; }
        public Guid CommentId { get; private set; }
        public Comment Comment { get; private set; }
        public string Username { get; private set; }
        public string ProfileUrl { get; private set;}
        public DateTime CreatedAt { get; private set; }
    }
}