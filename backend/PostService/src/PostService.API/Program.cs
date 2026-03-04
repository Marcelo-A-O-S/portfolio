using PostService.API.Extensions;
using PostService.Application.Extensions;
using PostService.Infrastructure.Extensions;
using Microsoft.OpenApi;
using PostService.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
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
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();
app.Run("http://+:5002");


