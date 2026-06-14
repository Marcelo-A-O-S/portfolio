namespace MediaService.Infrastructure.Messaging.Events
{
    public class MediaCommitEvent
    {
        public Guid MediaFileId { get; set; }
        public Guid OwnerId { get; set; }
        public string OwnerType { get; set; } 
    }
}