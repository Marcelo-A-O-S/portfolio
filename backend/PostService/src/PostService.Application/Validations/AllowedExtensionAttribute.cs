using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PostService.Application.Validations
{
    public class AllowedExtensionAttribute(string[] extensions) : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is IFormFile file){
                var extension = Path.GetExtension(file.FileName).ToLower();
                if(!extensions.Contains(extension))
                    return new ValidationResult("Extensão inválida");
            }
            return ValidationResult.Success;
        }
    }
}