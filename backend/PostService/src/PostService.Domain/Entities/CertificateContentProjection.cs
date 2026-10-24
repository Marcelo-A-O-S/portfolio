namespace PostService.Domain.Entities
{
    public class CertificateContentProjection
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid LanguageId { get; set; }
        public Language Language { get; set; }
    }
}