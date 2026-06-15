using Microsoft.EntityFrameworkCore;
using PostService.Domain.Entities;
namespace PostService.Infrastructure.Context
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }
        public DbSet<MediaProjection> MediaProjections { get; set; }
        public DbSet<LikeProjection> LikeProjections { get; set; }
        public DbSet<Tool> Tools { get; set; }
        public DbSet<ToolContent> ToolContents { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryContent> CategoriesContents { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostContent> PostContents { get; set; }
        public DbSet<Language> Languages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>().Property(p => p.Status).HasConversion<string>();
            modelBuilder.Entity<Tool>().Property(p => p.Status).HasConversion<string>();
            modelBuilder.Entity<PostContent>().HasIndex(pc => pc.Slug);
            modelBuilder.Entity<ToolContent>().HasIndex(tc => tc.Slug);
            modelBuilder.Entity<CategoryContent>().HasIndex(cc => cc.Slug);
            modelBuilder.Entity<Language>().HasIndex(lg => lg.Code);
            modelBuilder.Entity<LikeProjection>().HasIndex(lp => new { lp.TargetId, lp.UserId}).IsUnique();
            modelBuilder.Entity<MediaProjection>().HasIndex(mp => new { mp.OwnerId, mp.Url }).IsUnique();
        }
    }
}