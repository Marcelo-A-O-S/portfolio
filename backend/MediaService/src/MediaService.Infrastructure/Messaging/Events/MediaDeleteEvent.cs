namespace MediaService.Infrastructure.Messaging.Events
{
    public class MediaDeleteEvent
    {
        public Guid MediaFileId { get; set; }
        public string OwnerType { get; set; } 
    }
}