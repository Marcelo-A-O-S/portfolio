using System.ComponentModel.DataAnnotations;
namespace PostService.Application.DTOs.Request
{
    public class ToolContentRequest : BaseContentRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 2 a 25 caracteres")]
        public string Name { get; set; }
    }
}