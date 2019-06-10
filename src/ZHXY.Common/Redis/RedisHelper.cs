using System.Configuration;
using StackExchange.Redis;

namespace ZHXY.Common
{
    public static class RedisHelper
    {
        private static readonly ConnectionMultiplexer Instance;

        private static string ConnectionString { get; } = ConfigurationManager.ConnectionStrings["redis"].ConnectionString;
        static RedisHelper() => Instance = ConnectionMultiplexer.Connect(ConnectionString);
        public static IDatabase GetDatabase(int db = 0) => Instance.GetDatabase(db);

        public static void FlushDatabase(int db = 0) => Instance.GetServer(ConnectionString).FlushDatabase(db);
    }
}