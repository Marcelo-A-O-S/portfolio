using AuthService.Domain.Enums;

namespace AuthService.Application.DTOs.Request
{
    public class UpdateRoleRequest
    {
        public Role Role { get; set; }
    }
}