using CertificateService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace CertificateService.Infrastructure.Context
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }
        public DbSet<PostProjection> PostProjections { get; set; }
        public DbSet<PostContentProjection> PostContentProjections { get; set; }
        public DbSet<LanguageProjection> LanguageProjections { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
         public DbSet<MediaProjection> MediaProjections { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Certificate>().Property(c => c.Status).HasConversion<string>();
            modelBuilder.Entity<Certificate>().Property(c => c.CertificateType).HasConversion<string>();
            modelBuilder.Entity<MediaProjection>().HasIndex(mp => new { mp.Url, mp.MediaId }).IsUnique();
            modelBuilder.Entity<LanguageProjection>().HasIndex(lp => new { lp.Code}).IsUnique();
        }
    }
}