using AuthService.Domain.Entities;
using AuthService.Domain.Queries;
namespace AuthService.Application.Interfaces
{
    public interface IUserServices: IServices<User>
    {
        Task<User> GetUserByEmail(string email);
        Task<User> GetFullById(Guid id);
        Task<PaginatedResult<User>> GetByPagination(int page, string? search, string? role, string? status);
        Task<List<UserView>> GetAll(string? search);
    }
}