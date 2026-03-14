using System.ComponentModel.DataAnnotations;

namespace PostService.Application.DTOs.Request
{
    public class CategoryContentRequest
    {
        public Guid? Id { get; set; }
        public string Language { get; set; }
        [Required]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "O nome deve ter entre 5 a 25 caracteres")]
        public string Name { get; set; }
        public string Slug { get; set; }
    }
}