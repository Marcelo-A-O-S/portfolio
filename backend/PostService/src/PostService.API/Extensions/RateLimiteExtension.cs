using System.Threading.RateLimiting;
namespace PostService.API.Extensions
{
    public static class RateLimiteExtension
    {
        private static string GetPartitionKey(HttpContext context)
        {
            var userId =
                context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ??
                context.User?.Identity?.Name ??
                context.Connection.RemoteIpAddress?.MapToIPv4().ToString() ??
                "unknown";

            var endpoint = context.Request.Path;

            return $"{userId}--{endpoint}";
        }
        public static IServiceCollection AddRateLimiteExtension(
            this IServiceCollection services
        )
        {
            services.AddRateLimiter(options =>
            {
                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = 429;

                    await context.HttpContext.Response.WriteAsync(
                        "Too many requests. Please try again later.",
                        token);
                };
                options.AddPolicy("pagination", context =>
                {
                    var partitionKey = GetPartitionKey(context);
                    return RateLimitPartition.GetFixedWindowLimiter(
                     partitionKey,
                     factory: _ => new FixedWindowRateLimiterOptions
                     {
                         PermitLimit = 30,
                         Window = TimeSpan.FromMinutes(1),
                         QueueLimit = 14,
                         QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                     }
                 );
                });
                options.AddPolicy("patch", context =>
                {
                    var partitionKey = GetPartitionKey(context);
                    return RateLimitPartition.GetFixedWindowLimiter(
                     partitionKey,
                     factory: _ => new FixedWindowRateLimiterOptions
                     {
                         PermitLimit = 30,
                         Window = TimeSpan.FromMinutes(1),
                         QueueLimit = 14,
                         QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                     }
                 );
                });
                options.AddPolicy("read", context =>
                {
                    var partitionKey = GetPartitionKey(context);
                    return RateLimitPartition.GetFixedWindowLimiter(
                     partitionKey,
                     factory: _ => new FixedWindowRateLimiterOptions
                     {
                         PermitLimit = 120,
                         Window = TimeSpan.FromMinutes(1),
                         QueueLimit = 60,
                         QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                     }
                 );
                });
                options.AddPolicy("write", context =>
                {
                    var partitionKey = GetPartitionKey(context);
                    return RateLimitPartition.GetFixedWindowLimiter(
                     partitionKey,
                     factory: _ => new FixedWindowRateLimiterOptions
                     {
                         PermitLimit = 40,
                         Window = TimeSpan.FromMinutes(1),
                         QueueLimit = 20,
                         QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                     }
                 );
                });
                options.AddPolicy("sensitive", context =>
                {
                    var partitionKey = GetPartitionKey(context);
                    return RateLimitPartition.GetFixedWindowLimiter(
                     partitionKey,
                     factory: _ => new FixedWindowRateLimiterOptions
                     {
                         PermitLimit = 10,
                         Window = TimeSpan.FromMinutes(1),
                         QueueLimit = 5,
                         QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                     }
                 );
                });
            });
            return services;
        }
    }
}