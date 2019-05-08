using System;

namespace ZHXY.Application
{
    [Serializable]
    public class FieldItem
    {
        public string encode { get; set; }
        public string fullname { get; set; }
        public string parentid { get; set; }
        public int? level { get; set; }
    }
}