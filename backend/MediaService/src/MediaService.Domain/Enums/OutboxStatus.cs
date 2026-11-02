namespace MediaService.Domain.Enums
{
    public enum OutboxStatus
    {
        Pending,
        Processing,
        Processed,
        Failed
    }
}