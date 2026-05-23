using AuthService.Application.DTOs.Request;

namespace AuthService.Application.UseCases.Users.Interfaces
{
    public interface IModifyRoleUser
    {
        Task ExecuteAsync(Guid Id, UpdateRoleRequest updateRoleRequest);
    }
}