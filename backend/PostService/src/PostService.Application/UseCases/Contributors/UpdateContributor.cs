using PostService.Application.DTOs.Request;
using PostService.Application.Interfaces;
using PostService.Application.UseCases.Contributors.Interfaces;
using PostService.Application.Validators.Interfaces;
using PostService.Application.Validations;
using PostService.Application.Exceptions;
using PostService.Domain.Entities;

namespace PostService.Application.UseCases.Contributors
{
    public class UpdateContributor : IUpdateContributor
    {
        private readonly IValidationServices validationServices;
        private readonly IContributorServices contributorServices;
        private readonly IUnitOfWork unitOfWork;
        public UpdateContributor(
            IValidationServices _validationServices,
            IContributorServices _contributorServices,
            IUnitOfWork _unitOfWork
        )
        {
            this.validationServices = _validationServices;
            this.contributorServices = _contributorServices;
            this.unitOfWork = _unitOfWork;
        }
        public async Task ExecuteAsync(Guid Id, ContributorRequest request)
        {
            ValidateRequest(request);
            await this.validationServices.ValidatePostExists(request.PostId);
            var contributor = await this.GetContributorById(Id);
            contributor.Update(request.PostId, request.Name, request.Description, request.ProfileUrl);
            if(request.UserId is Guid userId)
            {
                await this.validationServices.ValidateUserExists(userId);
                contributor.SetUserId(userId);
            }
            await this.unitOfWork.BeginAsync();
            try
            {
                await this.contributorServices.Update(contributor);
                await this.unitOfWork.CommitAsync();
            }
            catch
            {
                await this.unitOfWork.RollbackAsync();
                throw;
            }
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
        private async Task<Contributor> GetContributorById(Guid Id)
        {
            var contributor = await this.contributorServices.GetById(Id);
            if(contributor == null)
                throw new NotFoundException("Contribuidor não encontrado.");
            return contributor;
        }
    }
}