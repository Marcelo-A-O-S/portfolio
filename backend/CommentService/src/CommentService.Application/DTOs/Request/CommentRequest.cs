using System.ComponentModel.DataAnnotations;

namespace CommentService.Application.DTOs.Request
{
    public class CommentRequest
    {
        public Guid? Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string Content { get; set; }
    }
}