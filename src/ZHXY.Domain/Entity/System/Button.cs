using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 模块按钮
    /// </summary>
    public class Button : IEntity
    {
        public string Id { get; set; }
        public string ModuleId { get; set; }
        public string ParentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public int? Sort { get; set; }
    }
}