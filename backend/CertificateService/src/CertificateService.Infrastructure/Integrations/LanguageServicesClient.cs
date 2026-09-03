using CertificateService.Application.DTOs.Responses;
using CertificateService.Application.Exceptions;
using CertificateService.Application.Interfaces;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace CertificateService.Infrastructure.Integrations
{
    public class LanguageServicesClient : ILanguageServicesClient
    {
        private readonly HttpClient http;
        private readonly IInternalAuthClient authClient;
        public LanguageServicesClient(
            HttpClient _http,
            IInternalAuthClient _authClient
        )
        {
            this.authClient = _authClient;
            this.http = _http;
        }
        public async Task<LanguageResponse> GetLanguageAsync(Guid languageId)
        {
            try
            {
                var token = await this.authClient.GetToken();
                if (token == null)
                    throw new UnauthorizedException("Usuário não autorizado");
                var request = new HttpRequestMessage(
                    HttpMethod.Get, $"/api/InternalLanguage/{languageId}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await this.http.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedException("Token interno inválido");
                if (response.StatusCode == HttpStatusCode.NotFound)
                    throw new NotFoundException("Publicação não encontrada");
                if (response.StatusCode == HttpStatusCode.Forbidden)
                    throw new ForbiddenException("Permissões insuficientes");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<LanguageResponse>();
            }
            catch (HttpRequestException)
            {
                throw new Exception("Serviço de postagens indisponivel");
            }
        }
    }
}