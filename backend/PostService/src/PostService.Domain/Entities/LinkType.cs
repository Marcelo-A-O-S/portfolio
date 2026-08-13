namespace PostService.Domain.Entities
{
    public class LinkType
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string BackgroundColor { get; private set; }
        public string TextColor { get; private set; }
        public string Icon { get; private set; }
    }
}