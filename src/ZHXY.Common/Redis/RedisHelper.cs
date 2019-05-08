using System;
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

        public static IDatabase GetDatabase()
        {
            var schoolCode = OperatorProvider.Current.SchoolCode;
            if (string.IsNullOrEmpty(schoolCode)) throw new NoLoggedInException();
            var dbNumber = Convert.ToByte(ConfigurationManager.AppSettings[$"{schoolCode}_redis_database"]);
            return _instance.GetDatabase(dbNumber);
        }

        public static IDatabase GetDatabase(int db)
        {
            return _instance.GetDatabase(db);
        }

        public static object GetClient(int v)
        {
            throw new NotImplementedException();
        }
    }
}