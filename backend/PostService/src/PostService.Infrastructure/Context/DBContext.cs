using Microsoft.EntityFrameworkCore;
using PostService.Domain.Entities;
namespace PostService.Infrastructure.Context
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }
        public DbSet<Tool> Tools { get; set; }
        public DbSet<ToolContent> ToolContents { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryContent> CategoriesContents { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostContent> PostContents { get; set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>().Property(p=> p.Status).HasConversion<string>();
        }
    }
}