namespace PostService.Domain.Entities
{
    public class Link
    {
        public Guid Id { get; private set; }
        public string Url { get; private set; }
        public ICollection<LinkDescription> Descriptions { get; private set; }
        public Guid LinkTypeId { get; private set; }
        public LinkType LinkType { get; private set; }
        public Guid? PostId { get; private set; }
        public Post? Post { get; private set; }
        public Guid? ToolId { get; private set; }
        public Tool? Tool { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public Link(string url, Guid linkTypeId)
        {
            this.Url = url;
            this.Descriptions = new List<LinkDescription>();
            this.LinkTypeId = linkTypeId;
            this.CreatedAt = DateTime.UtcNow;
            this.UpdatedAt = DateTime.UtcNow;
        }
        public void Update(string url, Guid linkTypeId)
        {
            this.Url = url;
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
        public void GeneratedId()
        {
            this.Id = Guid.NewGuid();
        }
        public void AddLinkDescription(LinkDescription linkDescription)
        {
            this.Descriptions.Add(linkDescription);
        }
        public void ValidateDescriptions(IEnumerable<Guid> descriptionIds)
        {
            if(this.Descriptions == null)
                throw new Exception("Lista de descrição não inicializada.");
            var ids = descriptionIds.ToHashSet();
            var toRemove = this.Descriptions
                .Where(tc => !ids.Contains(tc.Id))
                .ToList();
            foreach(var description in toRemove)
                this.Descriptions.Remove(description);
        }
    }
}