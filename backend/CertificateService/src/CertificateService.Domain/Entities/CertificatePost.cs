namespace CertificateService.Domain.Entities
{
    public class CertificatePost
    {
        public Guid Id { get; private set; }
        public Guid CertificateId { get; private set;}
        public Certificate certificate { get; private set; }
        public Guid PostId { get; private set; }
    }
}