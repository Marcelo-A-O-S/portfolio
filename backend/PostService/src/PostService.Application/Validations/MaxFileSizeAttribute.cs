using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PostService.API.Validations
{
    public class MaxFileSizeAttribute(int maxBytes) : ValidationAttribute
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