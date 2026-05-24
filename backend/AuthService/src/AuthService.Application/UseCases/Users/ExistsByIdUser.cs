using AuthService.Application.Interfaces;
using AuthService.Application.UseCases.Users.Interfaces;

namespace AuthService.Application.UseCases.Users
{
    public class ExistsByIdUser : IExistsByIdUser
    {
        private readonly IUserServices userServices;
        public ExistsByIdUser(
            IUserServices _userServices
        )
        {
            this.userServices = _userServices;
        }
        public async Task<bool> ExecuteAsync(Guid userId)
        {
            return await this.userServices.Exists(userId);
        }
    }
}