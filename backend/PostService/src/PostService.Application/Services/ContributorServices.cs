using System.Linq.Expressions;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Domain.Queries;
namespace PostService.Application.Services
{
    public class ContributorServices : IContributorServices
    {
        private readonly IContributorRepository contributorRepository;
        public ContributorServices(
            IContributorRepository _contributorRepository
        )
        {
            this.contributorRepository = _contributorRepository;
        }
        public async Task Delete(Contributor entity)
        {
            await this.contributorRepository.Delete(entity);
        }

        public async Task DeleteById(Guid Id)
        {
            await this.contributorRepository.DeleteById(Id);
        }

        public async Task<bool> Exists(Guid Id)
        {
            return await this.contributorRepository.Exists(Id);
        }

        public async Task<Contributor> FindBy(Expression<Func<Contributor, bool>> predicate)
        {
            return await this.contributorRepository.FindBy(predicate);
        }

        public async Task<Contributor> GetById(Guid Id)
        {
            return await this.contributorRepository.GetById(Id);
        }

        public async Task<PaginatedResult<ContributorView>> GetByPagination(int page, Guid postId, string? search, int itemsPage = 10)
        {
            return await this.contributorRepository.GetByPagination(page, postId, search, itemsPage);
        }

        public async Task<List<Contributor>> List()
        {
            return await this.contributorRepository.List();
        }

        public async Task<List<Contributor>> List(int page)
        {
            return await this.contributorRepository.List(page);
        }

        public async Task Save(Contributor entity)
        {
            await this.contributorRepository.Save(entity);
        }

        public async Task Update(Contributor entity)
        {
            await this.contributorRepository.Update(entity);
        }
    }
}