using Microsoft.EntityFrameworkCore;
using PostService.Domain.Entities;
namespace PostService.Infrastructure.Context
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }
        public DbSet<MediaFile> MediaFiles { get; set; }
        public DbSet<Tool> Tools { get; set; }
        public DbSet<ToolContent> ToolContents { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryContent> CategoriesContents { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostContent> PostContents { get; set;}
        public DbSet<Language> Languages {get; set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>().Property(p=> p.Status).HasConversion<string>();
            modelBuilder.Entity<Tool>().Property(p=> p.Status).HasConversion<string>();
            modelBuilder.Entity<PostContent>().HasIndex(pc => pc.Slug);
            modelBuilder.Entity<ToolContent>().HasIndex(tc => tc.Slug);
            modelBuilder.Entity<CategoryContent>().HasIndex(cc => cc.Slug);
            modelBuilder.Entity<Language>().HasIndex(lg => lg.Code);
            modelBuilder.Entity<MediaFile>().HasIndex(mf => mf.Path).IsUnique();
        }
    }
}