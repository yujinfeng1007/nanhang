using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 用户登录
    /// </summary>
    public class UserLogin : IEntity
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserPassword { get; set; }
        public string UserSecretkey { get; set; }
        public DateTime? PreviousVisitTime { get; set; }
        public DateTime? LastVisitTime { get; set; }
        public int? LogOnCount { get; set; }
    }
}