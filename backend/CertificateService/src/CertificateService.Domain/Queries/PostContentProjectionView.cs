namespace CertificateService.Domain.Queries
{
    public class PostContentProjectionView
    {
        public Guid? Id { get; set; }
        public Guid PostContentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public LanguageProjectionView Language { get; set; }
        public LanguageProjectionView LanguageProjection { get; set; }
        
    }
}