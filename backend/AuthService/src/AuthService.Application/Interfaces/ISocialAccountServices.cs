using AuthService.Domain.Entities;

namespace AuthService.Application.Interfaces
{
    public interface ISocialAccountServices: IServices<SocialAccount>
    {
        Task<SocialAccount> GetByProviderId(string providerId);
    }
}