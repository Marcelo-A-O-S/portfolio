namespace PostService.Domain.Entities
{
    public class CertificateContentProjection
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid LanguageId { get; set; }
        public Language Language { get; set; }
        public CertificateContentProjection(
            string _title,
            Guid _languageId
        )
        {
            this.Title = _title;
            this.LanguageId = _languageId;
        }
    }
}