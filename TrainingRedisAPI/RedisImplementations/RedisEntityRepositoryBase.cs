using StackExchange.Redis;
using System.Text.Json;

namespace TrainingRedisAPI.RedisImplementations
{
    public class RedisEntityRepositoryBase : IRedisEntityRepository
    {
        private readonly IDatabase _redisDB;

        public RedisEntityRepositoryBase(IConnectionMultiplexer connection)
        {
            _redisDB = connection.GetDatabase();
        }

        public byte[] Get(string key) => _redisDB.StringGet(key);
        public T Get<T>(string key)
        {
            var value = _redisDB.StringGet(key);
            return value.HasValue ? JsonSerializer.Deserialize<T>(value) : default;
        }
        public void Set(string key, object value)
        {
            var serilizedValue = JsonSerializer.Serialize(value);
            _redisDB.StringSet(key, serilizedValue,TimeSpan.FromMinutes(45));
        }
        public void Refresh(string key) => _redisDB.KeyExpire(key, TimeSpan.FromMinutes(30));
        public bool Any(string key) => _redisDB.KeyExists(key);
        public void Remove(string key) => _redisDB.KeyDelete(key);
        public void Increment(string key, string field, int incrementCount) => _redisDB.HashIncrement(key,field,incrementCount);
        public void HashSet(string key, string field, object value)
        {
            var serializedValue = JsonSerializer.Serialize(value);
            _redisDB.HashSet(key, field, serializedValue);
            _redisDB.KeyExpire(key,TimeSpan.FromMinutes(45));
        }
        public T HashGet<T>(string key, string field)
        {
            if (!_redisDB.KeyExists(key))
            {
                throw new Exception($"Gönderi Bulunamadı {key}");
            }
            if (!_redisDB.HashExists(key,field))
            {
                throw new Exception($"Gönderi Bulunamadı {key} , {field}");
            }
            var value = _redisDB.HashGet(key, field);
            return value.HasValue ? JsonSerializer.Deserialize<T>(value) : default;
        }
        public Dictionary<string, T> HashGetAll<T>(string key)
        {
            var hashEntries = _redisDB.HashGetAll(key);
            return hashEntries.ToDictionary(
                entry => entry.Name.ToString(),
                entry => JsonSerializer.Deserialize<T>(entry.Value)
            );
        }
        public bool HashFieldExist(string key, string field) => _redisDB.HashExists(key, field);
        public void HashRemove(string key, string field) => _redisDB.HashDelete(key, field);
    }
}