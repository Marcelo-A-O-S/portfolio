using System.Net;
using System.Net.Http.Headers;
using PostService.Application.Exceptions;
using PostService.Application.Interfaces;
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
                http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await this.http.GetAsync($"/api/InternalUser/internal/users/{userId}/exists");
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedException("Token interno inválido");
                if (response.StatusCode == HttpStatusCode.NotFound)
                    return false;
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (HttpRequestException)
            {
                throw new Exception("Serviço de usuários indisponivel");
            }
        }
    }
}