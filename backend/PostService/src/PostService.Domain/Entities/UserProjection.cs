namespace PostService.Domain.Entities
{
    public class UserProjection
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string Username { get; private set; }
        public string ProfileUrl { get; private set; }
        public string ProviderId { get; private set; }
        public string Provider { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public UserProjection(Guid userId, string username, string profileUrl, string providerId, string provider)
        {
            this.UserId = userId;
            this.Username = username;
            this.ProfileUrl = profileUrl;
            this.ProviderId = providerId;
            this.Provider = provider;
            this.CreatedAt = DateTime.UtcNow;
        }
    }
}