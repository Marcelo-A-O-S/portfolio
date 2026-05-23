using AuthService.Application.Interfaces;
using AuthService.Application.Services;
using AuthService.Application.UseCases.Users;
using AuthService.Application.UseCases.Users.Interfaces;
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

            services.AddScoped<IDeleteUser, DeleteUser>();
            services.AddScoped<IBanUser, BanUser>();
            services.AddScoped<IModifyRoleUser, ModifyRoleUser>();
            return services;
        }
    }
}