namespace TrainingRedisAPI.RedisImplementations
{
    public interface IRedisEntityRepository
    {
        byte[] Get(string key);
        T Get<T>(string key);
        void Set(string key, object value);
        void Refresh(string key);
        bool Any(string key);
        void Remove(string key);
        void Increment(string key, string field, int incrementCount);
        void HashSet(string key, string field, object value);
        T HashGet<T>(string key, string field);
        Dictionary<string, T> HashGetAll<T>(string key);
        bool HashFieldExist(string key, string field);
        void HashRemove(string key, string field);
    }
}
