using MediaService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace MediaService.Infrastructure.Context
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }
        public DbSet<MediaFile> MediaFiles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MediaFile>().HasIndex(mf => mf.Path).IsUnique();
            modelBuilder.Entity<MediaFile>().Property(mf => mf.Status).HasConversion<string>();
        }
    }
}