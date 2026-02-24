using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Context
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
            
        }
        public DbSet<User> Users { get; set;}
        public DbSet<SocialAccount> SocialAccounts {get; set;}
        public DbSet<RefreshToken> RefreshTokens {get; set;}
    }
}