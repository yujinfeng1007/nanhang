namespace TaskApi.NanHang
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class School_Teachers
    {
        [Key]
        [StringLength(50)]
        public string F_Id { get; set; }

        [StringLength(50)]
        public string F_User_ID { get; set; }

        [StringLength(50)]
        public string F_Divis_ID { get; set; }

        [StringLength(50)]
        public string F_Name { get; set; }

        [StringLength(50)]
        public string F_Name_Old { get; set; }

        [StringLength(50)]
        public string F_Gender { get; set; }

        [StringLength(50)]
        public string F_Num { get; set; }

        [StringLength(50)]
        public string F_Nation { get; set; }

        [StringLength(500)]
        public string F_FacePhoto { get; set; }

        [StringLength(50)]
        public string F_CredType { get; set; }

        [StringLength(50)]
        public string F_CredNum { get; set; }

        [StringLength(500)]
        public string F_CredPhoto_Obve { get; set; }

        [StringLength(500)]
        public string F_CredPhoto_Rever { get; set; }

        public DateTime? F_Birthday { get; set; }

        [StringLength(50)]
        public string F_Native { get; set; }

        [StringLength(500)]
        public string F_RegAddr { get; set; }

        [StringLength(50)]
        public string F_Volk { get; set; }

        [StringLength(50)]
        public string F_PolitStatu { get; set; }

        [StringLength(50)]
        public string F_Marrige { get; set; }

        [StringLength(50)]
        public string F_Health { get; set; }

        [StringLength(50)]
        public string F_InWork_Date { get; set; }

        public DateTime? F_EntryTime { get; set; }

        [StringLength(50)]
        public string F_Source_Teacher { get; set; }

        [StringLength(50)]
        public string F_Type_Teacher { get; set; }

        [StringLength(50)]
        public string F_Payroll { get; set; }

        [StringLength(50)]
        public string F_YRXS { get; set; }

        [StringLength(50)]
        public string F_If_Contract { get; set; }

        public string F_Profession { get; set; }

        [StringLength(50)]
        public string F_Academy { get; set; }

        [StringLength(50)]
        public string F_Education { get; set; }

        [StringLength(50)]
        public string F_Duties { get; set; }

        [StringLength(50)]
        public string F_Titles { get; set; }

        [StringLength(50)]
        public string F_TSJYCYZS { get; set; }

        [StringLength(50)]
        public string F_XXJSYY { get; set; }

        [StringLength(50)]
        public string F_GFSFS { get; set; }

        [StringLength(50)]
        public string F_JCFW { get; set; }

        [StringLength(50)]
        public string F_JCFW_Start { get; set; }

        [StringLength(50)]
        public string F_JCFW_End { get; set; }

        [StringLength(50)]
        public string F_If_Sp { get; set; }

        [StringLength(50)]
        public string F_If_Gg { get; set; }

        [StringLength(50)]
        public string F_Status { get; set; }

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

        [StringLength(50)]
        public string F_Duty { get; set; }

        [StringLength(500)]
        public string F_RoleId { get; set; }

        [StringLength(50)]
        public string F_MobilePhone { get; set; }

        public Guid msrepl_tran_version { get; set; }

        public string F_Introduction { get; set; }

        [StringLength(50)]
        public string F_Email { get; set; }

        public DateTime? F_Duties_RMSJ { get; set; }

        [StringLength(50)]
        public string F_GL { get; set; }

        [StringLength(50)]
        public string F_XL { get; set; }

        [StringLength(50)]
        public string F_DYXL { get; set; }

        [StringLength(50)]
        public string F_DYXLXW { get; set; }

        [StringLength(50)]
        public string F_DYBYYX { get; set; }

        [StringLength(50)]
        public string F_DYZY { get; set; }

        [StringLength(50)]
        public string F_ZGXW { get; set; }

        [StringLength(50)]
        public string F_ZGSJ { get; set; }

        public DateTime? F_ZCSJ { get; set; }

        [StringLength(50)]
        public string F_XNDJ { get; set; }

        public DateTime? F_XNDJSJ { get; set; }

        [StringLength(50)]
        public string F_SFZGZ { get; set; }

        [StringLength(50)]
        public string F_ZGZBH { get; set; }

        [StringLength(50)]
        public string F_ZGZMC { get; set; }

        [StringLength(50)]
        public string F_XJ { get; set; }

        public DateTime? F_SQSJ { get; set; }

        public DateTime? F_HTQDR { get; set; }

        public DateTime? F_HTDQR { get; set; }

        [StringLength(50)]
        public string F_HTN { get; set; }

        [StringLength(50)]
        public string F_SFZZ { get; set; }

        [StringLength(50)]
        public string F_SBQK { get; set; }

        [StringLength(50)]
        public string F_GSQK { get; set; }

        [StringLength(50)]
        public string F_GJJQK { get; set; }

        [StringLength(50)]
        public string F_JJUser { get; set; }

        [StringLength(50)]
        public string F_JJPhone { get; set; }

        [StringLength(50)]
        public string F_LZSJ { get; set; }

        [StringLength(50)]
        public string F_School { get; set; }

        [StringLength(50)]
        public string F_NL { get; set; }

        [StringLength(50)]
        public string F_ZGZY { get; set; }

        [StringLength(50)]
        public string F_Ktype { get; set; }

        [StringLength(50)]
        public string F_Kstatu { get; set; }

        [StringLength(50)]
        public string F_Identity { get; set; }

        [StringLength(50)]
        public string F_Type_Tea { get; set; }

        [StringLength(500)]
        public string F_YPNo { get; set; }

        [StringLength(50)]
        public string F_YPPwd { get; set; }

        [StringLength(50)]
        public string F_Mac_No { get; set; }
    }
}
