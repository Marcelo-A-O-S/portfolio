using CommentService.Domain.Enums;
namespace CommentService.Domain.Queries
{
    public class CommentView
    {
        public Guid? Id { get; set; }
        public Guid TargetId { get; set; }
        public CommentType Type { get; set; }
        public string Content { get; set; }
        public ICollection<CommentView> Replies { get; set; }
    }
}