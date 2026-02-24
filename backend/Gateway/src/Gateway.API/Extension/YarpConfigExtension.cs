using Gateway.API.Configuration;

namespace Gateway.API.Extension
{
    public static class YarpConfigExtension
    {
        public static IServiceCollection AddYarpConfig(
            this IServiceCollection services
        ){
            services.AddReverseProxy()
            .LoadFromMemory(BasicConfiguration.GetRoutes(), BasicConfiguration.GetClusters());
            return services;
        }
    }
}