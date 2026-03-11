namespace Gateway.API.Extension
{
    public static class BlockSimpleBotExtension
    {
        public static IApplicationBuilder UseSimpleBlockBot(
            this IApplicationBuilder app
        )
        {
            app.Use(async (context, next) =>
            {
                var agent = context.Request.Headers.UserAgent.ToString().ToLower();
                string[] blockedAgents =
                {
                    "curl","wget","python","nmap"
                };
                if (blockedAgents.Any(a => agent.Contains(a)))
                {
                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsync("Forbidden");
                    return;
                }
                await next();
            });
            return app;
        }
    }
}