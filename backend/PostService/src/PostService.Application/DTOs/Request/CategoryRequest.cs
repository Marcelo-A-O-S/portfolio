using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PostService.Application.DTOs.Request
{
    public class CategoryRequest
    {
        public Guid? Id { get; private set; }
        public string Language { get; private set; }
        [Required]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "O nome deve ter entre 5 a 25 caracteres")]
        public string Name { get; private set; }
        public string Slug { get; private set;}
    }
}