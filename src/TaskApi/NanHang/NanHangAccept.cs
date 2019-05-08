namespace TaskApi.NanHang
{
    using System.Data.Entity;

    public partial class NanHangAccept : DbContext
    {
        public NanHangAccept()
            : base("name=NanHangAccept")
        {
        }

        public virtual DbSet<Dorm_DormStudent> Dorm_DormStudent { get; set; }
        public virtual DbSet<School_Students> School_Students { get; set; }
        public virtual DbSet<School_Teachers> School_Teachers { get; set; }
        public virtual DbSet<Sys_Organize> Sys_Organize { get; set; }
        public virtual DbSet<Sys_User> Sys_User { get; set; }
        public virtual DbSet<Sys_User_Role> Sys_User_Role { get; set; }
        public virtual DbSet<Sys_UserLogOn> Sys_UserLogOn { get; set; }
        public virtual DbSet<Dorm_DormInfo> Dorm_DormInfo { get; set; }

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

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_Id)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_User_ID)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_Divis_ID)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_Name)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_Name_Old)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_Gender)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_Num)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_Nation)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_FacePhoto)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_CredType)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_CredNum)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_CredPhoto_Obve)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_CredPhoto_Rever)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_Native)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_RegAddr)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_Volk)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_PolitStatu)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_Marrige)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_Health)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_InWork_Date)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_Source_Teacher)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_Type_Teacher)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_Payroll)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_YRXS)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_If_Contract)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_Profession)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_Academy)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_Education)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_Duties)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_Titles)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_TSJYCYZS)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_XXJSYY)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_GFSFS)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_JCFW)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_JCFW_Start)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_JCFW_End)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_If_Sp)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_If_Gg)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_Status)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_DepartmentId)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_CreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_LastModifyUserId)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_DeleteUserId)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_Duty)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_RoleId)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_MobilePhone)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_Introduction)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_Email)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_GL)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_XL)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_DYXL)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_DYXLXW)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_DYBYYX)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_DYZY)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_ZGXW)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_ZGSJ)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_XNDJ)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_SFZGZ)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_ZGZBH)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_ZGZMC)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_XJ)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_HTN)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_SFZZ)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_SBQK)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_GSQK)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_GJJQK)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_JJUser)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_JJPhone)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_LZSJ)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_School)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_NL)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_ZGZY)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_Ktype)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_Kstatu)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_Identity)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_Type_Tea)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_YPNo)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_YPPwd)
                .IsUnicode(false);

            modelBuilder.Entity<School_Teachers>()
                .Property(e => e.F_Mac_No)
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

            modelBuilder.Entity<Sys_User_Role>()
                .Property(e => e.F_User)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User_Role>()
                .Property(e => e.F_Role)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User_Role>()
                .Property(e => e.F_Id)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User_Role>()
                .Property(e => e.F_DepartmentId)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User_Role>()
                .Property(e => e.F_CreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User_Role>()
                .Property(e => e.F_LastModifyUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_User_Role>()
                .Property(e => e.F_DeleteUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_UserLogOn>()
                .Property(e => e.F_Id)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_UserLogOn>()
                .Property(e => e.F_UserId)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_UserLogOn>()
                .Property(e => e.F_UserPassword)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_UserLogOn>()
                .Property(e => e.F_UserSecretkey)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_UserLogOn>()
                .Property(e => e.F_Question)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_UserLogOn>()
                .Property(e => e.F_AnswerQuestion)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_UserLogOn>()
                .Property(e => e.F_Language)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_UserLogOn>()
                .Property(e => e.F_Theme)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_UserLogOn>()
                .Property(e => e.F_DepartmentId)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_DormInfo>()
                .Property(e => e.F_Id)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_DormInfo>()
                .Property(e => e.F_DepartmentId)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_DormInfo>()
                .Property(e => e.F_CreatorUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_DormInfo>()
                .Property(e => e.F_LastModifyUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_DormInfo>()
                .Property(e => e.F_Memo)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_DormInfo>()
                .Property(e => e.F_Area)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_DormInfo>()
                .Property(e => e.F_Building_No)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_DormInfo>()
                .Property(e => e.F_Floor_No)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_DormInfo>()
                .Property(e => e.F_Unit_No)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_DormInfo>()
                .Property(e => e.F_Classroom_Type)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_DormInfo>()
                .Property(e => e.F_Classroom_No)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_DormInfo>()
                .Property(e => e.F_Sex)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_DormInfo>()
                .Property(e => e.F_Classroom_Status)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_DormInfo>()
                .Property(e => e.F_DeleteUserId)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_DormInfo>()
                .Property(e => e.F_Title)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_DormInfo>()
                .Property(e => e.F_Leader_ID)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_DormInfo>()
                .Property(e => e.F_Leader_Name)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_DormInfo>()
                .Property(e => e.F_Manager_ID)
                .IsUnicode(false);

            modelBuilder.Entity<Dorm_DormInfo>()
                .Property(e => e.F_Manager_Name)
                .IsUnicode(false);
        }
    }
}
