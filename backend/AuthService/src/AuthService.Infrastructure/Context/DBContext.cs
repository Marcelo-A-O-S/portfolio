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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(p=> p.Role).HasConversion<string>();
            modelBuilder.Entity<User>().Property(p=> p.Status).HasConversion<string>();
        }
    }
}