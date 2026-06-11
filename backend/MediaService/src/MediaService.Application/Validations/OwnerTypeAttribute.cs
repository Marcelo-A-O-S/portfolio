using System.ComponentModel.DataAnnotations;
using MediaService.Domain.Entities;
namespace MediaService.Application.Validations
{
    public class OwnerTypeAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value,ValidationContext validationContext)
        {
            if (value is not string ownerType)
                return new ValidationResult("OwnerType inválido.");
            var validTypes = new[]
            {
                OwnerTypes.Post,
                OwnerTypes.Tool,
                OwnerTypes.Certificate
            };
            return validTypes.Contains(ownerType)
                ? ValidationResult.Success
                : new ValidationResult("OwnerType inválido.");
        }
    }
}