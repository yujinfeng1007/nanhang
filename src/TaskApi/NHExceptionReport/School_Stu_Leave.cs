namespace TaskApi.NHExceptionReport
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class School_Stu_Leave
    {
        [Key]
        [StringLength(50)]
        public string F_Id { get; set; }

        public DateTime? F_CreatorTime { get; set; }

        [StringLength(50)]
        public string F_CreatorUserId { get; set; }

        public bool? F_DeleteMark { get; set; }

        public DateTime? F_DeleteTime { get; set; }

        [StringLength(50)]
        public string F_DeleteUserId { get; set; }

        [StringLength(50)]
        public string F_DepartmentId { get; set; }

        public DateTime? F_LastModifyTime { get; set; }

        [StringLength(50)]
        public string F_LastModifyUserId { get; set; }

        [StringLength(50)]
        public string F_StartTime { get; set; }

        [StringLength(50)]
        public string F_EndTime { get; set; }

        [StringLength(50)]
        public string F_StudentID { get; set; }

        [StringLength(50)]
        public string F_TeacherID { get; set; }

        [StringLength(5)]
        public string F_LeaveDays { get; set; }

        [StringLength(1)]
        public string F_LeaveType { get; set; }

        [StringLength(5000)]
        public string F_ReasonForLeave { get; set; }

        [StringLength(2)]
        public string F_Status { get; set; }

        [StringLength(500)]
        public string F_ApprovalOpinion { get; set; }

        [StringLength(50)]
        public string F_Applicant { get; set; }

        public int? F_SortCode { get; set; }

        public bool? F_EnabledMark { get; set; }

        [StringLength(5000)]
        public string F_Memo { get; set; }
    }
}
