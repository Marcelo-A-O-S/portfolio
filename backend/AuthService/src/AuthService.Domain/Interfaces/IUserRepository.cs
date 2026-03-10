using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces
{
    public interface IUserRepository : IGenerics<User>
    {
        Task<User> GetUserByEmail(string email);
        Task<PaginatedResult<User>> GetByPagination(int page, string? search, string? role, string? status, int itemsPage = 10);
    }
}