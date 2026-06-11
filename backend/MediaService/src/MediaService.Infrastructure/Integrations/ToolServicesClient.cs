using MediaService.Application.Interfaces;
using MediaService.Application.Exceptions;
using System.Net.Http.Headers;
using System.Net;
using System.Net.Http.Json;

namespace MediaService.Infrastructure.Integrations
{
    public class ToolServicesClient : IToolServicesClient
    {
        private readonly HttpClient http;
        private readonly IAuthServicesClient authClient;
        public ToolServicesClient(
            HttpClient _http,
            IAuthServicesClient _authClient
        )
        {
            this.authClient = _authClient;
            this.http = _http;
        }
        public async Task<bool> ToolExistsAsync(Guid toolId)
        {
            try
            {
                var token = await authClient.GetToken();
                if (token == null)
                    throw new UnauthorizedException("Usuário não autorizado");
                var request = new HttpRequestMessage(
                    HttpMethod.Get,$"/api/InternalTool/{toolId}/exists");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await this.http.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedException("Token interno inválido");
                if (response.StatusCode == HttpStatusCode.NotFound)
                    return false;
                if (response.StatusCode == HttpStatusCode.Forbidden)
                    throw new ForbiddenException("Permissões insuficientes");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<bool>();
            }
            catch (HttpRequestException)
            {
                throw new Exception("Serviço de ferramentas indisponivel");
            }
        }
    }
}