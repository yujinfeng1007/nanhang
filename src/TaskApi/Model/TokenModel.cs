using System.Collections.Generic;

namespace TaskApi.Model
{
    public class TokenModel
    {
        public string code { get; set; }
        public string msg { get; set; }
        public string tip { get; set; }
        public string requestId { get; set; }
        public Dictionary<string, object> data { get; set; }
    }
}