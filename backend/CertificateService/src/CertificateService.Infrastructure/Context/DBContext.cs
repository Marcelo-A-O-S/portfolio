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
        public DbSet<CertificateContent> CertificateContents { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Certificate>()
                .HasMany(c => c.PostProjections)
                .WithMany(pc => pc.Certificates);
            modelBuilder.Entity<Certificate>()
                .Property(c => c.Status)
                .HasConversion<string>();
            modelBuilder.Entity<Certificate>().Property(c => c.CertificateType).HasConversion<string>();
            modelBuilder.Entity<MediaProjection>().HasIndex(mp => new { mp.Url, mp.MediaId }).IsUnique();
            modelBuilder.Entity<LanguageProjection>().HasIndex(lp => new { lp.Code }).IsUnique();
            modelBuilder.Entity<PostProjection>().HasIndex(pp => new { pp.PostId }).IsUnique();
            modelBuilder.Entity<PostContentProjection>().HasIndex(pp => new { pp.PostContentId }).IsUnique();
            modelBuilder.Entity<CertificateContent>().HasOne(cc => cc.LanguageProjection)
                .WithMany().HasForeignKey(cc => cc.LanguageProjectionId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PostContentProjection>().HasOne(cc => cc.LanguageProjection)
                .WithMany().HasForeignKey(pc => pc.LanguageProjectionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}