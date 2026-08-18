using PostService.Application.UseCases.Contributors.Interfaces;
using PostService.Application.Validators.Interfaces;
using PostService.Application.Interfaces;
using PostService.Application.Exceptions;
using PostService.Domain.Entities;

namespace PostService.Application.UseCases.Contributors
{
    public class DeleteContributor : IDeleteContributor
    {
        private readonly IContributorServices contributorServices;
        private readonly IUnitOfWork unitOfWork;
        public DeleteContributor(
            IContributorServices _contributorServices,
            IUnitOfWork _unitOfWork
        )
        {
            this.contributorServices = _contributorServices;
            this.unitOfWork = _unitOfWork;
        }
        public async Task ExecuteAsync(Guid Id)
        {
            var contributor = await this.GetContributorById(Id);
            await this.unitOfWork.BeginAsync();
            try
            {
                await this.contributorServices.Delete(contributor);
                await this.unitOfWork.CommitAsync();
            }
            catch
            {
                await this.unitOfWork.RollbackAsync();
                throw;
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