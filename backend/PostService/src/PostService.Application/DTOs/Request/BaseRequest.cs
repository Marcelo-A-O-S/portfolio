using System.ComponentModel.DataAnnotations;
using PostService.Domain.Enums;
namespace PostService.Application.DTOs.Request
{
    public abstract class BaseRequest
    {
        public Guid? Id { get; set; }
        [Required( ErrorMessage ="A imagem principal é obrigatória.")]
        public MediaRequest Media { get; set; }
        [Required( ErrorMessage ="A lista das categorias é obrigatória.")]
        public List<CategoryRequest> Categories { get; set; }
        [Required( ErrorMessage = "O Status do conteúdo é obrigatório.")]
        public Status Status { get; set; }
    }
}