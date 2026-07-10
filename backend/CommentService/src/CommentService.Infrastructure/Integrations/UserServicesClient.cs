using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CommentService.Application.Exceptions;
using CommentService.Application.Interfaces;
using CommentService.Application.DTOs.Response;
using CommentService.Application.DTOs.Response;
using Microsoft.AspNetCore.Http.HttpResults;
namespace CommentService.Infrastructure.Integrations
{
    public class UserServicesClient : IUserServicesClient
    {
        private readonly HttpClient http;
        private readonly IAuthServicesClient authClient;
        public UserServicesClient(
            IAuthServicesClient _authClient,
            HttpClient _http
        )
        {
            this.authClient = _authClient;
            this.http = _http;
        }
        public async Task<UserResponse> GetUserAsync(Guid userId, string providerId)
        {
            var token = await authClient.GetToken();
            if (token == null)
                throw new UnauthorizedException("Usuário não autorizado");
            var request = new HttpRequestMessage(
                HttpMethod.Get, $"/api/InternalUser/{userId}/provider/{providerId}");
            var response = await this.http.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedException("Token interno inválido");
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new NotFoundException("Usuário não encontrado");
            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new ForbiddenException("Permissões insuficientes");
            return await response.Content.ReadFromJsonAsync<UserResponse>();
        }

        public async Task<bool> UserExistsAsync(Guid userId)
        {
            try
            {
                var token = await authClient.GetToken();
                if (token == null)
                    throw new UnauthorizedException("Usuário não autorizado");
                var request = new HttpRequestMessage(
                    HttpMethod.Get, $"/api/InternalUser/{userId}/exists");
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