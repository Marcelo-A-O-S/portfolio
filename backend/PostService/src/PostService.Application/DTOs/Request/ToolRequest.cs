using System.ComponentModel.DataAnnotations;
using PostService.Domain.Enums;
namespace PostService.Application.DTOs.Request
{
    public class ToolRequest
    {
        public Guid? Id { get; set; }
        [Required( ErrorMessage = "A imagem relacionada á ferramenta é obrigatória.")]
        public string ImgUrl { get; set; }
        [Required( ErrorMessage = "A lista de conteudo da ferramenta é obrigatória.")]
        public List<ToolContentRequest> ToolContents { get; set; }
        [Required( ErrorMessage = "A lista de categoria da ferramenta é obrigatória.")]
        public List<CategoryRequest> Categories { get; set; }
        [Required( ErrorMessage = "O Status de ferramenta é obrigatória." )]
        public Status Status { get; set; }
    }
}