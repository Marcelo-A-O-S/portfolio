using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CertificateService.Application.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CertificateService.Application.Extensions
{
    public static class ConfigurationExtension
    {
        public static IServiceCollection AddInternalConfigurations(
            this IServiceCollection services, IConfiguration configuration
        )
        {
            services.Configure<InternalClientOptions>(configuration.GetSection("InternalClientOptions"));
            services.Configure<RedisOptions>(configuration.GetSection("Redis"));
            return services;
        }
    }
}