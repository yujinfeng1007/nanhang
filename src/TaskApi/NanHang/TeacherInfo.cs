namespace TaskApi.NanHang
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TeacherInfo")]
    public partial class TeacherInfo
    {
        [Key]
        [StringLength(50)]
        public string teacherId { get; set; }

        [StringLength(100)]
        public string teacherName { get; set; }

        [StringLength(50)]
        public string LoginId { get; set; }

        [StringLength(50)]
        public string orgId { get; set; }

        [StringLength(50)]
        public string teacherNo { get; set; }

        public int? Sex { get; set; }

        [StringLength(50)]
        public string teacherPhone { get; set; }

        [StringLength(50)]
        public string certificateType { get; set; }

        [StringLength(50)]
        public string certificateNo { get; set; }

        [StringLength(100)]
        public string ImgUri { get; set; }

        [Column(TypeName = "date")]
        public DateTime? uploadTime { get; set; }

        [Column(TypeName = "date")]
        public DateTime? expirationTime { get; set; }

        public int? teacherStatus { get; set; }

        public int? ImgStatus { get; set; }
    }
}
