using Yarp.ReverseProxy.Configuration;
namespace Gateway.API.Configuration
{
    public static class BasicConfiguration
    {
        public static IReadOnlyList<RouteConfig> GetRoutes()
        {
            return new[]
            {
                new RouteConfig
                {
                    RouteId = "authRoute",
                    ClusterId = "authCluster",
                    Match = new RouteMatch
                    {
                        Path = "/api/auth/{**catch-all}"
                    }
                }
            };
        }
        public static IReadOnlyList<ClusterConfig> GetClusters()
        {
            return new[]
            {
                new ClusterConfig
                {
                    ClusterId = "authCluster",
                    Destinations = new Dictionary<string, DestinationConfig>
                    {
                        {
                            "authservice", new DestinationConfig { Address = "http://authservice:5001/" }
                        }
                    }
                }
            };
        }
    }
}