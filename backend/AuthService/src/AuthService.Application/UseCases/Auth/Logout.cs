using AuthService.Application.DTOs.Request;
using AuthService.Application.Exceptions;
using AuthService.Application.Interfaces;
using AuthService.Application.UseCases.Auth.Interfaces;
using AuthService.Application.Validations;
namespace AuthService.Application.UseCases.Auth
{
    public class Logout : ILogout
    {
        private readonly IRefreshTokenServices refreshTokenServices;
        private readonly IUnitOfWork unitOfWork;
        public Logout(
            IRefreshTokenServices _refreshTokenServices,
            IUnitOfWork _unitOfWork
        )
        {
            this.refreshTokenServices = _refreshTokenServices;
            this.unitOfWork = _unitOfWork;
        }
        public async Task ExecuteAsync(LogoutRequest logoutRequest)
        {
            ValidateRequest(logoutRequest);
            await this.unitOfWork.BeginAsync();
            try
            {
                var refreshToken = await this.refreshTokenServices.FindBy(rt => rt.DeviceId == logoutRequest.DeviceId && rt.UserId == logoutRequest.UserId);
                if (refreshToken != null)
                {
                    await refreshTokenServices.Delete(refreshToken);
                }
                await this.unitOfWork.CommitAsync();
            }
            catch
            {
                await this.unitOfWork.RollbackAsync();
                throw;
            }
        }
        private static void ValidateRequest(LogoutRequest logoutRequest)
        {
            var validationError = ValidationHelper.Validate(logoutRequest);
            if (validationError.Count > 0)
            {
                var erros = string.Join(", ", validationError.Select(e => e.ErrorMessage));
                throw new ValidationException($"Erro ao validar dados: {erros}");
            }
        }
    }
}