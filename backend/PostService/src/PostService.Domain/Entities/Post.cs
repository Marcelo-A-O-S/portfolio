using PostService.Domain.Enums;

namespace PostService.Domain.Entities
{
    public class Post
    {
        public Guid Id { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Like> Likes {get; set;}
        public ICollection<PostContent> PostContents { get; set;}
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set;}
        public Status Status { get; set;}

    }
}