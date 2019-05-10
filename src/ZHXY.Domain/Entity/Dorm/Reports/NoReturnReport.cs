using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 未归报表
    /// </summary>
    public class NoReturnReport : ReportEntity
    {
        /// <summary>
        /// 未归天数
        /// </summary>
        public int F_DayCount { get; set; }
        public DateTime? F_OutTime { get; set; }
        public virtual Organize Class { get; set; }
        public virtual DormRoom Dorm { get; set; }
    }
}
