using System.Text.Json.Serialization;
using AuthService.API.Extension;
using AuthService.API.Middleware;
using AuthService.Application.Extension;
using AuthService.Infrastructure.Extensions;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
   options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor; 
});
builder.Services.AddRateLimiteExtension();
builder.Services.AddSwaggerConfig();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCorsConfig();
builder.Services.AddInfrastructureExtension(builder.Configuration);
builder.Services.AddApplicationExtension();
var app = builder.Build();
// Configure the HTTP request pipeline.
app.Services.ApplyMigrations();
app.UseCors("AllowAll");
app.UseSwagger(options =>
{
    options.OpenApiVersion = OpenApiSpecVersion.OpenApi2_0;
});
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseForwardedHeaders();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();
app.Run("http://+:5001");

