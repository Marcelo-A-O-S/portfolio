using AuthService.Domain.Entities;
using AuthService.Domain.Enums;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repositories
{
    public class UserRepository : Generics<User>, IUserRepository
    {
        private readonly DBContext context;
        public UserRepository(DBContext _context) : base(_context)
        {
            this.context = _context;
        }

        public async Task<PaginatedResult<User>> GetByPagination(int page, string? search, string? role, string? status, int itemsPage = 10)
        {
            var query =  this.context.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u =>
                    EF.Functions.Like(u.Name, $"%{search}%") ||
                    EF.Functions.Like(u.Email, $"%{search}%")
                );
            }
            if (!string.IsNullOrWhiteSpace(role) &&
                Enum.TryParse<Role>(role, true, out var roleEnum))
            {
                query = query.Where(u =>
                    u.Role == roleEnum  
                );
            }
            if (!string.IsNullOrWhiteSpace(status) &&
                Enum.TryParse<UserStatus>(status, true, out var statusEnum))
            {
                query = query.Where(u =>
                    u.Status == statusEnum  
                );
            }
            var totalItems = await query.CountAsync();
            var items = await query.Skip((page - 1) * itemsPage)
            .Take(itemsPage)
            .Include(u => u.SocialAccounts)
            .ToListAsync();
            return new PaginatedResult<User>{
                Items = items,
                TotalItems = totalItems,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)itemsPage)
            };
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await this.context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
        }
    }
}