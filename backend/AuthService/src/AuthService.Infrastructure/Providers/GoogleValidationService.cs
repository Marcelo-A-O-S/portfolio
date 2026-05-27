using AuthService.Application.Configurations;
using AuthService.Application.DTOs.Request;
using AuthService.Application.Interfaces;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;
namespace AuthService.Infrastructure.Providers
{
    public class GoogleValidationService : IProviderValidationService
    {
        public readonly ProviderOptions providerOptions;
        public GoogleValidationService(
            IOptions<ProviderOptions> options
        )
        {
            this.providerOptions = options.Value;
        }
        public async Task<ProviderUserData> Validate(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new InvalidOperationException("Token não informado");
            if (string.IsNullOrWhiteSpace(providerOptions.GoogleClientId))
                throw new InvalidOperationException("Google ClientId não configurado");
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[]
                {
                    this.providerOptions.GoogleClientId
                }
            };
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);
                if(string.IsNullOrEmpty(payload.Email))
                    throw new UnauthorizedAccessException("Google não retornou email");
                if (!payload.EmailVerified)
                    throw new UnauthorizedAccessException("Email Google não verificado");
                return new ProviderUserData
                {
                    ProviderId = payload.Subject,
                    Email = payload.Email,
                    Name = payload.Name,
                    Username = $"{payload.GivenName ?? ""} {payload.FamilyName ?? ""}".Trim(),
                    PictureUrl = payload.Picture,
                    VerifiedAccount = true
                };
            }
            catch (InvalidJwtException)
            {
                throw new UnauthorizedAccessException("Token Google inválido");
            }
        }
    }
}