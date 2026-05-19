using System.ComponentModel.DataAnnotations;
namespace PostService.Application.DTOs.Deserialize
{
    public class CategoryDeserialize
    {
         public Guid? Id { get; set; }
        [Required]
        public List<CategoryContentDeserialize> CategoryContents { get; set; }
    }
}