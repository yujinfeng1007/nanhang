using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 晚归报表
    /// </summary>
    public class LateReturnReport: ReportEntity
    {
        /// <summary>
        /// 进宿舍时间
        /// </summary>
        public DateTime? F_InTime { get; set; }
        /// <summary>
        /// 晚归时长
        /// </summary>
        public double F_Time { get; set; }
        public virtual Organize Class { get; set; }
        public virtual DormRoom Dorm { get; set; }
    }
}
