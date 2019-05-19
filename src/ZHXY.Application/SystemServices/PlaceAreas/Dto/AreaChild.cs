using System.Collections.Generic;

namespace ZHXY.Application
{
    /// <summary>
    /// 地区
    /// </summary>
    public class AreaView 
    {
        public string Value { get; set; }
        public string Label { get; set; }
        public List<AreaView> Children { get; set; }
    }
}