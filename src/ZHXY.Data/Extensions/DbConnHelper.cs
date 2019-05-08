using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace ZHXY.Data
{
    public static class DbConnHelper
    {
        private static readonly object Obj = new object();
        private static Dictionary<string, string> Dic { get; set; } = new Dictionary<string, string>();

        public static string GetConnectionString(string schoolCode)
        {
            lock (Obj)
            {
                if (!Dic.ContainsKey(schoolCode))
                {
                    var value = GetConString(schoolCode);
                    lock (Obj)
                    {
                        Dic.Add(schoolCode, value);
                    }
                }
            }

            lock (Obj)
            {
                return Dic[schoolCode];
            }
        }

        private static string GetConString(string schoolCode)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[schoolCode]?.ConnectionString;
            if (!string.IsNullOrEmpty(connectionString)) return connectionString;
            var url = ConfigurationManager.AppSettings["getSchoolDbConnecionStringUrl"];
            var result = new HttpClient().GetStringAsync($"{url}{schoolCode}").Result;
            var j = JObject.Parse(result);
            var isError = (bool)j.GetValue("IsError", StringComparison.InvariantCultureIgnoreCase);
            if (isError) throw new Exception("获取数据库连接失败!");
            try
            {
                var data = JObject.FromObject(j.GetValue("Data", StringComparison.InvariantCultureIgnoreCase));
                var ip = data.GetValue("ip", StringComparison.InvariantCultureIgnoreCase);
                var userName = data.GetValue("userName", StringComparison.InvariantCultureIgnoreCase);
                var password = data.GetValue("password", StringComparison.InvariantCultureIgnoreCase);
                var dbName = data.GetValue("dbName", StringComparison.InvariantCultureIgnoreCase);
                connectionString = $"Data Source={ip};Initial Catalog={dbName};Persist Security Info=True;User ID={userName};Password={password}";
                return connectionString;
            }
            catch
            {
                throw new Exception("获取数据库连接失败!");
            }
        }
    }
}