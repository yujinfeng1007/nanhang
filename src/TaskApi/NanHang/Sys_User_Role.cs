namespace TaskApi.NanHang
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class Sys_User_Role
    {
        [StringLength(50)]
        public string F_User { get; set; }

        [StringLength(50)]
        public string F_Role { get; set; }

        [Key]
        [StringLength(50)]
        public string F_Id { get; set; }

        public int? F_SortCode { get; set; }

        [StringLength(50)]
        public string F_DepartmentId { get; set; }

        public bool? F_DeleteMark { get; set; }

        public bool? F_EnabledMark { get; set; }

        public DateTime? F_CreatorTime { get; set; }

        [StringLength(50)]
        public string F_CreatorUserId { get; set; }

        public DateTime? F_LastModifyTime { get; set; }

        [StringLength(50)]
        public string F_LastModifyUserId { get; set; }

        public DateTime? F_DeleteTime { get; set; }

        [StringLength(50)]
        public string F_DeleteUserId { get; set; }

        public Guid msrepl_tran_version { get; set; }
    }
}
