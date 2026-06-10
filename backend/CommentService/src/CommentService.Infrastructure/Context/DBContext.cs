using CommentService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace CommentService.Infrastructure.Context
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>().HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies).HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Comment>().HasIndex(p => new { p.Type, p.TargetId }).IsUnique();
            modelBuilder.Entity<Comment>().Property(p => p.Type).HasConversion<string>();
            modelBuilder.Entity<Like>().HasIndex(p => new { p.UserId ,p.Type, p.TargetId }).IsUnique();
            modelBuilder.Entity<Like>().Property(p => p.Type).HasConversion<string>();
        }
    }
}