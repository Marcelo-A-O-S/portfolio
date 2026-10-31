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
            Guid LanguageProjectionId,
            string title,
            string description
        )
        {
            this.PostProjectionId = postProjectionId;
            this.PostContentId = postContentId;
            this.LanguageProjectionId = LanguageProjectionId;
            this.Title = title;
            this.Description = description;
        }
        public void Update(string title, string description)
        {
            this.Title = title;
            this.Description = description;
        }
        
    }
}