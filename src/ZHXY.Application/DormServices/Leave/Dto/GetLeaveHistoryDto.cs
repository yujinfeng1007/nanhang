using System;
using ZHXY.Common;

namespace ZHXY.Application
{
    /// <summary>
    /// 获取请假历史Dto
    /// </summary>
    public class GetLeaveHistoryDto: Pagination
    {
        /// <summary>
        /// 用户Id(请假人Id)
        /// </summary>
        public string Keyword { get; set; }
        public string UserId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 截至时间
        /// </summary>
        public DateTime EndOfTime { get; set; }

        /// <summary>
        /// 审批状态
        /// </summary>
        public string Status { get; set; }

    }
}