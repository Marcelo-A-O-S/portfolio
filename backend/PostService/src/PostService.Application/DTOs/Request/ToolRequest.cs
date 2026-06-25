using System.ComponentModel.DataAnnotations;
namespace PostService.Application.DTOs.Request
{
    public class ToolRequest : BaseRequest
    {
        [Required(ErrorMessage = "A lista de conteudos da ferramentas é obrigatório.")]
        public List<ToolContentRequest> ToolContents { get; set; }
    }
}