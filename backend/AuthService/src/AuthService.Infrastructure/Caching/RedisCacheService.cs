using AuthService.Application.Configurations;
using AuthService.Application.Interfaces;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace AuthService.Infrastructure.Caching
{
    public class RedisCacheService : ICacheServices
    {
        private readonly IDatabase database;
        private readonly RedisOptions redisOptions;
        public RedisCacheService(
            IConnectionMultiplexer _redis,
            IOptions<RedisOptions> _redisOptions
            )
        {
            this.database = _redis.GetDatabase();
            this.redisOptions = _redisOptions.Value;
        }
        public async Task<string?> GetAsync(string key)
            => await this.database.StringGetAsync(BuildKey(key));

        public async Task RemoveAsync(string key)
            => await this.database.KeyDeleteAsync(BuildKey(key));

        public async Task SetAsync(string key, string value, TimeSpan ttl)
            => await this.database.StringSetAsync(BuildKey(key), value, ttl);
        private string BuildKey(string key)
        {
            if(string.IsNullOrWhiteSpace(this.redisOptions.InstanceName))
                throw new Exception("O nome da instância do redis não foi configurada.");
            return $"{this.redisOptions.InstanceName}{key}";
        }
    }
}