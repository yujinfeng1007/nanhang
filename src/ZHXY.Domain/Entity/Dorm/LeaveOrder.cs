using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZHXY.Domain
{
    /// <summary>
    /// 请假单
    /// </summary>
    public class LeaveOrder : IEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N").ToUpper();


        public DateTime CreatedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 申请人id
        /// </summary>
        public string ApplicantId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndOfTime { get; set; }

        /// <summary>
        /// 请假学生的id
        /// </summary>
        public string LeaveerId { get; set; }

        /// <summary>
        /// 班主任id
        /// </summary>
        public string HeadTeacherId { get; set; }

        /// <summary>
        /// 请假天数
        /// </summary>
        public decimal LeaveDays { get; set; }        

        /// <summary>
        /// 请假类型
        /// </summary>
        public string LeaveType { get; set; }

        /// <summary>
        /// 请假理由
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 状态(0:未审批 1:已审批)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 审批意见
        /// </summary>

        public string Opinion { get; set; }

      
        public string AttachmentsPath { get; set; }
    }
}