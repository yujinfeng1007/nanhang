using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 模块
    /// </summary>
    public class Module: IEntity
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string Target { get; set; }
        public bool? IsMenu { get; set; }
        public bool? IsExpand { get; set; }
        public int? Sort { get; set; }
        public bool? Enabled { get; set; }

        public string BelongSys { get; set; }
    }
}