using Microsoft.OpenApi.Models;

namespace PostService.API.Extensions
{
    public static class SwaggerConfigExtension
    {
        public static IServiceCollection AddSwaggerConfig(
            this IServiceCollection services
        )
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Post API",
                    Version = "v1",
                    Description = "Api de gerenciamento de publicações de postagens envolvendo os projetos.",
                    Contact = new OpenApiContact
                    {
                        Name = "Marcelo Augusto de Oliveira Soares",
                        Email = "marceloaugustooliveirasoares@gmail.com",
                        Url = new Uri("https://github.com/Marcelo-A-O-S")
                    },
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Insira o token JWT aqui:"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string [] {}
                    }   
                });
            });
            return services;
        }
    }
}