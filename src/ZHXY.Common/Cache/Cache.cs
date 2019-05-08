using System;
using System.Web;

namespace ZHXY.Common
{
    public class Cache : ICache
    { 
        public T GetCache<T>(string cacheKey) where T : class
        {
            if (HttpRuntime.Cache[cacheKey] != null) return (T)HttpRuntime.Cache[cacheKey];
            return default;
        }

        public void WriteCache<T>(T value, string cacheKey) where T : class
        {
            HttpRuntime.Cache.Insert(cacheKey, value, null, DateTime.Now.AddHours(5),
                System.Web.Caching.Cache.NoSlidingExpiration);
        }

        public void WriteCache<T>(T value, string cacheKey, DateTime expireTime) where T : class
        {
            HttpRuntime.Cache.Insert(cacheKey, value, null, expireTime, System.Web.Caching.Cache.NoSlidingExpiration);
        }

        public void RemoveCache(string cacheKey)
        {
            HttpRuntime.Cache.Remove(cacheKey);
        }

        public void RemoveCache()
        {
            var CacheEnum = HttpRuntime.Cache.GetEnumerator();
            while (CacheEnum.MoveNext()) HttpRuntime.Cache.Remove(CacheEnum.Key.ToString());
        }
    }

}