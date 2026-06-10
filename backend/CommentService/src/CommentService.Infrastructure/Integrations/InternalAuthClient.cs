using System.Net.Http.Json;
using CommentService.Application.Configurations;
using CommentService.Application.Interfaces;
using Microsoft.Extensions.Options;
using CommentService.Application.DTOs.Response;
namespace CommentService.Infrastructure.Integrations
{
    public class InternalAuthClient : IInternalAuthClient
    {
        private readonly HttpClient http;
        private readonly ICacheServices cacheServices;
        private readonly InternalClientOptions internalOptions;
        public InternalAuthClient(
            HttpClient _http,
            ICacheServices _cacheServices,
            IOptions<InternalClientOptions> _internalOptions
        )
        {
            this.http = _http;
            this.cacheServices = _cacheServices;
            this.internalOptions = _internalOptions.Value;
        }
        public async Task<string> GetToken()
        {
            var client = this.internalOptions.InternalClients["AuthService"];
            const string CACHE_KEY = "internal:commentservice:token";
            string cached = await this.cacheServices.GetAsync(CACHE_KEY);
            if(!string.IsNullOrWhiteSpace(cached))
                return cached;
            if(string.IsNullOrEmpty(client.ClientId) || string.IsNullOrEmpty(client.ClientSecret))
                throw new Exception("Chaves de validação internas não configuradas");
            var response = await this.http.PostAsJsonAsync("/api/InternalAuth/internal/token",new
            {
                ClientId = client.ClientId,
                ClientSecret= client.ClientSecret
            });
            response.EnsureSuccessStatusCode();
            var tokenResponse =  await response.Content.ReadFromJsonAsync<TokenResponse>();
            if(tokenResponse == null || string.IsNullOrWhiteSpace(tokenResponse.AccessToken))
                throw new Exception("Falha ao obter token interno");
            await this.cacheServices.SetAsync(CACHE_KEY, tokenResponse.AccessToken, TimeSpan.FromSeconds(tokenResponse.ExpireIn - 10));
            return tokenResponse.AccessToken;
        }
    }
}