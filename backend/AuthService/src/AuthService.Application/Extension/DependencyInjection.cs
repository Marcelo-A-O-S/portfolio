using AuthService.Application.Interfaces;
using AuthService.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Application.Extension
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(
            this IServiceCollection services
        )
        {
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<ISocialAccountServices, SocialAccountServices>();
            services.AddScoped<IRefreshTokenServices,RefreshTokenServices>();
            services.AddScoped<IJwtBearerServices, JwtBearerServices>();
            return services;
        }
    }
}