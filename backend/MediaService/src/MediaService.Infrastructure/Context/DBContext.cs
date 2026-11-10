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
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        public DbSet<MediaOwner> MediaOwners { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("media_service");
            modelBuilder.Entity<Media>().HasIndex(mf => mf.Path).IsUnique();
            modelBuilder.Entity<MediaOwner>()
                .HasIndex(mo => new
                {
                    mo.MediaId,
                    mo.OwnerId,
                    mo.OwnerType
                })
                .IsUnique();
            modelBuilder.Entity<Media>().Property(mf => mf.Status).HasConversion<string>();
        }
    }
}