using AuthService.Application.DTOs.Request;
using AuthService.Application.DTOs.Response;
using AuthService.Application.Exceptions;
using AuthService.Application.Interfaces;
using AuthService.Application.UseCases.Auth.Interfaces;
using AuthService.Application.Validations;
namespace AuthService.Application.UseCases.Auth
{
    public class RefreshToken : IRefreshToken
    {
        private readonly IJwtBearerServices jwtBearerServices;
        private readonly IUnitOfWork unitOfWork;
        public RefreshToken(
            IJwtBearerServices _jwtBearerServices,
            IUnitOfWork _unitOfWork
        )
        {
            this.jwtBearerServices = _jwtBearerServices;
            this.unitOfWork = _unitOfWork;
        }
        public async Task<AuthResponse> ExecuteAsync(RefreshRequest refreshRequest)
        {
            ValidateRequest(refreshRequest);
            await this.unitOfWork.BeginAsync();
            try
            {
                var authResponse = await this.jwtBearerServices.RefreshAsync(
                    refreshRequest.RefreshTokenId, 
                    refreshRequest.UserId, 
                    refreshRequest.ProviderId,
                    refreshRequest.RefreshToken, 
                    refreshRequest.DeviceId, 
                    refreshRequest.DeviceName
                );
                await this.unitOfWork.CommitAsync();
                return authResponse;
            }
            catch
            {
                await this.unitOfWork.RollbackAsync();
                throw;
            }
        }
        private static void ValidateRequest(RefreshRequest refreshRequest)
        {
            var validationError = ValidationHelper.Validate(refreshRequest);
            if (validationError.Count > 0)
            {
                var erros = string.Join(", ", validationError.Select(e => e.ErrorMessage));
                throw new ValidationException($"Erro ao validar dados: {erros}");
            }
        }
    }
}