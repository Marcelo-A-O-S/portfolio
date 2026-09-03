namespace CertificateService.Domain.Entities
{
    public class CertificateContent
    {
        public Guid Id { get; private set; }
        public Guid CertificateId { get; private set; }
        public Certificate Certificate { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public Guid LanguageProjectionId { get; private set; }
        public LanguageProjection LanguageProjection { get; private set; }
        public CertificateContent(
            Guid languageProjectionId,
            string title,
            string description
        )
        {
            this.LanguageProjectionId = languageProjectionId;
            this.Title = title;
            this.Description = description;
        }
    }
}