namespace TaskApi.NanHang
{
    using System.Data.Entity;

    public partial class NHModel : DbContext
    {
        public NHModel()
            : base("name=NHModel")
        {
        }

        public virtual DbSet<OrganizationInfo> OrganizationInfoes { get; set; }
        public virtual DbSet<OrganizationInfo_stu> OrganizationInfo_stu { get; set; }
        public virtual DbSet<StudentInfo> StudentInfoes { get; set; }
        public virtual DbSet<TeacherInfo> TeacherInfoes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrganizationInfo>()
                .Property(e => e.OrgId)
                .IsUnicode(false);

            modelBuilder.Entity<OrganizationInfo>()
                .Property(e => e.OrgName)
                .IsUnicode(false);

            modelBuilder.Entity<OrganizationInfo>()
                .Property(e => e.ParentOrgId)
                .IsUnicode(false);

            modelBuilder.Entity<OrganizationInfo_stu>()
                .Property(e => e.OrgId)
                .IsUnicode(false);

            modelBuilder.Entity<OrganizationInfo_stu>()
                .Property(e => e.OrgName)
                .IsUnicode(false);

            modelBuilder.Entity<OrganizationInfo_stu>()
                .Property(e => e.ParentOrgId)
                .IsUnicode(false);

            modelBuilder.Entity<StudentInfo>()
                .Property(e => e.studentId)
                .IsUnicode(false);

            modelBuilder.Entity<StudentInfo>()
                .Property(e => e.studentName)
                .IsUnicode(false);

            modelBuilder.Entity<StudentInfo>()
                .Property(e => e.LoginId)
                .IsUnicode(false);

            modelBuilder.Entity<StudentInfo>()
                .Property(e => e.studentNo)
                .IsUnicode(false);

            modelBuilder.Entity<StudentInfo>()
                .Property(e => e.orgId)
                .IsUnicode(false);

            modelBuilder.Entity<StudentInfo>()
                .Property(e => e.orgName)
                .IsUnicode(false);

            modelBuilder.Entity<StudentInfo>()
                .Property(e => e.studentBuildingId)
                .IsUnicode(false);

            modelBuilder.Entity<StudentInfo>()
                .Property(e => e.studentSex)
                .IsUnicode(false);

            modelBuilder.Entity<StudentInfo>()
                .Property(e => e.certificateType)
                .IsUnicode(false);

            modelBuilder.Entity<StudentInfo>()
                .Property(e => e.certificateNo)
                .IsUnicode(false);

            modelBuilder.Entity<StudentInfo>()
                .Property(e => e.studentGrade)
                .IsUnicode(false);

            modelBuilder.Entity<StudentInfo>()
                .Property(e => e.studentClass)
                .IsUnicode(false);

            modelBuilder.Entity<StudentInfo>()
                .Property(e => e.studentMeto)
                .IsUnicode(false);

            modelBuilder.Entity<StudentInfo>()
                .Property(e => e.ImgUri)
                .IsUnicode(false);

            modelBuilder.Entity<StudentInfo>()
                .Property(e => e.studentPhone)
                .IsUnicode(false);

            modelBuilder.Entity<TeacherInfo>()
                .Property(e => e.teacherId)
                .IsUnicode(false);

            modelBuilder.Entity<TeacherInfo>()
                .Property(e => e.teacherName)
                .IsUnicode(false);

            modelBuilder.Entity<TeacherInfo>()
                .Property(e => e.LoginId)
                .IsUnicode(false);

            modelBuilder.Entity<TeacherInfo>()
                .Property(e => e.orgId)
                .IsUnicode(false);

            modelBuilder.Entity<TeacherInfo>()
                .Property(e => e.teacherNo)
                .IsUnicode(false);

            modelBuilder.Entity<TeacherInfo>()
                .Property(e => e.teacherPhone)
                .IsUnicode(false);

            modelBuilder.Entity<TeacherInfo>()
                .Property(e => e.certificateType)
                .IsUnicode(false);

            modelBuilder.Entity<TeacherInfo>()
                .Property(e => e.certificateNo)
                .IsUnicode(false);

            modelBuilder.Entity<TeacherInfo>()
                .Property(e => e.ImgUri)
                .IsUnicode(false);
        }
    }
}
