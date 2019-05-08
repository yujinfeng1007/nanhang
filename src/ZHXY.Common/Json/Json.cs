using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Data;

namespace ZHXY.Common
{
    public static class Json
    {
        public static object ToJson(this string Json) => Json == null ? null : JsonConvert.DeserializeObject(Json);

        public static string ToJson(this object obj)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }

        public static string ToCamelJson(this object obj)
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return JsonConvert.SerializeObject(obj, settings);
        }

        public static string ToJson(this object obj, string datetimeformats)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = datetimeformats };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }

        public static string ToCamelJson(this object obj, string datetimeformats)
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new IsoDateTimeConverter { DateTimeFormat = datetimeformats });
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return JsonConvert.SerializeObject(obj, settings);
        }

        public static T ToObject<T>(this string Json) => Json == null ? default : JsonConvert.DeserializeObject<T>(Json);

        public static List<T> ToList<T>(this string Json) => Json == null ? null : JsonConvert.DeserializeObject<List<T>>(Json);

        public static DataTable ToTable(this string Json) => Json == null ? null : JsonConvert.DeserializeObject<DataTable>(Json);

        public static JObject ToJObject(this string Json) => Json == null ? JObject.Parse("{}") : JObject.Parse(Json.Replace("&nbsp;", ""));
    }
}