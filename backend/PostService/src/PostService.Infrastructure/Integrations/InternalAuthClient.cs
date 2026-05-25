using System.Net.Http.Json;
using PostService.Application.Interfaces;
using PostService.Application.DTOs.Response;
using PostService.Application.Configurations;
using Microsoft.Extensions.Options;
namespace PostService.Infrastructure.Integrations
{
    public class InternalAuthClient : IInternalAuthClient
    {
        private readonly HttpClient http;
        private readonly ICacheService cacheServices;
        private readonly InternalClient internalOptions;
        public InternalAuthClient(
            HttpClient _http,
            ICacheService _cacheServices,
            IOptions<InternalClient> _internalOptions
        )
        {
            this.http = _http;
            this.cacheServices = _cacheServices;
            this.internalOptions = _internalOptions.Value;
        }
        public async Task<string> GetToken()
        {
            const string CACHE_KEY = "internal:postservice:token";
            string cached = await this.cacheServices.GetAsync(CACHE_KEY);
            if(!string.IsNullOrWhiteSpace(cached))
                return cached;
            if(string.IsNullOrEmpty(this.internalOptions.ClientId) || string.IsNullOrEmpty(this.internalOptions.ClientSecret))
                throw new Exception("Chaves de validação internas não configuradas");
            var response = await this.http.PostAsJsonAsync("/api/InternalAuth/internal/token",new
            {
                ClientId = this.internalOptions.ClientId,
                ClientSecret= this.internalOptions.ClientSecret
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