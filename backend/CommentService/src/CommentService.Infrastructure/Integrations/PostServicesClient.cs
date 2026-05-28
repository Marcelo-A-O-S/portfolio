using CommentService.Application.Interfaces;
using CommentService.Application.Exceptions;
using System.Net.Http.Headers;
using System.Net;
using System.Net.Http.Json;

namespace CommentService.Infrastructure.Integrations
{
    public class PostServicesClient : IPostServicesClient
    {
        private readonly HttpClient http;
        private readonly IInternalAuthClient authClient;
        public PostServicesClient(
            HttpClient _http,
            IInternalAuthClient _authClient
        )
        {
            this.authClient = _authClient;
            this.http = _http;
        }
        public async Task<bool> PostExistsAsync(Guid postId)
        {
            try
            {
                var token = await authClient.GetToken();
                if (token == null)
                    throw new UnauthorizedException("Usuário não autorizado");
                var request = new HttpRequestMessage(
                    HttpMethod.Get,$"/api/InternalPost/internal/post/{postId}/exists");
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