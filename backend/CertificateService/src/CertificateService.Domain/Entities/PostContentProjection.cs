namespace CertificateService.Domain.Entities
{
    public class PostContentProjection
    {
        public Guid Id { get; private set; }
        public Guid PostContentId { get; private set; }
        public Guid PostProjectionId { get; private set; }
        public PostProjection PostProjection { get; private set; }
        public Guid LanguageProjectionId { get; private set; }
        public LanguageProjection LanguageProjection { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public PostContentProjection(
            Guid postProjectionId,
            Guid postContentId,
            Guid languageProjectionId,
            string title,
            string description
        )
        {
            this.PostProjectionId = postProjectionId;
            this.PostContentId = postContentId;
            this.LanguageProjectionId = languageProjectionId;
            this.Title = title;
            this.Description = description;
        }
        public void Update(Guid languageProjectionId,string title, string description)
        {
            this.LanguageProjectionId = languageProjectionId;
            this.Title = title;
            this.Description = description;
        }
        
    }
}