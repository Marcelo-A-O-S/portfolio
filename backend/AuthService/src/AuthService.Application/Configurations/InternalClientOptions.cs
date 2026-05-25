namespace AuthService.Application.Configurations
{
    public class InternalClientOptions
    {
        public Dictionary<string, InternalClient> InternalClients { get; set; } = [];
        public class InternalClient
        {
            public string ClientId { get; set; }
            public string ClientSecret { get; set; }
            public List<string> Scopes { get; set; } = [];
        }
    }
}