using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 销假记录
    /// </summary>
    public class LeaveSuspend : IEntity
    {
        /// <summary>
        /// 请假单Id
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 销假天数
        /// </summary>
        public int Days { get; set; }

        /// <summary>
        /// 销假时间
        /// </summary>
        public DateTime Time { get; set; } = DateTime.Now;
    }
}