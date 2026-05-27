using AuthService.Application.DTOs.Request;
using AuthService.Application.DTOs.Response;
using AuthService.Application.Exceptions;
using AuthService.Application.Interfaces;
using AuthService.Application.UseCases.Auth.Interfaces;
using AuthService.Application.Validations;
using AuthService.Domain.Entities;
namespace AuthService.Application.UseCases.Auth
{
    public class Login : ILogin
    {
        private readonly IProviderFactory providerFactory;
        private readonly IUserServices userServices;
        private readonly ISocialAccountServices socialAccountServices;
        private readonly IJwtBearerServices jwtBearerServices;
        public Login(
            IUserServices _userServices,
            ISocialAccountServices _socialAccountServices,
            IProviderFactory _providerFactory,
            IJwtBearerServices _jwtBearerServices
        )
        {
            this.userServices = _userServices;
            this.socialAccountServices = _socialAccountServices;
            this.providerFactory = _providerFactory;
            this.jwtBearerServices = _jwtBearerServices;
        }
        public async Task<AuthResponse> ExecuteASync(LoginRequest loginRequest)
        {
            ValidateRequest(loginRequest);
            var provider = providerFactory.Get(loginRequest.Provider);
            var providerData = await provider.Validate(loginRequest.ProviderToken);
            var user = await GetOrCreateUser(providerData);
            await SaveUser(user);
            var socialAccount = await GetOrCreateSocial(user, loginRequest, providerData);
            await SaveSocial(socialAccount);
            var accessData = await this.jwtBearerServices.GenerateAccessToken(user);
            var data = await this.jwtBearerServices.GenerateRefreshToken(user.Id, loginRequest.DeviceId, loginRequest.DeviceName);
            return new AuthResponse
            {
                UserId = user.Id,
                AccessToken = accessData.token,
                RefreshToken = data.plainToken,
                ExpireIn = accessData.expireIn,
                Role = user.Role.ToString(),
                RefreshTokenId = data.entity.Id
            };
        }
        private static void ValidateRequest(LoginRequest loginRequest)
        {
            var validationError = ValidationHelper.Validate(loginRequest);
            if (validationError.Count > 0)
            {
                var erros = string.Join(", ", validationError.Select(e => e.ErrorMessage));
                throw new ValidationException($"Erro ao validar dados: {erros}");
            }
        }
        private async Task<User> GetOrCreateUser(ProviderUserData providerUserData)
        {
            var user = await this.userServices.FindBy(x => x.Email == providerUserData.Email);
            if (user == null)
            {
                user = new User(providerUserData.Email, providerUserData.Name);
            }
            return user;
        }
        private async Task SaveUser(User user)
        {
            if (user.Id == Guid.Empty)
                await userServices.Save(user);
        }
        private async Task<SocialAccount> GetOrCreateSocial(User user, LoginRequest loginRequest, ProviderUserData providerUserData)
        {
            var socialAccount = await this.socialAccountServices.GetByProviderId(providerUserData.ProviderId);
            if (socialAccount == null)
            {
                socialAccount = new SocialAccount(
                    user.Id, providerUserData.Username,
                    providerUserData.PictureUrl,
                    providerUserData.ProviderId,
                    loginRequest.Provider,
                    providerUserData.VerifiedAccount
                    );
            }
            return socialAccount;
        }
        private async Task SaveSocial( SocialAccount socialAccount)
        {
            if (socialAccount.Id == Guid.Empty)
                await socialAccountServices.Save(socialAccount);
        }
    }
}