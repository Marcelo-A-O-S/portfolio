using System.Linq.Expressions;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;

namespace AuthService.Application.Services
{
    public class SocialAccountServices : ISocialAccountServices
    {
        private readonly ISocialAccountRepository socialAccountRepository;
        public SocialAccountServices(ISocialAccountRepository _socialAccountRepository)
        {
            this.socialAccountRepository = _socialAccountRepository;
        }
        public async Task Delete(SocialAccount entity)
        {
            await this.socialAccountRepository.Delete(entity);
        }

        public async Task<SocialAccount> FindBy(Expression<Func<SocialAccount, bool>> predicate)
        {
            return await this.socialAccountRepository.FindBy(predicate);
        }

        public async Task<SocialAccount> GetById(Guid Id)
        {
            return await this.socialAccountRepository.GetById(Id);
        }

        public async Task<SocialAccount> GetByProviderId(string providerId)
        {
            return await this.socialAccountRepository.GetByProviderId(providerId);
        }

        public async Task<List<SocialAccount>> List()
        {
            return await this.socialAccountRepository.List();
        }

        public async Task<List<SocialAccount>> List(int page)
        {
            return await this.socialAccountRepository.List(page);
        }

        public async Task Save(SocialAccount entity)
        {
            await this.socialAccountRepository.Save(entity);
        }

        public async Task Update(SocialAccount entity)
        {
            await this.socialAccountRepository.Update(entity);
        }
    }
}