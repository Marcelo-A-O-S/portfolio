using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace MediaService.Application.Validations
{
    public class AllowedExtensionAttribute(string[] extensions) : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is IFormFile file){
                var extension = Path.GetExtension(file.FileName).ToLower();
                if(!extensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                    return new ValidationResult("Extensão inválida");
            }
            return ValidationResult.Success;
        }
    }
}