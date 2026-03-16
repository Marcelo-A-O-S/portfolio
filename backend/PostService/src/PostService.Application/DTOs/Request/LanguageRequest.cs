using System.ComponentModel.DataAnnotations;

namespace PostService.Application.DTOs.Request
{
    public class LanguageRequest
    {
        public Guid? Id { get; set; }
        [Required(ErrorMessage = "O código é obrigatório.")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "O nome deve ter entre 5 a 25 caracteres")]
        public string Code { get; set; }
        [Required(ErrorMessage = "O nome da linguagem é obrigatório.")]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "O nome deve ter entre 5 a 25 caracteres")]
        public string Name { get; set; }
    }
}