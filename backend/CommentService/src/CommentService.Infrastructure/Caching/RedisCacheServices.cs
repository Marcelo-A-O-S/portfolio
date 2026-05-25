using CommentService.Application.Interfaces;
using StackExchange.Redis;

namespace CommentService.Infrastructure.Caching
{
    public class RedisCacheServices : ICacheServices
    {
        private readonly string _prefix = "CommentService:";
        private readonly IDatabase database;
        public RedisCacheServices(IConnectionMultiplexer _redis)
        {
            this.database = _redis.GetDatabase();
        }
        public async Task<string?> GetAsync(string key)
            => await this.database.StringGetAsync(BuildKey(key));

        public async Task RemoveAsync(string key)
            => await this.database.KeyDeleteAsync(BuildKey(key));

        public async Task SetAsync(string key, string value, TimeSpan ttl) 
            => await this.database.StringSetAsync(BuildKey(key), value, ttl);
        private string BuildKey(string key) => $"{_prefix}{key}";
    }
}