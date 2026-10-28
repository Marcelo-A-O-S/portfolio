namespace PostService.Domain.Entities
{
    public class CertificateContentProjection
    {
        public Guid Id { get; private set; }
        public Guid CertificateContentId { get; private set; }
        public Guid CertificateProjectionId { get; private set; }
        public CertificateProjection CertificateProjection { get; private set; }
        public string Title { get; private set; }
        public Guid LanguageId { get; private set; }
        public Language Language { get; private set; }
        public CertificateContentProjection(
            Guid certificateProjectionId,
            Guid certificateContentId,
            string title,
            Guid languageId
        )
        {
            this.CertificateProjectionId = certificateProjectionId;
            this.CertificateContentId = certificateContentId;
            this.Title = title;
            this.LanguageId = languageId;
        }
        public void Update(string _title)
        {
            this.Title = _title;
        }
    }
}