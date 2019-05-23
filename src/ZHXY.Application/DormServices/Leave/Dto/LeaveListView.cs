using System;

namespace ZHXY.Application
{
    /// <summary>
    /// 请假列表Dto
    /// </summary>
    public class LeaveListView
    {
        /// <summary>
        /// 请假Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 请假人名称
        /// </summary>
        public string LeaveerName { get; set; }

        /// <summary>
        /// 请假开始时间
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 请假结束时间
        /// </summary>
        public string EndOfTime { get; set; }

        /// <summary>
        /// 请假天数
        /// </summary>
        public decimal LeaveDays { get; set; }

        /// <summary>
        /// 请假类型
        /// </summary>
        public string LeaveType { get; set; }

        /// <summary>
        /// 审批状态
        /// </summary>
        public string ApprovalStatus { get; set; }

        /// <summary>
        /// 请假事由
        /// </summary>
        public string ReasonForLeave { get; set; }

        public DateTime? CreatedTime { get; set; }
    }

    public class LeaveView: LeaveListView
    {
        public bool IsFinal { get; set; }
        public string AttachmentsPath { get;  set; }
    }

}