namespace ZHXY.Api.moudle
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Teacher_Organ
    {
        [Key]
        [StringLength(50)]
        public string OrgId { get; set; }

        [Required]
        [StringLength(100)]
        public string OrgName { get; set; }

        [StringLength(50)]
        public string ParentOrgId { get; set; }

        public int? OrgIndex { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedTime { get; set; }

        [Column(TypeName = "date")]
        public DateTime LastUpdatedTime { get; set; }
    }
}
