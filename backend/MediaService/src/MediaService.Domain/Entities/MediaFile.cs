using MediaService.Domain.Enums;
namespace MediaService.Domain.Entities
{
    public class MediaFile
    {
        public Guid Id { get; private set;}
        public Guid? OwnerId { get; private set; }
        public string OwnerType { get; private set; }
        public string Path { get; private set; }
        public string MimeType { get; private set; }
        public long Size {get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public Status Status { get; private set; }
        public MediaFile(
            Guid? ownerId,
            string ownerType,
            string path, 
            string mimeType, 
            long size)
        {
            this.OwnerId = ownerId;
            this.OwnerType = ownerType;
            this.Path = path;
            this.MimeType = mimeType;
            this.Size = size;
            this.CreatedAt = DateTime.UtcNow;
            this.Status = Status.Pending;
        }
        public void AssignOwner(Guid ownerId)
        {
            this.OwnerId = ownerId;
            this.UpdatedAt = DateTime.UtcNow;
        }
        public void Commit()
        {
            if (this.Status == Status.Committed)
                return;
            this.Status = Status.Committed;
            this.UpdatedAt = DateTime.UtcNow;
        }
        public void MarkAsFailed()
        {
            this.Status = Status.Failed;
            this.UpdatedAt = DateTime.UtcNow;
        }
        public void MarkAsUploaded()
        {
            this.Status = Status.Uploaded;
            this.UpdatedAt = DateTime.UtcNow;
        }
        public void UpdatePath(string path)
        {
            this.Path = path;
            this.UpdatedAt = DateTime.UtcNow;
        }
    }
}