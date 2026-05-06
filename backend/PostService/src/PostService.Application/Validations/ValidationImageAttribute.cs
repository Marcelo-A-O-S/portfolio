using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace PostService.Application.Validations
{
    public class ValidationImageAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                if (!file.ContentType.StartsWith("image/"))
                    return new ValidationResult("Arquivo deve ser uma imagem.");
            }
            return ValidationResult.Success;
        }
    }
}