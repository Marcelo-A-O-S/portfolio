namespace PostService.Domain.Entities
{
    public class Link
    {
        public Guid Id { get; private set; }
        public string Url { get; private set; }
        public string Title { get; private set; }
        public Guid LinkTypeId { get; private set; }
        public LinkType LinkType { get; private set; }
    }
}