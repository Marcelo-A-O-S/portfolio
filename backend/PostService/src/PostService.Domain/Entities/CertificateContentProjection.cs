namespace PostService.Domain.Entities
{
    public class CertificateContentProjection
    {
        public Guid Id { get; private set; }
        public Guid CertificateContentId { get; private set; }
        public string Title { get; private set; }
        public Guid LanguageId { get; private set; }
        public Language Language { get; private set; }
        public CertificateContentProjection(
            Guid _certificateContentId,
            string _title,
            Guid _languageId
        )
        {
            this.CertificateContentId = _certificateContentId;
            this.Title = _title;
            this.LanguageId = _languageId;
        }
        public void Update(string _title)
        {
            this.Title = _title;
        }
    }
}