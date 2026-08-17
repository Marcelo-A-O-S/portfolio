using System.Linq.Expressions;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Domain.Queries;
namespace PostService.Application.Services
{
    public class LinkServices : ILinkServices
    {
        private readonly ILinkRepository linkRepository;
        public LinkServices(
            ILinkRepository _linkRepository
        )
        {
            this.linkRepository = _linkRepository;
        }
        public async Task Delete(Link entity)
        {
            await this.linkRepository.Delete(entity);
        }

        public async Task DeleteById(Guid Id)
        {
            await this.linkRepository.DeleteById(Id);
        }

        public async Task<bool> Exists(Guid Id)
        {
            return await this.linkRepository.Exists(Id);
        }

        public async Task<Link> FindBy(Expression<Func<Link, bool>> predicate)
        {
            return await this.linkRepository.FindBy(predicate);
        }

        public async Task<Link> GetById(Guid Id)
        {
            return await this.linkRepository.GetById(Id);
        }

        public async Task<PaginatedResult<LinkView>> GetByPagination(int page, Guid? toolId, Guid? postId, string? search, int itemsPage = 10)
        {
            return await this.linkRepository.GetByPagination(page, toolId, postId, search);
        }

        public async Task<Link> GetFullDataById(Guid Id)
        {
            return await this.linkRepository.GetFullDataById(Id);
        }

        public async Task<List<Link>> List()
        {
            return await this.linkRepository.List();
        }

        public async Task<List<Link>> List(int page)
        {
            return await this.linkRepository.List(page);
        }

        public async Task Save(Link entity)
        {
            await this.linkRepository.Save(entity);
        }

        public async Task Update(Link entity)
        {
            await this.linkRepository.Update(entity);
        }
    }
}