using AuthService.Application.Interfaces;
using AuthService.Application.UseCases.InternalUser.Interfaces;
namespace AuthService.Application.UseCases.InternalUser
{
    public class ExistsProviderId : IExistsProviderId
    {
        private readonly ISocialAccountServices socialAccountServices;
        public ExistsProviderId(
            ISocialAccountServices _socialAccountServices
        )
        {
            this.socialAccountServices = _socialAccountServices;
        }
        public async Task<bool> ExecuteAsync(Guid userId, string providerId)
        {
            return await this.socialAccountServices.VerifyProviderExists(userId, providerId);
        }
    }
}