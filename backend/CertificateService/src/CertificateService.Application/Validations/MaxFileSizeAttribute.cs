using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace CertificateService.Application.Validations
{
    public class MaxFileSizeAttribute(int maxBytes) :ValidationAttribute 
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is IFormFile file && file.Length > maxBytes)
            {
                return new ValidationResult("Arquivo muito grande");
            }
            return ValidationResult.Success;
        }
    }
}