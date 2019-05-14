namespace TaskApi.NHExceptionReport
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Dorm_NoOutReport
    {
        [Key]
        [StringLength(50)]
        public string F_Id { get; set; }

        [StringLength(50)]
        public string F_CreatorUserId { get; set; }

        public DateTime? F_CreatorTime { get; set; }

        [StringLength(50)]
        public string F_DepartmentId { get; set; }

        [StringLength(50)]
        public string F_LastModifyUserId { get; set; }

        public DateTime? F_LastModifyTime { get; set; }

        public bool? F_DeleteMark { get; set; }

        [StringLength(50)]
        public string F_DeleteUserId { get; set; }

        public DateTime? F_DeleteTime { get; set; }

        [StringLength(50)]
        public string F_Memo { get; set; }

        public int? F_SortCode { get; set; }

        public bool? F_EnabledMark { get; set; }

        [StringLength(50)]
        public string F_Account { get; set; }

        [StringLength(50)]
        public string F_Name { get; set; }

        [StringLength(50)]
        public string F_College { get; set; }

        [StringLength(50)]
        public string F_Profession { get; set; }

        [StringLength(50)]
        public string F_Class { get; set; }

        [StringLength(50)]
        public string F_Dorm { get; set; }

        public DateTime? F_InTime { get; set; }

        public decimal? F_Time { get; set; }

        public DateTime? F_OutTime { get; set; }

        [StringLength(50)]
        public string F_StudentId { get; set; }
    }
}
