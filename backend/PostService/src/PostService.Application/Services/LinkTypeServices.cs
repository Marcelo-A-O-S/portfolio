using System.Linq.Expressions;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
namespace PostService.Application.Services
{
    public class LinkTypeServices : ILinkTypeServices
    {
        private readonly ILinkTypeRepository linkTypeRepository;
        public LinkTypeServices(
            ILinkTypeRepository _linkTypeRepository
        )
        {
            this.linkTypeRepository = _linkTypeRepository;
        }
        public async Task Delete(LinkType entity)
        {
            await this.linkTypeRepository.Delete(entity);
        }

        public async Task DeleteById(Guid Id)
        {
            await this.linkTypeRepository.DeleteById(Id);
        }

        public async Task<bool> Exists(Guid Id)
        {
            return await this.linkTypeRepository.Exists(Id);
        }

        public async Task<LinkType> FindBy(Expression<Func<LinkType, bool>> predicate)
        {
            return await this.linkTypeRepository.FindBy(predicate);
        }

        public async Task<LinkType> GetById(Guid Id)
        {
            return await this.linkTypeRepository.GetById(Id);
        }

        public async Task<List<LinkType>> List()
        {
            return await this.linkTypeRepository.List();
        }

        public async Task<List<LinkType>> List(int page)
        {
            return await this.linkTypeRepository.List(page);
        }

        public async Task Save(LinkType entity)
        {
            await this.linkTypeRepository.Save(entity);
        }
        public async Task Update(LinkType entity)
        {
            await this.linkTypeRepository.Update(entity);
        }
    }
}