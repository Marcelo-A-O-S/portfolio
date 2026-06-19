using MediaService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace MediaService.Infrastructure.Context
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }
        public DbSet<Media> Medias { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Media>().HasIndex(mf => mf.Path).IsUnique();
            modelBuilder.Entity<Media>().Property(mf => mf.Status).HasConversion<string>();
        }
    }
}