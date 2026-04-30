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
                },
                new RouteConfig
                {
                    RouteId = "userRoute",
                    ClusterId = "authCluster",
                    Match = new RouteMatch
                    {
                        Path = "/api/user/{**catch-all}"
                    }
                },
                new RouteConfig
                {
                    RouteId = "postRoute",
                    ClusterId = "postCluster",
                    Match = new RouteMatch
                    {
                        Path = "/api/post/{**catch-all}"
                    }
                },
                new RouteConfig
                {
                    RouteId = "toolRoute",
                    ClusterId = "postCluster",
                    Match = new RouteMatch
                    {
                        Path = "/api/tool/{**catch-all}"
                    }
                },
                new RouteConfig
                {
                    RouteId = "Category",
                    ClusterId = "postCluster",
                    Match = new RouteMatch
                    {
                        Path = "/api/category/{**catch-all}"
                    }
                },
                new RouteConfig
                {
                    RouteId = "Language",
                    ClusterId = "postCluster",
                    Match = new RouteMatch
                    {
                        Path = "/api/language/{**catch-all}"
                    }
                },
                new RouteConfig
                {
                    RouteId = "fileRoute",
                    ClusterId = "postCluster",
                    Match = new RouteMatch
                    {
                        Path = "/api/file/{**catch-all}"
                    }
                },
                new RouteConfig
                {
                    RouteId = "mediaRoute",
                    ClusterId = "postCluster",
                    Match = new RouteMatch
                    {
                        Path = "/media/{**catch-all}"
                    }
                }
            };
        }
        public static IReadOnlyList<ClusterConfig> GetClusters(
            IConfiguration configuration
        )
        {
            var authAddress = configuration.GetSection("Destinations:AuthAddress").Value;
            var postAddress = configuration.GetSection("Destinations:PostAddress").Value;
            if (string.IsNullOrEmpty(authAddress) || string.IsNullOrEmpty(postAddress))
            {
                throw new InvalidOperationException("Chaves de destino não configuradas corretamente.");
            }
            return new[]
            {
                new ClusterConfig
                {
                    ClusterId = "authCluster",
                    Destinations = new Dictionary<string, DestinationConfig>
                    {
                        {
                            "authservice", new DestinationConfig { Address = authAddress }
                        }
                    }
                },
                new ClusterConfig
                {
                    ClusterId = "postCluster",
                    Destinations = new Dictionary<string, DestinationConfig>
                    {
                        {
                            "postservice", new DestinationConfig { Address = postAddress}
                        }
                    }
                }
            };
        }
    }
}