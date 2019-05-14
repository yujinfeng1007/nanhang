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
        public decimal F_Time { get; set; }

        /// <summary>
        /// 进宿舍时间
        /// </summary>
        public DateTime? F_InTime { get; set; }

        public virtual Organ Class { get; set; }
        public virtual DormRoom Dorm{ get; set; }
    }
}
