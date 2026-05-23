namespace PostService.Domain.Entities
{
    public class UserCache
    {
        public Guid UserId { get; set; }
        public UserCache(Guid userId)
        {
            this.UserId = userId;
        }
    }
}