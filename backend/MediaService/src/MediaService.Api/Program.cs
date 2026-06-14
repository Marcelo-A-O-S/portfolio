using System.Text.Json.Serialization;
using System.Text.Json;
using MediaService.Api.Extensions;
using MediaService.Infrastructure.Extensions;
using MediaService.Application.Extensions;
using MediaService.Api.Middleware;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
   options.ForwardedHeaders = ForwardedHeaders.XForwardedFor; 
});
builder.Services.AddRateLimiteExtension();
builder.Services.AddSwaggerConfig();
builder.Services.AddPolicyAuthentications(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCorsConfig();
builder.Services.AddInfrastructureExtension(builder.Configuration);
builder.Services.AddApplicationExtensions(builder.Configuration);
var app = builder.Build();
// Configure the HTTP request pipeline.
app.Services.ApplyMigrations();
app.UseCors("AllowAll");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options=>{
        options.OpenApiVersion = OpenApiSpecVersion.OpenApi2_0;
        });
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseForwardedHeaders();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();
app.UseStaticFiles();
app.Run("http://+:5005");
