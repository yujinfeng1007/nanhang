namespace TaskApi.NHExceptionReport
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Dorm_DormStudent
    {
        [StringLength(50)]
        public string F_Student_ID { get; set; }

        [StringLength(50)]
        public string F_DormId { get; set; }

        [StringLength(50)]
        public string F_Bed_ID { get; set; }

        public DateTime? F_In_Time { get; set; }

        [Key]
        [StringLength(50)]
        public string F_Id { get; set; }

        public DateTime? F_CreatorTime { get; set; }

        public int? F_SortCode { get; set; }

        [StringLength(50)]
        public string F_DepartmentId { get; set; }

        public bool? F_DeleteMark { get; set; }

        [StringLength(50)]
        public string F_Sex { get; set; }

        [StringLength(50)]
        public string F_CreatorUserId { get; set; }

        public DateTime? F_LastModifyTime { get; set; }

        [StringLength(50)]
        public string F_LastModifyUserId { get; set; }

        public DateTime? F_DeleteTime { get; set; }

        [StringLength(500)]
        public string F_Memo { get; set; }

        [StringLength(50)]
        public string F_DeleteUserId { get; set; }

        public bool? F_EnabledMark { get; set; }
    }
}
