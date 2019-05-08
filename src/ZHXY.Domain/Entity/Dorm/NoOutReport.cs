using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 长时间未出报表
    /// </summary>
    public class NoOutReport:ReportEntity
    {
        /// <summary>
        /// 未出时长
        /// </summary>
        public int F_Time { get; set; }

        /// <summary>
        /// 进宿舍时间
        /// </summary>
        public DateTime? F_InTime { get; set; }

        public virtual Organize Class { get; set; }
        public virtual DormRoom Dorm{ get; set; }
    }
}
