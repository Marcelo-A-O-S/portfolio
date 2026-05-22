using System.ComponentModel.DataAnnotations;

namespace PostService.Application.Validations
{
    public static class ValidationHelper
    {
        public static IReadOnlyCollection<ValidationResult> Validate(object model)
        {
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }
    }
}