using System.Text.Json;
using System.Text.Json.Serialization;
using CertificateService.API.Extensions;
using CertificateService.Application.Extensions;
using CertificateService.Infrastructure.Extensions;
using Microsoft.OpenApi;
using CertificateService.API.Middleware;
using Microsoft.AspNetCore.HttpOverrides;
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
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCorsConfig();
builder.Services.AddInfrastructureExtensions(builder.Configuration);
builder.Services.AddApplicationExtensions();
var app = builder.Build();
// Configure the HTTP request pipeline.
app.Services.ApplyMigrations();
app.UseCors("AllowAll");
app.UseSwagger(options=>{
    options.OpenApiVersion = OpenApiSpecVersion.OpenApi2_0;
});
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseForwardedHeaders();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();
app.Run("http://+:5004");

