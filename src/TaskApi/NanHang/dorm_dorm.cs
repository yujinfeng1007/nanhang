namespace TaskApi.NanHang
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class dorm_dorm
    {
        [Key]
        [StringLength(50)]
        public string F_Id { get; set; }

        public DateTime? F_CreatorTime { get; set; }

        public int? F_SortCode { get; set; }

        [StringLength(50)]
        public string F_DepartmentId { get; set; }

        public bool? F_DeleteMark { get; set; }

        [StringLength(50)]
        public string F_CreatorUserId { get; set; }

        public DateTime? F_LastModifyTime { get; set; }

        [StringLength(50)]
        public string F_LastModifyUserId { get; set; }

        public DateTime? F_DeleteTime { get; set; }

        [StringLength(500)]
        public string F_Memo { get; set; }

        [StringLength(50)]
        public string F_Area { get; set; }

        [StringLength(50)]
        public string F_Building_No { get; set; }

        [StringLength(50)]
        public string F_Floor_No { get; set; }

        [StringLength(50)]
        public string F_Unit_No { get; set; }

        public int? F_Accommodate_No { get; set; }

        [StringLength(50)]
        public string F_Classroom_Type { get; set; }

        [StringLength(50)]
        public string F_Classroom_No { get; set; }

        [StringLength(50)]
        public string F_Sex { get; set; }

        public int? F_In { get; set; }

        public int? F_Free { get; set; }

        [StringLength(50)]
        public string F_Classroom_Status { get; set; }

        public bool? F_EnabledMark { get; set; }

        [StringLength(50)]
        public string F_DeleteUserId { get; set; }

        [StringLength(500)]
        public string F_Title { get; set; }

        [StringLength(50)]
        public string F_Leader_ID { get; set; }

        [StringLength(500)]
        public string F_Leader_Name { get; set; }

        [StringLength(50)]
        public string F_Manager_ID { get; set; }

        [StringLength(50)]
        public string F_Manager_Name { get; set; }
    }
}
