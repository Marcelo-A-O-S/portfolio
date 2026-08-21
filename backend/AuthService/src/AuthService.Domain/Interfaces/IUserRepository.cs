using AuthService.Domain.Entities;
using AuthService.Domain.Queries;
namespace AuthService.Domain.Interfaces
{
    public interface IUserRepository : IGenerics<User>
    {
        Task<User> GetUserByEmail(string email);
        Task<User> GetFullById(Guid id);
        Task<PaginatedResult<User>> GetByPagination(int page, string? search, string? role, string? status, int itemsPage = 10);
        Task<List<UserView>> GetAll(string? search);
    }
}