using AuthService.Domain.Entities;

namespace AuthService.Application.Interfaces
{
    public interface IUserServices: IServices<User>
    {
        Task<User> GetUserByEmail(string email);
    }
}