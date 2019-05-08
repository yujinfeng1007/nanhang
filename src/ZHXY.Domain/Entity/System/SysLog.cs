using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 日志
    /// </summary>
    public class SysLog : IEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N").ToUpper();
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public string UserId { get; set; }
        public string Account { get; set; }
        public string NickName { get; set; }
        public string Type { get; set; }
        public string IPAddress { get; set; }
        public string IPAddressName { get; set; }
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public bool? Result { get; set; }
        public string Description { get; set; }
    }
}