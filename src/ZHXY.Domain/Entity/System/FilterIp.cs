using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 过滤IP
    /// </summary>
    public class FilterIp : IEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N").ToUpper();
        public bool? Type { get; set; }
        public string StartIp { get; set; }
        public string EndIp { get; set; }
        public string Description { get; set; }
    }
}