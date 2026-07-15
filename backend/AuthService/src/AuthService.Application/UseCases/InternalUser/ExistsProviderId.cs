using AuthService.Application.Interfaces;
using AuthService.Application.UseCases.InternalUser.Interfaces;

namespace AuthService.Application.UseCases.InternalUser
{
    public class ExistsProviderId : IExistsProviderId
    {
        private readonly IUserServices userServices;
        private readonly ISocialAccountServices socialAccountServices;
        public ExistsProviderId(
            IUserServices _userServices,
            ISocialAccountServices _socialAccountServices
        )
        {
            this.userServices = _userServices;
            this.socialAccountServices = _socialAccountServices;
        }
        public Task<bool> ExecuteAsync(Guid userId, string providerId)
        {

            throw new NotImplementedException();
        }
    }
}