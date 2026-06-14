namespace PostService.Domain.Entities
{
    public class LikeProjection
    {
        public Guid Id { get; set; }
        public Guid TargetId { get; set; }
        public Guid UserId { get; set; }
        public LikeProjection(Guid targetId, Guid userId)
        {
            this.TargetId = targetId;
            this.UserId = userId;
        }
    }
}