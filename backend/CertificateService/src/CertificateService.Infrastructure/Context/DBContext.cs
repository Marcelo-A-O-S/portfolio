using CertificateService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace CertificateService.Infrastructure.Context
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }
       
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<MediaFile> MediaFiles { get; set; }
        public DbSet<CertificatePost> CertificatePosts { get; set; }
         public DbSet<MediaProjection> MediaProjections { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Certificate>().Property(c => c.Status).HasConversion<string>();
            modelBuilder.Entity<Certificate>().Property(c => c.CertificateType).HasConversion<string>();
            modelBuilder.Entity<MediaProjection>().HasIndex(mp => new { mp.Url, mp.MediaId }).IsUnique();
        }
    }
}