namespace MediaService.Domain.Entities
{
    public class MediaOwner
    {
        public Guid Id { get; private set; }
        public Guid MediaId { get; private set; }
        public Media Media { get; private set; }
        public Guid OwnerId { get; private set; }
        public string OwnerType { get; private set; }
    }
}