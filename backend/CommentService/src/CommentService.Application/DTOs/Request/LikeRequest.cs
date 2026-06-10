using CommentService.Domain.Enums;
using System.ComponentModel.DataAnnotations;
namespace CommentService.Application.DTOs.Request
{
    public class LikeRequest
    {
        public Guid? Id { get; set; }
        [Required( ErrorMessage = "O identificador do objeto é obrigatório")]
        public Guid TargetId { get; set; }
        [Required( ErrorMessage = "O tipo de curtida é obrigatório")]
        public LikeType Type { get; set; }
    }
}