using CertificateService.Domain.Enums;
namespace CertificateService.Domain.Entities
{
    public class Certificate
    {
        public Guid Id { get; private set;}
        public string Title { get; private set;}
        public string Description { get; private set;}
        public string ImgUrl { get; private set; }
        public DateTime CreatedAt { get; private set;}
        public DateTime UpdateAt { get; private set; }
        public string Institution { get; private set;}
        public Status Status { get; private set;}
    }
}