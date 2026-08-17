namespace PostService.Domain.Entities
{
    public class LinkDescription
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public Guid LinkId { get; private set; }
        public Link Link { get; private set; }
        public Guid LanguageId { get; private set; }
        public Language Language { get; private set; }
        public LinkDescription(
            Guid linkId,
            Guid languageId,
            string title
        )
        {
            LinkId = linkId;
            LanguageId = languageId;
            Title = title;
        }
        public void Update(string title)
        {
            Title = title;
        }
    }
}