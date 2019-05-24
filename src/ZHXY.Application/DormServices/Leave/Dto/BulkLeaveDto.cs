using System;

namespace ZHXY.Application
{
    /// <summary>
    /// 批量请假Dto
    /// </summary>
    public class BulkLeaveDto
    {
        /// <summary>
        /// 请假人
        /// </summary>
        public string[] Leaveers { get; set; }

        /// <summary>
        /// 请假事项
        /// </summary>
        public string ReasonForLeave { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndOfTime { get; set; }

    }
}