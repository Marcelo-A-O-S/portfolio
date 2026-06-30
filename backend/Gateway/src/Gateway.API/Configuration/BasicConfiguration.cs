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
                    RouteId = "categoryRoute",
                    ClusterId = "postCluster",
                    Match = new RouteMatch
                    {
                        Path = "/api/category/{**catch-all}"
                    }
                },
                new RouteConfig
                {
                    RouteId = "languageRoute",
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
                    ClusterId = "mediaCluster",
                    Match = new RouteMatch
                    {
                        Path = "/api/media/{**catch-all}"
                    }
                },
                new RouteConfig
                {
                    RouteId = "archivesRoute",
                    ClusterId = "mediaCluster",
                    Match = new RouteMatch
                    {
                        Path = "/archives/{**catch-all}"
                    }
                },
                new RouteConfig
                {
                    RouteId = "commentRoute",
                    ClusterId = "commentCluster",
                    Match = new RouteMatch
                    {
                        Path = "/api/comment/{**catch-all}"
                    }
                },
                new RouteConfig
                {
                    RouteId = "likeRoute",
                    ClusterId = "commentCluster",
                    Match = new RouteMatch
                    {
                        Path = "/api/like/{**catch-all}"
                    }
                },
                new RouteConfig
                {
                    RouteId = "certificateRoute",
                    ClusterId = "certificateCluster",
                    Match = new RouteMatch
                    {
                        Path = "/api/certificate/{**catch-all}"
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
            var commentAddress = configuration.GetSection("Destinations:CommentAddress").Value;
            var certificateAddress = configuration.GetSection("Destinations:CertificateAddress").Value;
            var mediaAddress = configuration.GetSection("Destinations:MediaAddress").Value;
            if (string.IsNullOrEmpty(authAddress) || string.IsNullOrEmpty(postAddress)
            || string.IsNullOrEmpty(commentAddress) || string.IsNullOrEmpty(certificateAddress)
            || string.IsNullOrEmpty(mediaAddress)
            )
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
                },
                new ClusterConfig
                {
                    ClusterId = "commentCluster",
                    Destinations = new Dictionary<string, DestinationConfig>
                    {
                        {
                            "commentservice", new DestinationConfig { Address = commentAddress}
                        }
                    }
                },
                new ClusterConfig
                {
                    ClusterId = "certificateCluster",
                    Destinations = new Dictionary<string, DestinationConfig>
                    {
                        {
                            "certificateservice", new DestinationConfig { Address = certificateAddress}
                        }
                    }
                },
                new ClusterConfig
                {
                    ClusterId = "mediaCluster",
                    Destinations = new Dictionary<string, DestinationConfig>
                    {
                        {
                            "mediaservice", new DestinationConfig { Address = mediaAddress}
                        }
                    }
                }
            };
        }
    }
}