namespace CertificateService.Domain.Entities
{
    public class LanguageProjection
    {
        public Guid Id { get; private set; }
        public string Code {get; private set;}
        public string Name { get; private set;}
    }
}