using System;
using ZHXY.Domain;

namespace ZHXY.Application.RequestDto.Api
{
    /// <summary>
    /// 学生请假输入
    /// </summary>
    public class StudentLeaveInput
    {
        /// <summary>
        /// 申请人
        /// </summary>
        public string F_Applicant { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime F_StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime F_EndTime { get; set; }

        /// <summary>
        /// 学生ID
        /// </summary>
        public string F_StudentID { get; set; }

        /// <summary>
        /// 请假天数
        /// </summary>
        public decimal F_LeaveDays { get; set; }

        /// <summary>
        /// 请假类型
        /// </summary>
        public string F_LeaveType { get; set; }

        /// <summary>
        /// 请假事由
        /// </summary>
        public string F_ReasonForLeave { get; set; }

        /// <summary>
        /// 审批状态
        /// </summary>
        public string F_Status { get; set; } = "1";

        /// <summary>
        /// 学校代码
        /// </summary>

        public string F_School_Id { get; set; }

        /// <summary>
        /// 自动类型转换
        /// </summary>
        /// <param name="dto"></param>

        public static implicit operator LeaveOrder(StudentLeaveInput dto)
        {
            return new LeaveOrder
            {
                ApplicantId = dto.F_Applicant,
                StartTime = dto.F_StartTime,
                EndOfTime = dto.F_EndTime,
                LeaveerId = dto.F_StudentID,
                LeaveDays = dto.F_LeaveDays,
                LeaveType = dto.F_LeaveType,
                Reason = dto.F_ReasonForLeave,
                Status = dto.F_Status
            };
        }
    }
}