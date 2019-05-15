using System.Configuration;
using StackExchange.Redis;

namespace ZHXY.Common
{
    public static class RedisHelper
    {
        private static readonly ConnectionMultiplexer _instance;

        static RedisHelper()
        {
            var connectionString = ConfigurationManager.AppSettings["redis_server_session"];
            _instance= ConnectionMultiplexer.Connect(connectionString);
        }

        public static IDatabase GetDatabase(int db)
        {
            return _instance.GetDatabase(db);
        }
      
    }
}