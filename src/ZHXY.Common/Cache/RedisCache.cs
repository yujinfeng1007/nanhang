using System;
using StackExchange.Redis;

namespace ZHXY.Common
{
    public class RedisCache
    {
        private static int DbIndex { get; } = 15;
        private static IDatabase CacheDb { get; } = RedisHelper.GetDatabase(DbIndex);
        private static TimeSpan DefaultExpiry { get; } = new TimeSpan(5, 0, 0);

        public static T Get<T>(string key) where T : class
        {
            var exists = CacheDb.KeyExists(key);
            return exists ? CacheDb.StringGet(key).ToString().Deserialize<T>() : null;
        }

        public static bool KeyExists(string key) => CacheDb.KeyExists(key);

        public static bool Set<T>(string key, T value) where T : class => CacheDb.StringSet(key, value.Serialize(), DefaultExpiry);

        public static bool Set<T>(string key, T value, DateTime expireTime) where T : class => CacheDb.StringSet(key, value.Serialize(), expireTime - DateTime.Now);

        public static bool Remove(string key) => CacheDb.KeyDelete(key);

        public static void Clear() => RedisHelper.FlushDatabase(DbIndex);
       
    }

}