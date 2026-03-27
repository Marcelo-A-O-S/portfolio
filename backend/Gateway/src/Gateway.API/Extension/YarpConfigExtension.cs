using Gateway.API.Configuration;

namespace Gateway.API.Extension
{
    public static class YarpConfigExtension
    {
        public static IServiceCollection AddYarpConfig(
            this IServiceCollection services, IConfiguration configuration
        ){
            services.AddReverseProxy()
            .LoadFromMemory(BasicConfiguration.GetRoutes(), BasicConfiguration.GetClusters(configuration));
            return services;
        }
    }
}