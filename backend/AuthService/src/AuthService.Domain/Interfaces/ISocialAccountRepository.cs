using AuthService.Domain.Entities;
namespace AuthService.Domain.Interfaces
{
    public interface ISocialAccountRepository : IGenerics<SocialAccount>
    {
        Task<SocialAccount> GetByProviderId(string providerId);
        Task<bool> VerifyProviderExists(Guid userId, string providerId);
    }
}