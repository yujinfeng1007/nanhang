using Newtonsoft.Json;
using ZHXY.Common;

namespace ZHXY.Application
{
    public class ContactView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public string GroupName => Name.GetFirstPinyin()[0].ToString();
    }
}