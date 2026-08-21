using AuthService.Domain.Entities;
using AuthService.Domain.Enums;
using AuthService.Domain.Interfaces;
using AuthService.Domain.Queries;
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
            var query =  this.context.Users
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u =>
                    EF.Functions.Like(u.Name, $"%{search}%") ||
                    EF.Functions.Like(u.Email, $"%{search}%") ||
                    EF.Functions.Like(u.Description, $"%{search}%")
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
            var items = await query
                .OrderByDescending(u => u.CreatedAt)
                .Include(u => u.SocialAccounts)
                .Skip((page - 1) * itemsPage)
                .Take(itemsPage)
                .ToListAsync();
            return new PaginatedResult<User>{
                Items = items,
                TotalItems = totalItems,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalItems / (double)itemsPage)
            };
        }
        public async Task<List<UserView>> GetAll(string? search)
        {
            var query = this.context.Users
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u =>
                    EF.Functions.Like(u.Name, $"%{search}%") ||
                    EF.Functions.Like(u.Email, $"%{search}%") ||
                    EF.Functions.Like(u.Description, $"%{search}%")
                );
            }
            var items = await query
                .OrderByDescending(u => u.CreatedAt)
                .Select(u => new UserView
                {
                    Id = u.Id,
                    ProfileUrl = u.ProfileUrl,
                    Name = u.Name,
                    Description = u.Description
                }).ToListAsync();
            return items;
        }

        public async Task<User> GetFullById(Guid id)
        {
            var query =  this.context.Users
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable();
            var item = await query
                .Where(u => u.Id == id)
                .Include(u => u.SocialAccounts)
                .FirstOrDefaultAsync();
            return item;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await this.context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
        }
    }
}