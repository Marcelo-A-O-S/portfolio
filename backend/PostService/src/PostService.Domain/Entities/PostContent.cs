namespace PostService.Domain.Entities
{
    public class PostContent
    {
        public Guid Id { get; private set; }
        public Guid PostId { get; private set; }
        public Post Post { get; private set; }
        public string Language { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Content { get; private set; }
        public DateTime UpdatedAt { get; private set; }
    }
}