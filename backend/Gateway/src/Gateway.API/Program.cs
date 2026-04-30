using Gateway.API.Extension;
using Gateway.API.Middleware;
using Microsoft.OpenApi;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddYarpConfig(builder.Configuration);
builder.Services.AddSwaggerConfig();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCorsConfig();
builder.Services.AddHttpClient();
var app = builder.Build();
app.UseCors("AllowAll");
app.UseSwagger(options =>
{
    options.OpenApiVersion = OpenApiSpecVersion.OpenApi2_0;
});
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Gateway API");
    options.SwaggerEndpoint("http://localhost:5001/swagger/v1/swagger.json","Auth API");
    options.SwaggerEndpoint("http://localhost:5002/swagger/v1/swagger.json","Post API");
});
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();
app.UseStaticFiles();
app.MapReverseProxy();
app.UseSimpleBlockBot();
app.Run("http://+:5000");