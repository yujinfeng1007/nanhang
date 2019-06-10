namespace ZHXY.Api.moudle
{
    using System.Data.Entity;

    public partial class NanHangAccept : DbContext
    {
        public NanHangAccept()
            : base("name=NanHangAccept")
        {
        }

        public virtual DbSet<Student_Organ> Student_Organ { get; set; }
        public virtual DbSet<StudentInfo> StudentInfo { get; set; }
        public virtual DbSet<Teacher_Organ> Teacher_Organ { get; set; }
        public virtual DbSet<TeacherInfo> TeacherInfo { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student_Organ>()
                .Property(e => e.OrgId)
                .IsUnicode(false);

            modelBuilder.Entity<Student_Organ>()
                .Property(e => e.OrgName)
                .IsUnicode(false);

            modelBuilder.Entity<Student_Organ>()
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

            modelBuilder.Entity<Teacher_Organ>()
                .Property(e => e.OrgId)
                .IsUnicode(false);

            modelBuilder.Entity<Teacher_Organ>()
                .Property(e => e.OrgName)
                .IsUnicode(false);

            modelBuilder.Entity<Teacher_Organ>()
                .Property(e => e.ParentOrgId)
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
