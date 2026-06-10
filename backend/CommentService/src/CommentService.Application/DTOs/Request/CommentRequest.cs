using System.ComponentModel.DataAnnotations;
using CommentService.Domain.Enums;

namespace CommentService.Application.DTOs.Request
{
    public class CommentRequest
    {
        public Guid? Id { get; set; }
        [Required( ErrorMessage = "O identificador do objeto é obrigatório")]
        public Guid TargetId { get; set; }
        [Required( ErrorMessage = "O conteúdo do comentário é obrigatório")]
        public string Content { get; set; }
        [Required( ErrorMessage = "O tipo de comentário é obrigatório")]
        public CommentType Type { get; set; }
    }
}