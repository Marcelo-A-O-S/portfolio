using System.ComponentModel.DataAnnotations;
namespace PostService.Application.DTOs.Request
{
    public class CategoryRequest
    {
        public Guid? Id { get; set; }
        [Required]
        public List<CategoryContentRequest> CategoryContents { get; set; }
    }
}