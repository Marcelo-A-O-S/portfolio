namespace MediaService.Infrastructure.Messaging.Events
{
    public class MediaDeleteEvent
    {
        public Guid MediaId { get; set; }
        public string OwnerType { get; set; } 
    }
}