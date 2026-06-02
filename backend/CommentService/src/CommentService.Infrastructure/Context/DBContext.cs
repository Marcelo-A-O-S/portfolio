using CommentService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace CommentService.Infrastructure.Context
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>().HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies).HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Like>().HasIndex(p => new { p.UserId, p.CommentId }).IsUnique();
        }
    }
}