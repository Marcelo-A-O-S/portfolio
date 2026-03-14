using PostService.API.Extensions;
using PostService.Application.Extensions;
using PostService.Infrastructure.Extensions;
using Microsoft.OpenApi;
using PostService.API.Middleware;
using System.Text.Json.Serialization;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
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
builder.Services.AddApplicationExtensions();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.Services.ApplyMigrations();
app.UseCors("AllowAll");
app.UseSwagger(options=>{
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
app.Run("http://+:5002");


