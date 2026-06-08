using System.Text.Json.Serialization;
namespace AuthService.Application.DTOs.Response
{
    public class LinkedinResponse
    {
        [JsonPropertyName("sub")]
        public string Sub { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        public string Username { get; set; }
        [JsonPropertyName("picture")]
        public string Picture { get; set; }
    }
}