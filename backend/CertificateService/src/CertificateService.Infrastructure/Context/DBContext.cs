using CertificateService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace CertificateService.Infrastructure.Context
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
            
        }
        public DbSet<Certificate> Certificates { get; set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Certificate>().Property(c => c.Status).HasConversion<string>();
        }
    }
}