using System;

namespace ZHXY.Application
{
    /// <summary>
    /// 请假申请Dto
    /// </summary>
    public class LeaveRequestDto
    {
        /// <summary>
        /// 请假人Id
        /// </summary>
        public string LeaveerId { get; set; }

        /// <summary>
        /// 请假类型
        /// </summary>
        public string LeaveType { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndOfTime { get; set; }

        /// <summary>
        /// 请假天数
        /// </summary>
        public decimal LeaveDays { get; set; }

        /// <summary>
        /// 请假事由
        /// </summary>
        public string ReasonForLeave { get; set; }

        /// <summary>
        /// 附件地址
        /// </summary>
        public string AttachmentsPath { get; set; }

        /// <summary>
        /// 审批人
        /// </summary>
        public string[] Approvers { get; set; }
    }
}