namespace PostService.Domain.Entities
{
    public class Link
    {
        public Guid Id { get; private set; }
        public string Url { get; private set; }
        public string Title { get; private set; }
        public Guid LinkTypeId { get; private set; }
        public LinkType LinkType { get; private set; }
        public Guid? PostId { get; private set; }
        public Post? Post { get; private set; }
        public Guid? ToolId { get; private set; }
        public Tool? Tool { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public Link(string url, string title, Guid linkTypeId)
        {
            this.Url = url;
            this.Title = title;
            this.LinkTypeId = linkTypeId;
            this.CreatedAt = DateTime.UtcNow;
            this.UpdatedAt = DateTime.UtcNow;
        }
        public void Update(string url, string title, Guid linkTypeId)
        {
            this.Url = url;
            this.Title = title;
            this.LinkTypeId = linkTypeId;
            this.UpdatedAt = DateTime.UtcNow;
        }
        public void SetPostId(Guid postId)
        {
            this.PostId = postId;
        }
        public void SetToolId(Guid toolId)
        {
            this.ToolId = toolId;
        }
    }
}