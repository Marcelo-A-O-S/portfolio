using System.ComponentModel.DataAnnotations;
namespace PostService.Application.DTOs.Request
{
    public class ToolRequest
    {
        public Guid? Id { get; set; }
        [Required(ErrorMessage = "O idioma do conteúdo é obrigatório é obrigatório.")]
        public string Language { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "O nome deve ter entre 5 a 25 caracteres")]
        public string Name { get; set; }
        [Required(ErrorMessage = "A descrição é obrigatório.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "A descrição deve ter entre 5 a 100 caracteres")]
        public string Description { get; set; }
        [Required(ErrorMessage = "O conteúdo da ferramenta é obrigatório.")]
        public string Content { get; set; }
        [Required(ErrorMessage = "O slug de pesquisa é obrigatório.")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "O slug deve ter entre 5 a 30 caracteres")]
        public string Slug { get; set; }
    }
}