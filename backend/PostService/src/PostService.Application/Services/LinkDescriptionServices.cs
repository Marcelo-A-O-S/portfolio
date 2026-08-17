using System.Linq.Expressions;
using PostService.Application.Interfaces;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
namespace PostService.Application.Services
{
    public class LinkDescriptionServices : ILinkDescriptionServices
    {
        private readonly ILinkDescriptionRepository linkDescriptionRepository;
        public LinkDescriptionServices(
            ILinkDescriptionRepository _linkDescriptionRepository
        )
        {
            this.linkDescriptionRepository = _linkDescriptionRepository;
        }
        public async Task Delete(LinkDescription entity)
        {
            await this.linkDescriptionRepository.Delete(entity);
        }

        public async Task DeleteById(Guid Id)
        {
            await this.linkDescriptionRepository.DeleteById(Id);
        }

        public async Task<bool> Exists(Guid Id)
        {
            return await this.linkDescriptionRepository.Exists(Id);
        }

        public async Task<LinkDescription> FindBy(Expression<Func<LinkDescription, bool>> predicate)
        {
            return await this.linkDescriptionRepository.FindBy(predicate);
        }

        public async Task<LinkDescription> GetById(Guid Id)
        {
            return await this.linkDescriptionRepository.GetById(Id);
        }

        public async Task<List<LinkDescription>> List()
        {
            return await this.linkDescriptionRepository.List();
        }

        public async Task<List<LinkDescription>> List(int page)
        {
            return await this.linkDescriptionRepository.List(page);
        }

        public async Task Save(LinkDescription entity)
        {
            await this.linkDescriptionRepository.Save(entity);
        }

        public async Task Update(LinkDescription entity)
        {
            await this.linkDescriptionRepository.Update(entity);
        }
    }
}