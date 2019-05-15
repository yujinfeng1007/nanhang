
using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 模块按钮
    /// </summary>
    public class Button : IEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N").ToUpper();
        public string MenuId { get; set; }
        public string EnCode { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int? SortCode { get; set; }
   
    }
}