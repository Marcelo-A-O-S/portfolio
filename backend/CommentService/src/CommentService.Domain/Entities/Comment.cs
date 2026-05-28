namespace CommentService.Domain.Entities
{
    public class Comment
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public Guid PostId { get; private set; }
        public string Content { get; private set; }
        public ICollection<Answer> Answers { get; private set; }
        public Comment(Guid userId, Guid postId, string content)
        {
            this.Id = Guid.Empty;
            this.UserId = userId;
            this.PostId = postId;
            this.Content = content;
        }
    }
}