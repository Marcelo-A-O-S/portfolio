namespace Gateway.API.Extension
{
    public static class CorsConfigExtension
    {
        public static IServiceCollection AddCorsConfig(
            this IServiceCollection services
        )
        {
            services.AddCors(options =>
            {
               options.AddPolicy("AllowAll", policy =>
               {
                   policy.WithOrigins(
                    "http://localhost:3000"
                   ).AllowAnyHeader().AllowAnyMethod();
               }); 
            });
            return services;
        }
    }
}