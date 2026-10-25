namespace PostService.Domain.Entities
{
    public class CertificateContentProjection
    {
        public Guid Id { get; set; }
        public Guid CertificateContentId { get; set; }
        public string Title { get; set; }
        public Guid LanguageId { get; set; }
        public Language Language { get; set; }
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