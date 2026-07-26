using AuthService.Application.Interfaces;
using AuthService.Application.UseCases.InternalUser.Interfaces;
using AuthService.Application.DTOs.Response;
using AuthService.Application.Exceptions;
namespace AuthService.Application.UseCases.InternalUser
{
    public class GetByIdUser : IGetByIdUser
    {
        private readonly IUserServices userServices;
        public GetByIdUser(
            IUserServices _userServices
        )
        {
            this.userServices = _userServices;
        }
        public async Task<UserResponse> ExecuteAsync(Guid userId, string providerId)
        {
            var user = await this.userServices.GetFullById(userId);
            if(user == null)
                throw new NotFoundException("Usuário não encontrado");
            var account = user.SocialAccounts.Where(sc => sc.ProviderId == providerId).FirstOrDefault();
            if(account == null)
                throw new ValidationException("Erro ao validar Conta");
            return new UserResponse
            {
                Name = user.Name,
                ProviderId = account.ProviderId,
                ProfileUrl = account.ProfileUrl,
                Provider = account.Provider
            };
        }
    }
}