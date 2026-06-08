using System.Net.Http.Headers;
using System.Text.Json;
using AuthService.Application.DTOs.Request;
using AuthService.Application.DTOs.Response;
using AuthService.Application.Interfaces;
namespace AuthService.Infrastructure.Providers
{
    public class LinkedinValidationService : IProviderValidationService
    {
        private readonly HttpClient client;
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
        public LinkedinValidationService(
            HttpClient _client
        )
        {
            this.client = _client;
        }
        public async Task<ProviderUserData> Validate(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new InvalidOperationException("Token não informado");
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.linkedin.com/v2/userinfo");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await this.client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                throw new UnauthorizedAccessException("Token do Linkedin inválido");
            var content = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<LinkedinResponse>(content, JsonOptions);
            if(user == null)
                throw new UnauthorizedAccessException("Resposta inválida do Linkedin");
            if(string.IsNullOrWhiteSpace(user.Email))
                throw new UnauthorizedAccessException("Linkedin não retornou email");
            return new ProviderUserData
            {
                ProviderId = user.Sub,
                Email = user.Email,
                Name = user.Name,
                Username = user.Name,
                PictureUrl = user.Picture,
                VerifiedAccount = true
            };
        }
    }
}