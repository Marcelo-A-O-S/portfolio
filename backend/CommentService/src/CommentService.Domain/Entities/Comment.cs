using System.Collections.Generic;
namespace CommentService.Domain.Entities
{
    public class Comment
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public Guid PostId { get; private set; }
        public string Content { get; private set; }
        public Guid? ParentCommentId { get; private set; }
        public Comment? ParentComment { get; private set; }
        public ICollection<Comment> Replies { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public Comment(Guid userId, Guid postId, string content, Guid? parentCommentId = null)
        {
            this.UserId = userId;
            this.PostId = postId;
            this.Content = content;
            this.ParentCommentId = parentCommentId;
            this.Replies = new List<Comment>();
            this.CreatedAt = DateTime.UtcNow;
            this.UpdatedAt = DateTime.UtcNow;
        }
        public void Update(string content, Guid? parentCommentId = null)
        {
            this.Content = content;
            this.ParentCommentId = parentCommentId;
            this.UpdatedAt = DateTime.UtcNow;
        }
    }
}