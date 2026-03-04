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
        public DbSet<Category> Categories { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<PostCategory> PostCategories { get; set; } 
        public DbSet<PostTool> PostTools { get; set; }
    }
}