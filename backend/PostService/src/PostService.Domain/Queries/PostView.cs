using PostService.Domain.Enums;
namespace PostService.Domain.Queries
{
    public class PostView
    {
        public Guid Id { get; set; }
        public string ImgUrl { get; set; }
        public ICollection<CategoryView> Categories { get; set; }
        public int Likes { get; set; }
        public bool Liked { get; set; } = false;
        public ICollection<ToolView> Tools { get; set; }
        public ICollection<PostContentView> PostContents { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Status Status { get; set; }
    }
}