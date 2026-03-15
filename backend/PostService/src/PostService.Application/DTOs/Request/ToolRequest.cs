using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
namespace PostService.Application.DTOs.Request
{
    public class ToolRequest
    {
        public Guid? Id { get; set; }
        [NotNull]
        [Required( ErrorMessage = "A lista de conteudo da ferramenta é obrigatória")]
        public List<ToolContentRequest> toolContents { get; set; }
        [NotNull]
        [Required( ErrorMessage = "A lista de categoria da ferramenta é obrigatória")]
        public List<CategoryRequest> Categories { get; set; }
    }
}