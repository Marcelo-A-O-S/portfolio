using PostService.Domain.Enums;
namespace PostService.Domain.Queries
{
    public class PostBaseView
    {
        public Guid Id { get; set; }
        public MediaView Media { get; set; }
        public ICollection<CategoryView> Categories { get; set; }
        public int Likes { get; set; }
        public int Comments { get; set; }
        public bool Liked { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Status Status { get; set; }
    }
}