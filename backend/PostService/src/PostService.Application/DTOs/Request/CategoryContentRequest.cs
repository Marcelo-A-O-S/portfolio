using System.ComponentModel.DataAnnotations;

namespace PostService.Application.DTOs.Request
{
    public class CategoryContentRequest
    {
        public Guid? Id { get; set; }
        [Required(ErrorMessage = "O identificador do idioma é obrigatório.")]
        public Guid LanguageId { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "O nome deve ter entre 5 a 25 caracteres")]
        public string Name { get; set; }
        [Required(ErrorMessage = "O slug de pesquisa é obrigatório.")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "O slug deve ter entre 5 a 30 caracteres")]
        public string Slug { get; set; }
    }
}