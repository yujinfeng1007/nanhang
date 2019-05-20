using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 用户登录
    /// </summary>
    public class UserLogin : IEntity
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 安全码
        /// </summary>
        public string Secretkey { get; set; }
        /// <summary>
        /// 上次访问时间
        /// </summary>
        public DateTime? PreVisitTime { get; set; }
        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime? LastVisitTime { get; set; }
        /// <summary>
        /// 登录总次数
        /// </summary>
        public int? LoginCount { get; set; }
    }
}