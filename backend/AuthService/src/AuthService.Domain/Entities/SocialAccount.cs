namespace AuthService.Domain.Entities
{
    public class SocialAccount
    {
        public Guid Id { get; set;}
        public Guid UserId { get; set;}
        public string Username { get; set;}
        public string ProfileUrl { get; set;}
        public string ProviderId {get;set;}
        public string Provider {get;set; }
        public SocialAccount(Guid userId, string username, string profileUrl, string providerId, string provider)
        {
            this.Id = Guid.NewGuid();
            this.UserId = userId;
            this.Username = username;
            this.ProfileUrl = profileUrl;
            this.ProviderId = providerId;
            this.Provider = provider;
            
        }
    }
}