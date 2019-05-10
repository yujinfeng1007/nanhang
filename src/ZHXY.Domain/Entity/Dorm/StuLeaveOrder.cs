using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZHXY.Domain
{
    /// <summary>
    /// 学生请假
    /// </summary>
    public class StuLeaveOrder : IEntity
    {
        [Key]
        [Column("F_Id", TypeName = "varchar")]
        [MaxLength(50)]
        public string Id { get; set; } = Guid.NewGuid().ToString("N").ToUpper();

        [Column("F_Memo", TypeName = "varchar")]
        [MaxLength(500)]
        public string Remark { get; set; }

        /// <summary>
        ///     创建人(外键)
        /// </summary>
        [Column("F_CreatorUserId", TypeName = "varchar")]
        [StringLength(50)]
        public string CreatedByUserId { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [Column("F_CreatorTime", TypeName = "datetime")]
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        /// <summary>
        ///     所属部门
        /// </summary>
        [Column("F_DepartmentId", TypeName = "varchar")]
        [StringLength(50)]
        public string OwnerDeptId { get; set; }

        /// <summary>
        ///     最后修改的用户(外键)
        /// </summary>
        [Column("F_LastModifyUserId", TypeName = "varchar")]
        [StringLength(50)]
        public string LastModifiedByUserId { get; set; }

        /// <summary>
        ///     最后修改的时间
        /// </summary>
        [Column("F_LastModifyTime", TypeName = "datetime")]
        public DateTime? LastModifiedTime { get; set; }

        /// <summary>
        ///     删除标记(逻辑删除)
        /// </summary>
        [Column("F_DeleteMark")]
        public bool IsDeleted { get; set; }

        /// <summary>
        ///     删除的用户(外键)
        /// </summary>
        [Column("F_DeleteUserId", TypeName = "varchar")]
        [StringLength(50)]
        public string DeletedByUserId { get; set; }

        /// <summary>
        ///     删除的时间
        /// </summary>
        [Column("F_DeleteTime", TypeName = "datetime")]
        public DateTime? DeletedTime { get; set; }

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
        public string LeaveDays { get; set; }

        [NotMapped]
        public decimal Days { get { return Convert.ToDecimal(LeaveDays); } }

        /// <summary>
        /// 请假类型
        /// </summary>
        public string LeaveType { get; set; }

        /// <summary>
        /// 请假理由
        /// </summary>
        public string ReasonForLeave { get; set; }

        /// <summary>
        /// 状态(0:未审批 1:已审批)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 审批意见
        /// </summary>

        public string ApprovalOpinion { get; set; }

        /// <summary>
        /// 班主任
        /// </summary>
        public virtual User HeadTeacher { get; set; }

        /// <summary>
        /// 请假人
        /// </summary>
        public virtual User Leaveer { get; set; }

        /// <summary>
        /// 申请人
        /// </summary>
        public virtual User Applicant { get; set; }
    }
}