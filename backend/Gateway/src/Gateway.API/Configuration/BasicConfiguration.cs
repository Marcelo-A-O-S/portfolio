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
                },
                new ClusterConfig
                {
                    ClusterId = "postCluster",
                    Destinations = new Dictionary<string, DestinationConfig>
                    {
                        {
                            "postservice", new DestinationConfig { Address = "http://postservice:5002/"}
                        }
                    }
                }
            };
        }
    }
}