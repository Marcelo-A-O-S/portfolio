using CommentService.Domain.Enums;
namespace CommentService.Domain.Entities
{
    public class Comment
    {
        public Guid Id { get; private set; }
        public Guid TargetId { get; private set; }
        public Guid UserProjectionId { get; private set; }
        public UserProjection UserProjection { get; private set; }
        public CommentType Type { get; private set; }
        public string Content { get; private set; }
        public CommentStatus CommentStatus { get; private set; }
        public CommentDeletionType CommentDeletion { get; private set; }
        public Guid? ParentCommentId { get; private set; }
        public Comment? ParentComment { get; private set; }
        public ICollection<Comment> Replies { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }
        public Comment(
            Guid targetId,
            CommentType type,
            string content,
            Guid? parentCommentId = null)
        {
            this.TargetId = targetId;
            this.Type = type;
            this.Content = content;
            this.ParentCommentId = parentCommentId;
            this.Replies = new List<Comment>();
            this.CommentDeletion = CommentDeletionType.None;
            this.CommentStatus = CommentStatus.Active;
            this.CreatedAt = DateTime.UtcNow;
            this.UpdatedAt = DateTime.UtcNow;
        }
        public void Update(string content, Guid? parentCommentId = null)
        {
            if (CommentStatus == CommentStatus.Deleted)
                throw new InvalidOperationException("Deleted comments cannot be edited.");
            this.Content = content;
            this.ParentCommentId = parentCommentId;
            this.UpdatedAt = DateTime.UtcNow;
        }
        public void SetUserProjectionId(Guid userProjectionId)
        {
            this.UserProjectionId = userProjectionId;
        }
        public void DeleteByUser()
        {
            this.Delete(CommentDeletionType.User);
        }
        public void DeleteByModerator()
        {
            this.Delete(CommentDeletionType.Moderator);
        }
        public void Restore()
        {
            this.CommentDeletion = CommentDeletionType.None;
            this.CommentStatus = CommentStatus.Active;
            this.DeletedAt = null;
        }
        private void Delete(CommentDeletionType type)
        {
            CommentStatus = CommentStatus.Deleted;
            CommentDeletion = type;
            DeletedAt = DateTime.UtcNow;
        }
    }
}