using System.Configuration;

namespace ZHXY.Common
{
    public class Configs
    {
        /// <summary>
        ///     根据Key取Value值
        /// </summary>
        /// <param name="key">  </param>
        public static string GetValue(string key) => ConfigurationManager.AppSettings[key]?.Trim();
        public static void SetValue(string key, string value)
        {
            ConfigurationManager.AppSettings.Set(key, value);
        }
    }
}