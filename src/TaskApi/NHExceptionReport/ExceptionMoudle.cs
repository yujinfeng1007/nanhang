namespace TaskApi.NHExceptionReport
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ExceptionMoudle : DbContext
    {
        public ExceptionMoudle()
            : base("name=ExceptionMoudle")
        {
        }

        public virtual DbSet<Dorm_DormStudent> Dorm_DormStudent { get; set; }
        public virtual DbSet<Dorm_NoOutReport> Dorm_NoOutReport { get; set; }
        public virtual DbSet<School_Students> School_Students { get; set; }
        public virtual DbSet<Sys_Organize> Sys_Organize { get; set; }
        public virtual DbSet<Sys_User> Sys_User { get; set; }
        public virtual DbSet<Dorm_LateReturnReport> Dorm_LateReturnReport { get; set; }
        public virtual DbSet<Dorm_NoReturnReport> Dorm_NoReturnReport { get; set; }
        public virtual DbSet<sys_org_leader> sys_org_leader { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dorm_DormStudent>()
                .Property(e => e.F_Student_ID)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_DormStudent>()
                .Property(e => e.F_DormId)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_DormStudent>()
                .Property(e => e.F_Bed_ID)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_DormStudent>()
                .Property(e => e.F_Id)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_DormStudent>()
                .Property(e => e.F_DepartmentId)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_DormStudent>()
                .Property(e => e.F_Sex)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_DormStudent>()
                .Property(e => e.F_CreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_DormStudent>()
                .Property(e => e.F_LastModifyUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_DormStudent>()
                .Property(e => e.F_Memo)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_DormStudent>()
                .Property(e => e.F_DeleteUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_NoOutReport>()
                .Property(e => e.F_Id)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_NoOutReport>()
                .Property(e => e.F_CreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_NoOutReport>()
                .Property(e => e.F_DepartmentId)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_NoOutReport>()
                .Property(e => e.F_LastModifyUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_NoOutReport>()
                .Property(e => e.F_DeleteUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_NoOutReport>()
                .Property(e => e.F_Memo)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_NoOutReport>()
                .Property(e => e.F_Account)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_NoOutReport>()
                .Property(e => e.F_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_NoOutReport>()
                .Property(e => e.F_College)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_NoOutReport>()
                .Property(e => e.F_Profession)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_NoOutReport>()
                .Property(e => e.F_Class)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_NoOutReport>()
                .Property(e => e.F_Dorm)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_NoOutReport>()
                .Property(e => e.F_StudentId)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Id)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Year)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Divis_ID)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Grade_ID)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Class_ID)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Subjects_ID)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_StudentNum)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_NationNum)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Users_ID)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_InitNum)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_ICType)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Type)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_YXT_Num)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Mac_No)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Mac_Addr)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_ComeFrom)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_RegisterName)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_RegisterNum)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_SchoolType)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_CurStatu)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_CurChargeDesc)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Father)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_FatherTel)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Mother)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_MotherTel)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Guarder)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Guarder_Relation)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Guarder_Tel)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Guarder_Nation)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Guarder_CredNum)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Guarder_CredType)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Guarder_CredPhoto_Obve)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Guarder_CredPhoto_Rever)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_InClass)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Stu_Type)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_ComeFrom_Type)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Sco_Line)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Sco_Pir)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Sqzn)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Dzn_File)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Dzn_Memo)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Teacherzn_File)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Teacherzn_Memo)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Old)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_In_Memo)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Name)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Name_Old)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Name_En)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Gender)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Only_One)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_FacePic_File)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Nation)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_ZJXY)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_CredType)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_CredNum)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_CredPhoto_Obve)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_CredPhoto_Rever)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Native)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Native_Old)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Cn_Level)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_HSK_TEST)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Volk)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_PolitStatu)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Home_Addr)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Height)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Weight)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Blood_Type)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Allergy)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Food)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_MedicalHis)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_MedicalHis_Memo)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Reg_Status)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_RegAddr)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_RegRelat)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_RegPhoto_Obve)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_RegPhoto_Rever)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_RegMainName)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_RegMain_CredNum)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_RegMain_CredPhoto_Obve)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_RegMain_CredPhoto_Rever)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_FamilyAddr)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Tel)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_ComeFromAddress)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Score)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Score_File)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_ComeBackSite)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_ComeBackType)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_ComeBackRoute_ID)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_ComeBackPro)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_ComeBackArea)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_ComeBackCity)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Relative2_Guarder_Relation)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Relative3_Guarder_Relation)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Relative3_Name)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Relative3_Tel)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Relative1_Name)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Relative1_Tel)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Relative1_Guarder)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Relative1_Comp)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Relative1_Guarder_Relation)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Relative2_Name)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Relative2_Tel)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Relative2_Guarder)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Relative2_Comp)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Relative3_Guarder3)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Relative3_Comp3)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Guarder_Dw)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Guarder_Wh)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Guarder_LinkType)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Eat)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Relish)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Incontinence)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Dress)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Sleep)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Stripped)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Physique)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Life_Memo)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_DepartmentId)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_CreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_LastModifyUserId)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_DeleteUserId)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_INYear)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Subjects)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_ComeFrom_Province)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_ComeFrom_City)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_ComeFrom_Area)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Health)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Introduction)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Ktype)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Kstatu)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_Identity)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_OnlyNo)
                .IsUnicode(false);

            modelBuilder.Entity<School_Students>()
                .Property(e => e.F_InOut)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_Organize>()
                .Property(e => e.F_Id)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_Organize>()
                .Property(e => e.F_ParentId)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_Organize>()
                .Property(e => e.F_EnCode)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_Organize>()
                .Property(e => e.F_FullName)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_Organize>()
                .Property(e => e.F_ShortName)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_Organize>()
                .Property(e => e.F_CategoryId)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_Organize>()
                .Property(e => e.F_ManagerId)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_Organize>()
                .Property(e => e.F_TelePhone)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_Organize>()
                .Property(e => e.F_MobilePhone)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_Organize>()
                .Property(e => e.F_WeChat)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_Organize>()
                .Property(e => e.F_Fax)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_Organize>()
                .Property(e => e.F_Email)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_Organize>()
                .Property(e => e.F_AreaId)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_Organize>()
                .Property(e => e.F_Address)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_Organize>()
                .Property(e => e.F_Description)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_Organize>()
                .Property(e => e.F_CreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_Organize>()
                .Property(e => e.F_LastModifyUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_Organize>()
                .Property(e => e.F_DeleteUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_Organize>()
                .Property(e => e.F_Class_Type)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_Organize>()
                .Property(e => e.F_Template)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_Organize>()
                .Property(e => e.F_DepartmentId)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User>()
                .Property(e => e.F_Id)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User>()
                .Property(e => e.F_Account)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User>()
                .Property(e => e.F_RealName)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User>()
                .Property(e => e.F_NickName)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User>()
                .Property(e => e.F_HeadIcon)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User>()
                .Property(e => e.F_MobilePhone)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User>()
                .Property(e => e.F_Email)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User>()
                .Property(e => e.F_WeChat)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User>()
                .Property(e => e.F_ManagerId)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User>()
                .Property(e => e.F_Signature)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User>()
                .Property(e => e.F_OrganizeId)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User>()
                .Property(e => e.F_DepartmentId)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User>()
                .Property(e => e.F_RoleId)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User>()
                .Property(e => e.F_DutyId)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User>()
                .Property(e => e.F_Description)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User>()
                .Property(e => e.F_CreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User>()
                .Property(e => e.F_LastModifyUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User>()
                .Property(e => e.F_DeleteUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User>()
                .Property(e => e.F_Data_Type)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User>()
                .Property(e => e.F_Data_Deps)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User>()
                .Property(e => e.F_KHSF)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User>()
                .Property(e => e.F_User_SetUp)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User>()
                .Property(e => e.F_Class)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User>()
                .Property(e => e.F_File)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User>()
                .Property(e => e.EmailPassword)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_LateReturnReport>()
                .Property(e => e.F_Id)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_LateReturnReport>()
                .Property(e => e.F_CreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_LateReturnReport>()
                .Property(e => e.F_DepartmentId)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_LateReturnReport>()
                .Property(e => e.F_LastModifyUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_LateReturnReport>()
                .Property(e => e.F_DeleteUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_LateReturnReport>()
                .Property(e => e.F_Memo)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_LateReturnReport>()
                .Property(e => e.F_Account)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_LateReturnReport>()
                .Property(e => e.F_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_LateReturnReport>()
                .Property(e => e.F_College)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_LateReturnReport>()
                .Property(e => e.F_Profession)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_LateReturnReport>()
                .Property(e => e.F_Class)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_LateReturnReport>()
                .Property(e => e.F_Dorm)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_LateReturnReport>()
                .Property(e => e.F_StudentId)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_NoReturnReport>()
                .Property(e => e.F_Id)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_NoReturnReport>()
                .Property(e => e.F_CreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_NoReturnReport>()
                .Property(e => e.F_DepartmentId)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_NoReturnReport>()
                .Property(e => e.F_LastModifyUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_NoReturnReport>()
                .Property(e => e.F_DeleteUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_NoReturnReport>()
                .Property(e => e.F_Memo)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_NoReturnReport>()
                .Property(e => e.F_Account)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_NoReturnReport>()
                .Property(e => e.F_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_NoReturnReport>()
                .Property(e => e.F_College)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_NoReturnReport>()
                .Property(e => e.F_Profession)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_NoReturnReport>()
                .Property(e => e.F_Class)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_NoReturnReport>()
                .Property(e => e.F_Dorm)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_NoReturnReport>()
                .Property(e => e.F_StudentId)
                .IsUnicode(false);

            modelBuilder.Entity<sys_org_leader>()
                .Property(e => e.org_id)
                .IsUnicode(false);

            modelBuilder.Entity<sys_org_leader>()
                .Property(e => e.user_id)
                .IsUnicode(false);
        }
    }
}
