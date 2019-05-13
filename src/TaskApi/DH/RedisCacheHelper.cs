using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace TaskApi.DH
{
    class RedisCacheHelper
    {
        private static readonly PooledRedisClientManager pool = null;
        private static readonly string[] writeHosts = null;
        private static readonly string[] readHosts = null;
        public static int RedisMaxReadPool = int.Parse(ConfigurationManager.AppSettings["redis_max_read_pool"]);
        public static int RedisMaxWritePool = int.Parse(ConfigurationManager.AppSettings["redis_max_write_pool"]);
        static RedisCacheHelper()
        {
            var redisWriteHost = ConfigurationManager.AppSettings["redis_server_write"];
            var redisReadHost = ConfigurationManager.AppSettings["redis_server_read"];
            if (!string.IsNullOrEmpty(redisWriteHost))
            {
                writeHosts = redisWriteHost.Split(',');
                readHosts = redisReadHost.Split(',');
                if (readHosts.Length > 0)
                {
                    pool = new PooledRedisClientManager(writeHosts, readHosts,
                        new RedisClientManagerConfig()
                        {
                            MaxWritePoolSize = RedisMaxWritePool,
                            MaxReadPoolSize = RedisMaxReadPool,

                            AutoStart = true
                        });
                }
            }
        }
        public static void Add<T>(string key, T value, DateTime expiry)
        {
            if (value == null)
            {
                return;
            }

            if (expiry <= DateTime.Now)
            {
                Remove(key);

                return;
            }

            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            r.Set(key, value, expiry - DateTime.Now);
                        }
                    }
                }
            }
            catch 
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "存储", key);
            }

        }

        public static void Add<T>(string key, T value)
        {
            RedisCacheHelper.Add<T>(key, value, DateTime.Now.AddDays(1));
        }

        public static void Add<T>(string key, T value, TimeSpan slidingExpiration)
        {
            if (value == null)
            {
                return;
            }

            if (slidingExpiration.TotalSeconds <= 0)
            {
                Remove(key);
                return;
            }
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            r.Set(key, value, slidingExpiration);
                        }
                    }
                }
            }
            catch 
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "存储", key);
            }

        }

        public static T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default(T);
            }
            var obj = default(T);
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            obj = r.Get<T>(key);
                        }
                    }
                }
            }
            catch 
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "获取", key);
            }
            return obj;
        }

        public static void Remove(string key)
        {
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            r.Remove(key);
                        }
                    }
                }
            }
            catch
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "删除", key);
            }

        }

        public static bool Exists(string key)
        {
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            return r.ContainsKey(key);
                        }
                    }
                }
            }
            catch 
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "是否存在", key);
            }

            return false;
        }

        public static IDictionary<string, T> GetAll<T>(IEnumerable<string> keys) where T : class
        {
            if (keys == null)
            {
                return null;
            }
            keys = keys.Where(k => !string.IsNullOrWhiteSpace(k));

            if (keys.Count() == 1)
            {
                var obj = Get<T>(keys.Single());

                if (obj != null)
                {
                    return new Dictionary<string, T>() { { keys.Single(), obj } };
                }

                return null;
            }
            if (!keys.Any())
            {
                return null;
            }
            try
            {
                using (var r = pool.GetClient())
                {
                    if (r != null)
                    {
                        r.SendTimeout = 1000;
                        return r.GetAll<T>(keys);
                    }
                }
            }
            catch 
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "获取", keys.Aggregate((a, b) => a + "," + b));
            }
            return null;
        }
    }
}
