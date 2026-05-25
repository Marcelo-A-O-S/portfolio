using AuthService.Application.Configurations;
using AuthService.Application.DTOs.Request;
using AuthService.Application.DTOs.Response;
using AuthService.Application.Exceptions;
using AuthService.Application.Interfaces;
using AuthService.Application.UseCases.InternalUser.Interfaces;
using AuthService.Application.Validations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
namespace AuthService.Application.UseCases.InternalUser
{
    public class CreateToken : ICreateToken
    {
        private readonly IJwtBearerServices jwtBearerServices;
        private readonly InternalClientOptions options;
        private readonly InternalJWTOptions jWTInternalOptions;
        public CreateToken(
            IJwtBearerServices _jwtBearerServices,
            IOptions<InternalClientOptions> _options,
            IOptions<InternalJWTOptions> _jWTInternalOptions
        )
        {
            this.jwtBearerServices = _jwtBearerServices;
            this.options = _options.Value;
            this.jWTInternalOptions = _jWTInternalOptions.Value;
        }
        public async Task<TokenResponse> ExecuteAsync(ServiceAuthRequest request)
        {
            ValidateRequest(request);
            var token =  await GenerateToken(request);
            return new TokenResponse
            {
                AccessToken = token,
                ExpireIn = (int)TimeSpan.FromMinutes(this.jWTInternalOptions.InternalExpirationMinutes).TotalSeconds
            };
        }
        private void ValidateRequest(ServiceAuthRequest request)
        {
            var validationError = ValidationHelper.Validate(request);
            if(validationError.Count > 0)
            {
                var errors = string.Join(", ", validationError.Select(e => e.ErrorMessage));
                throw new ValidationException($"Erro ao validar dados: {errors}");
            }
            var client = this.options.InternalClients.Values
                .FirstOrDefault(c =>c.ClientId == request.ClientId && c.ClientSecret == request.ClientSecret);
            if(client == null)
                throw new UnauthorizedException("Cliente interno inválido.");
        }
        private async Task<string> GenerateToken(ServiceAuthRequest request)
        {
            var client = options.InternalClients.Values
                .First(c => c.ClientId == request.ClientId && c.ClientSecret == request.ClientSecret);
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, client.ClientId),
                new("client_type","internal")
            };
            foreach(var scope in client.Scopes)
            {
                claims.Add(new Claim("scope", scope));
            }
            return await this.jwtBearerServices.GenerateInternalToken(claims);
        }
    }
}