using System.ComponentModel.DataAnnotations;
using CommentService.Domain.Enums;

namespace CommentService.Application.DTOs.Request
{
    public class GetCommentRequest
    {
        [Required]
        public Guid TargetId { get; set; }
        [Required]
        public CommentType Type { get; set; }
    }
}