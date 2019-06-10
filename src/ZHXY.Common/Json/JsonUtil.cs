using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace ZHXY.Common
{
    public static class JsonUtil
    {
        /// <summary>
        /// 默认时间转换器
        /// </summary>
        private static IsoDateTimeConverter DefaultTimeConverter { get; }

        /// <summary>
        /// 默认序列化设置
        /// </summary>
        private static JsonSerializerSettings DefaultSerializerSettings { get; }

        static JsonUtil()
        {
            DefaultTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };

            DefaultSerializerSettings = new JsonSerializerSettings();
            DefaultSerializerSettings.Converters.Add(DefaultTimeConverter);
            DefaultSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }


        public static string ToJson(this object obj) => JsonConvert.SerializeObject(obj, DefaultTimeConverter);

        public static string Serialize(this object obj) => JsonConvert.SerializeObject(obj, DefaultSerializerSettings);

        public static T Deserialize<T>(this string json) => json == null ? default : JsonConvert.DeserializeObject<T>(json);

        public static JObject Parse2JObject(this string json) => json == null ? JObject.Parse("{}") : JObject.Parse(json);
    }
}