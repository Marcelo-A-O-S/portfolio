using System.ComponentModel.DataAnnotations;
using PostService.Application.DTOs.Request;
using PostService.Application.UseCases.Contributors.Interfaces;
using PostService.Application.Validations;
using PostService.Application.Validators.Interfaces;

namespace PostService.Application.UseCases.Contributors
{
    public class AddContributor : IAddContributor
    {
        private readonly IValidationServices validationServices;
        public AddContributor(
            IValidationServices _validationServices
        )
        {
            this.validationServices = _validationServices;
        }
        public Task ExecuteAsync(ContributorRequest request)
        {
            ValidateRequest(request);
            if(request.UserId is Guid userId)
            {
                this.validationServices.ValidateUserExists(userId);
            }
            
            throw new NotImplementedException();
        }
        private static void ValidateRequest(ContributorRequest request)
        {
            var validationError = ValidationHelper.Validate(request);
            if (validationError.Count > 0)
            {
                var errors = string.Join(", ", validationError.Select(e => e.ErrorMessage));
                throw new ValidationException($"Erro ao validar dados: {errors}");
            }
        }
        
    }
}