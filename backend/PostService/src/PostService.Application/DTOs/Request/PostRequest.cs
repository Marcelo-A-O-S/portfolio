using System.ComponentModel.DataAnnotations;
namespace PostService.Application.DTOs.Request
{
    public class PostRequest : BaseRequest
    {
        [Required(ErrorMessage = "A lista de conteudos das postagens é obrigatória.")]
        public List<PostContentRequest> PostContents { get; set; }
        [Required(ErrorMessage = "A lista de ferramentas é obrigatória.")]
        public List<ToolRequest> Tools { get; set; }
    }
}