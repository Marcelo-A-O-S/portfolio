using AuthService.Domain.Enums;
using System.Text.Json.Serialization;

namespace AuthService.Application.DTOs.Request
{
    public class UpdateRoleRequest
    {
         [JsonConverter(typeof(JsonStringEnumConverter))]
        public Role Role { get; set; }
    }
}