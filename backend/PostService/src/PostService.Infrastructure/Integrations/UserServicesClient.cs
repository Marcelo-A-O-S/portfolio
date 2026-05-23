using System.Net;
using PostService.Application.Interfaces;
using static System.Net.WebRequestMethods;

namespace PostService.Infrastructure.Integrations
{
    public class UserServicesClient : IUserServicesClient
    {
        private readonly HttpClient http;
        public UserServicesClient(
            HttpClient _http
        )
        {
            this.http = _http;
        }
        public async Task<bool> UserExistsAsync(Guid userId)
        {
            try
            {
                var response = await this.http.GetAsync($"/api/User/{userId}");
                if(response.StatusCode == HttpStatusCode.NotFound)
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