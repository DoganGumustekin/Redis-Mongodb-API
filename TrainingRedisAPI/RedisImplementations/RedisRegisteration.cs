using StackExchange.Redis;

namespace TrainingRedisAPI.RedisImplementations
{
    public static class RedisRegisteration
    {
        public static IConnectionMultiplexer ConfigureRedis(this IServiceProvider service, IConfiguration configuration)
        {
            var redisConf = ConfigurationOptions.Parse(configuration["RedisSettings:ConnectionString"], true);
            redisConf.ResolveDns = true;
            return ConnectionMultiplexer.Connect(redisConf);
        }
    }
}
