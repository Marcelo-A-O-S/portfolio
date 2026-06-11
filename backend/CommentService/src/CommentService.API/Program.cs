using System.Text.Json;
using System.Text.Json.Serialization;
using CommentService.API.Extensions;
using CommentService.API.Middleware;
using CommentService.Application.Extensions;
using CommentService.Infrastructure.Extensions;
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
   options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor; 
});
builder.Services.AddRateLimiteExtension();
builder.Services.AddSwaggerConfig();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCorsConfig();
builder.Services.AddInfrastructureExtensions(builder.Configuration);
builder.Services.AddApplicationExtensions(builder.Configuration);
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
app.Run("http://+:5003");
