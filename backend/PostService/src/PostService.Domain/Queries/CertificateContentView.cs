namespace PostService.Domain.Queries
{
    public class CertificateContentView
    {
        public Guid? Id { get; set ;}
        public string Title { get; set; }
        public LanguageView Language { get; set; }
    }
}