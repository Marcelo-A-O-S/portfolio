using PostService.Domain.Enums;

namespace PostService.Domain.Entities
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<PostCategory> PostCategories { get; set; }
        public ICollection<Like> Likes {get; set;}
        public ICollection<Section> Sections { get; set;}
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set;}
        public Status Status { get; set;}

    }
}