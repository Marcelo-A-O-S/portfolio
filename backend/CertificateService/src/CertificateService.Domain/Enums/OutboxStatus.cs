namespace CertificateService.Domain.Enums
{
    public enum OutboxStatus
    {
        Pending,
        Processing,
        Processed,
        Failed
    }
}