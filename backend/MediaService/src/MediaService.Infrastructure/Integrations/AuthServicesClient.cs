using MediaService.Application.Interfaces;
using MediaService.Application.Configurations;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using MediaService.Application.DTOs.Responses;

namespace MediaService.Infrastructure.Integrations
{
    public class AuthServicesClient : IAuthServicesClient
    {
        private readonly HttpClient http;
        private readonly ICacheServices cacheServices;
        private readonly InternalClientOptions internalOptions;
        public AuthServicesClient(
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
            const string CACHE_KEY = "internal:mediaservice:token";
            string cached = await this.cacheServices.GetAsync(CACHE_KEY);
            if(!string.IsNullOrWhiteSpace(cached))
                return cached;
            if(string.IsNullOrEmpty(client.ClientId) || string.IsNullOrEmpty(client.ClientSecret))
                throw new Exception("Chaves de validação internas não configuradas");
            var response = await this.http.PostAsJsonAsync("/api/InternalAuth/token",new
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