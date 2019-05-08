using System;
using System.Collections.Generic;

namespace ZHXY.Domain
{
    /// <summary>
    /// 子地区
    /// </summary>
    [Serializable]
    public class AreaChild : EntityBase
    {
        public string value { get; set; }
        public string label { get; set; }
        public List<AreaChild> children { get; set; }
    }
}