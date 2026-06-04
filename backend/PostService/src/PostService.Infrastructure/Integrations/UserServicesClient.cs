using System.Net;
using System.Net.Http.Headers;
using PostService.Application.Exceptions;
using PostService.Application.Interfaces;
using System.Net.Http.Json;
namespace PostService.Infrastructure.Integrations
{
    public class UserServicesClient : IUserServicesClient
    {
        private readonly HttpClient http;
        private readonly IInternalAuthClient authClient;
        public UserServicesClient(
            HttpClient _http,
            IInternalAuthClient _authClient
        )
        {
            this.http = _http;
            this.authClient = _authClient;
        }
        public async Task<bool> UserExistsAsync(Guid userId)
        {
            try
            {
                var token = await authClient.GetToken();
                if (token == null)
                    throw new UnauthorizedException("Usuário não autorizado");
                var request = new HttpRequestMessage(
                    HttpMethod.Get,$"/api/InternalUser/{userId}/exists");
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
                throw new Exception("Serviço de usuários indisponivel");
            }
        }
    }
}