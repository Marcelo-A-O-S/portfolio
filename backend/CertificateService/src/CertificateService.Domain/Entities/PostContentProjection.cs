namespace CertificateService.Domain.Entities
{
    public class PostContentProjection
    {
        public Guid Id { get; private set; }
        public Guid PostContentId { get; private set; }
        public Guid LanguageProjectionId { get; private set; }
        public LanguageProjection LanguageProjection { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public PostContentProjection(
            Guid postContentId,
            Guid languageId,
            string title,
            string description
        )
        {
            this.PostContentId = postContentId;
            this.LanguageProjectionId = languageId;
            this.Title = title;
            this.Description = description;
        }
        
    }
}