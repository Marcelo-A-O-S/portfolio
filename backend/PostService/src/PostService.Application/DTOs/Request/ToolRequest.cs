using System.ComponentModel.DataAnnotations;
namespace PostService.Application.DTOs.Request
{
    public class ToolRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "O nome deve ter entre 5 a 25 caracteres")]
        public string Name { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "A descrição deve ter entre 5 a 100 caracteres")]
        public string Description { get; set; }
        public string Content { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "O slug deve ter entre 5 a 30 caracteres")]
        public string Slug { get; set; }
    }
}