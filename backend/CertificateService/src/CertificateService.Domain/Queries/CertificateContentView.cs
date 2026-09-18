namespace CertificateService.Domain.Queries
{
    public class CertificateContentView
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public LanguageProjectionView LanguageProjection { get; set; }
    }
}