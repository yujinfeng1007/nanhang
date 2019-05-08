namespace TaskApi.NanHang
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("StudentInfo")]
    public partial class StudentInfo
    {
        [Key]
        [StringLength(50)]
        public string studentId { get; set; }

        [StringLength(50)]
        public string studentName { get; set; }

        [StringLength(50)]
        public string LoginId { get; set; }

        [StringLength(50)]
        public string studentNo { get; set; }

        [StringLength(50)]
        public string orgId { get; set; }

        [StringLength(100)]
        public string orgName { get; set; }

        [StringLength(50)]
        public string studentBuildingId { get; set; }

        [StringLength(50)]
        public string studentSex { get; set; }

        [StringLength(50)]
        public string certificateType { get; set; }

        [StringLength(50)]
        public string certificateNo { get; set; }

        [StringLength(50)]
        public string studentGrade { get; set; }

        [StringLength(50)]
        public string studentClass { get; set; }

        [StringLength(200)]
        public string studentMeto { get; set; }

        [StringLength(500)]
        public string ImgUri { get; set; }

        [Column(TypeName = "date")]
        public DateTime? uploadTime { get; set; }

        [Column(TypeName = "date")]
        public DateTime? expirationTime { get; set; }

        public int? Status { get; set; }

        [StringLength(50)]
        public string studentPhone { get; set; }
    }
}
