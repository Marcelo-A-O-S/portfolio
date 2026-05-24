using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using PostService.Application.Interfaces;
using PostService.Application.DTOs.Response;
namespace PostService.Infrastructure.Integrations
{
    public class InternalAuthClient : IInternalAuthClient
    {
        private readonly HttpClient http;
        private readonly IConfiguration configuration;
        private readonly ICacheService cacheServices;
        public InternalAuthClient(
            HttpClient _http,
            IConfiguration _configuration,
            ICacheService _cacheServices
        )
        {
            this.http = _http;
            this.configuration = _configuration;
            this.cacheServices = _cacheServices;
        }
        public async Task<string> GetToken()
        {
            const string CACHE_KEY = "internal:postservice:token";
            string cached = await this.cacheServices.GetAsync(CACHE_KEY);
            if(cached != null)
                return cached;
            var clientId = this.configuration.GetValue<string>("InternalClients:PostService:ClientId");
            var clientSecret = this.configuration.GetValue<string>("InternalClients:PostService:ClientSecret");
            if(string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
                throw new Exception("Chaves de validação internas não configuradas");
            var response = await this.http.PostAsJsonAsync("/api/InternalAuth/internal/token",new
            {
                ClientId= clientId,
                ClientSecret= clientSecret
            });
            response.EnsureSuccessStatusCode();
            var tokenResponse =  await response.Content.ReadFromJsonAsync<TokenResponse>();
            await this.cacheServices.SetAsync(CACHE_KEY, tokenResponse.AccessToken, TimeSpan.FromSeconds(tokenResponse.ExpireIn - 10));
            return tokenResponse.AccessToken;    
        }
    }
}