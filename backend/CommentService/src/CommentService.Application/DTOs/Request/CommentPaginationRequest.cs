using CommentService.Domain.Enums;

namespace CommentService.Application.DTOs.Request
{
    public class CommentPaginationRequest
    {
        public string TargetId { get; set; }
        public CommentType Type { get; set; }
    }
}