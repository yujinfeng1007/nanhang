namespace TaskApi.NanHang
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class School_Students
    {
        [Key]
        [StringLength(50)]
        public string F_Id { get; set; }

        [StringLength(50)]
        public string F_Year { get; set; }

        [StringLength(50)]
        public string F_Divis_ID { get; set; }

        [StringLength(50)]
        public string F_Grade_ID { get; set; }

        [StringLength(50)]
        public string F_Class_ID { get; set; }

        [StringLength(50)]
        public string F_Subjects_ID { get; set; }

        [StringLength(50)]
        public string F_StudentNum { get; set; }

        [StringLength(30)]
        public string F_NationNum { get; set; }

        public DateTime? F_InitDTM { get; set; }

        [StringLength(50)]
        public string F_Users_ID { get; set; }

        [StringLength(30)]
        public string F_InitNum { get; set; }

        [StringLength(30)]
        public string F_ICType { get; set; }

        [StringLength(30)]
        public string F_Type { get; set; }

        [StringLength(30)]
        public string F_YXT_Num { get; set; }

        [StringLength(50)]
        public string F_Mac_No { get; set; }

        [StringLength(50)]
        public string F_Mac_Addr { get; set; }

        [StringLength(50)]
        public string F_ComeFrom { get; set; }

        public int? F_RegUsers_ID { get; set; }

        [StringLength(30)]
        public string F_RegisterName { get; set; }

        [StringLength(30)]
        public string F_RegisterNum { get; set; }

        [StringLength(30)]
        public string F_SchoolType { get; set; }

        [StringLength(50)]
        public string F_CurStatu { get; set; }

        [StringLength(30)]
        public string F_CurChargeDesc { get; set; }

        [StringLength(30)]
        public string F_Father { get; set; }

        [StringLength(30)]
        public string F_FatherTel { get; set; }

        [StringLength(30)]
        public string F_Mother { get; set; }

        [StringLength(30)]
        public string F_MotherTel { get; set; }

        [StringLength(30)]
        public string F_Guarder { get; set; }

        [StringLength(30)]
        public string F_Guarder_Relation { get; set; }

        [StringLength(30)]
        public string F_Guarder_Tel { get; set; }

        [StringLength(30)]
        public string F_Guarder_Nation { get; set; }

        [StringLength(30)]
        public string F_Guarder_CredNum { get; set; }

        [StringLength(30)]
        public string F_Guarder_CredType { get; set; }

        [StringLength(100)]
        public string F_Guarder_CredPhoto_Obve { get; set; }

        [StringLength(100)]
        public string F_Guarder_CredPhoto_Rever { get; set; }

        [StringLength(50)]
        public string F_InClass { get; set; }

        [StringLength(50)]
        public string F_Stu_Type { get; set; }

        [StringLength(50)]
        public string F_ComeFrom_Type { get; set; }

        [StringLength(500)]
        public string F_Sco_Line { get; set; }

        [StringLength(500)]
        public string F_Sco_Pir { get; set; }

        [StringLength(50)]
        public string F_Sqzn { get; set; }

        public bool? F_Dzn { get; set; }

        [StringLength(500)]
        public string F_Dzn_File { get; set; }

        [StringLength(500)]
        public string F_Dzn_Memo { get; set; }

        public bool? F_Teacherzn { get; set; }

        [StringLength(500)]
        public string F_Teacherzn_File { get; set; }

        [StringLength(500)]
        public string F_Teacherzn_Memo { get; set; }

        [StringLength(50)]
        public string F_Old { get; set; }

        [StringLength(5000)]
        public string F_In_Memo { get; set; }

        [StringLength(50)]
        public string F_Name { get; set; }

        [StringLength(50)]
        public string F_Name_Old { get; set; }

        [StringLength(50)]
        public string F_Name_En { get; set; }

        [StringLength(10)]
        public string F_Gender { get; set; }

        public DateTime? F_Birthday { get; set; }

        [StringLength(50)]
        public string F_Only_One { get; set; }

        [StringLength(100)]
        public string F_FacePic_File { get; set; }

        [StringLength(20)]
        public string F_Nation { get; set; }

        [StringLength(50)]
        public string F_ZJXY { get; set; }

        [StringLength(20)]
        public string F_CredType { get; set; }

        [StringLength(50)]
        public string F_CredNum { get; set; }

        [StringLength(100)]
        public string F_CredPhoto_Obve { get; set; }

        [StringLength(100)]
        public string F_CredPhoto_Rever { get; set; }

        [StringLength(50)]
        public string F_Native { get; set; }

        [StringLength(50)]
        public string F_Native_Old { get; set; }

        [StringLength(50)]
        public string F_Cn_Level { get; set; }

        [StringLength(50)]
        public string F_HSK_TEST { get; set; }

        public DateTime? F_Stu_Time { get; set; }

        [StringLength(20)]
        public string F_Volk { get; set; }

        [StringLength(20)]
        public string F_PolitStatu { get; set; }

        [StringLength(500)]
        public string F_Home_Addr { get; set; }

        [StringLength(20)]
        public string F_Height { get; set; }

        [StringLength(20)]
        public string F_Weight { get; set; }

        [StringLength(50)]
        public string F_Blood_Type { get; set; }

        [StringLength(500)]
        public string F_Allergy { get; set; }

        [StringLength(500)]
        public string F_Food { get; set; }

        [StringLength(500)]
        public string F_MedicalHis { get; set; }

        [StringLength(500)]
        public string F_MedicalHis_Memo { get; set; }

        [StringLength(50)]
        public string F_Reg_Status { get; set; }

        [StringLength(100)]
        public string F_RegAddr { get; set; }

        [StringLength(50)]
        public string F_RegRelat { get; set; }

        [StringLength(100)]
        public string F_RegPhoto_Obve { get; set; }

        [StringLength(100)]
        public string F_RegPhoto_Rever { get; set; }

        [StringLength(20)]
        public string F_RegMainName { get; set; }

        [StringLength(50)]
        public string F_RegMain_CredNum { get; set; }

        [StringLength(100)]
        public string F_RegMain_CredPhoto_Obve { get; set; }

        [StringLength(100)]
        public string F_RegMain_CredPhoto_Rever { get; set; }

        [StringLength(100)]
        public string F_FamilyAddr { get; set; }

        [StringLength(20)]
        public string F_Tel { get; set; }

        [StringLength(500)]
        public string F_ComeFromAddress { get; set; }

        [StringLength(500)]
        public string F_Score { get; set; }

        [StringLength(500)]
        public string F_Score_File { get; set; }

        [StringLength(500)]
        public string F_ComeBackSite { get; set; }

        [StringLength(30)]
        public string F_ComeBackType { get; set; }

        [StringLength(50)]
        public string F_ComeBackRoute_ID { get; set; }

        [StringLength(50)]
        public string F_ComeBackPro { get; set; }

        [StringLength(50)]
        public string F_ComeBackArea { get; set; }

        [StringLength(50)]
        public string F_ComeBackCity { get; set; }

        [StringLength(50)]
        public string F_Relative2_Guarder_Relation { get; set; }

        [StringLength(50)]
        public string F_Relative3_Guarder_Relation { get; set; }

        [StringLength(50)]
        public string F_Relative3_Name { get; set; }

        [StringLength(20)]
        public string F_Relative3_Tel { get; set; }

        [StringLength(50)]
        public string F_Relative1_Name { get; set; }

        [StringLength(20)]
        public string F_Relative1_Tel { get; set; }

        [StringLength(50)]
        public string F_Relative1_Guarder { get; set; }

        [StringLength(50)]
        public string F_Relative1_Comp { get; set; }

        [StringLength(50)]
        public string F_Relative1_Guarder_Relation { get; set; }

        [StringLength(50)]
        public string F_Relative2_Name { get; set; }

        [StringLength(20)]
        public string F_Relative2_Tel { get; set; }

        [StringLength(50)]
        public string F_Relative2_Guarder { get; set; }

        [StringLength(50)]
        public string F_Relative2_Comp { get; set; }

        [StringLength(50)]
        public string F_Relative3_Guarder3 { get; set; }

        [StringLength(50)]
        public string F_Relative3_Comp3 { get; set; }

        [StringLength(50)]
        public string F_Guarder_Dw { get; set; }

        [StringLength(50)]
        public string F_Guarder_Wh { get; set; }

        [StringLength(200)]
        public string F_Guarder_LinkType { get; set; }

        [StringLength(50)]
        public string F_Eat { get; set; }

        [StringLength(50)]
        public string F_Relish { get; set; }

        [StringLength(50)]
        public string F_Incontinence { get; set; }

        [StringLength(50)]
        public string F_Dress { get; set; }

        [StringLength(50)]
        public string F_Sleep { get; set; }

        [StringLength(50)]
        public string F_Stripped { get; set; }

        [StringLength(50)]
        public string F_Physique { get; set; }

        [StringLength(500)]
        public string F_Life_Memo { get; set; }

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
        public string F_INYear { get; set; }

        [StringLength(50)]
        public string F_Subjects { get; set; }

        [StringLength(50)]
        public string F_ComeFrom_Province { get; set; }

        [StringLength(50)]
        public string F_ComeFrom_City { get; set; }

        [StringLength(50)]
        public string F_ComeFrom_Area { get; set; }

        [StringLength(50)]
        public string F_Health { get; set; }

        public Guid msrepl_tran_version { get; set; }

        public string F_Introduction { get; set; }

        [StringLength(50)]
        public string F_Ktype { get; set; }

        [StringLength(50)]
        public string F_Kstatu { get; set; }

        [StringLength(50)]
        public string F_Identity { get; set; }

        [StringLength(50)]
        public string F_OnlyNo { get; set; }
    }
}
