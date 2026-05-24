using AuthService.Application.DTOs.Request;
using AuthService.Application.DTOs.Response;
using AuthService.Application.Exceptions;
using AuthService.Application.Interfaces;
using AuthService.Application.UseCases.InternalUser.Interfaces;
using AuthService.Application.Validations;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
namespace AuthService.Application.UseCases.InternalUser
{
    public class CreateToken : ICreateToken
    {
        private readonly IConfiguration configuration;
        private readonly IJwtBearerServices jwtBearerServices;
        public CreateToken(
            IConfiguration _configuration,
            IJwtBearerServices _jwtBearerServices
        )
        {
            this.configuration = _configuration;
            this.jwtBearerServices = _jwtBearerServices;
        }
        public async Task<TokenResponse> ExecuteAsync(ServiceAuthRequest request)
        {
            ValidateRequest(request);
            var token =  await GenerateToken(request);
            return new TokenResponse
            {
                AccessToken = token,
                ExpireIn = 60
            };
        }
        private void ValidateRequest(ServiceAuthRequest request)
        {
            var clientId = this.configuration.GetValue<string>("InternalClients:PostService:ClientId");
            var clientSecret = this.configuration.GetValue<string>("InternalClients:PostService:ClientSecret");
            if(string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
                throw new ValidationException("Chaves de validação internas não configuradas");
            var validationError = ValidationHelper.Validate(request);
            if(validationError.Count > 0)
            {
                var errors = string.Join(", ", validationError.Select(e => e.ErrorMessage));
                throw new ValidationException($"Erro ao validar dados: {errors}");
            }
            if(request.ClientId != clientId || request.ClientSecret != clientSecret)
                throw new UnauthorizedException("Usuário não autorizado.");
        }
        private async Task<string> GenerateToken(ServiceAuthRequest request)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim("scope","users.read"));
            claims.Add(new Claim(JwtRegisteredClaimNames.Aud, request.ClientId));
            return await this.jwtBearerServices.GenerateInternalToken(claims);
        }
    }
}