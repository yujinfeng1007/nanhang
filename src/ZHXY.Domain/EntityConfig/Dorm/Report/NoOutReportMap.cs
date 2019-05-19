using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class NoOutReportMap : EntityTypeConfiguration<NoOutReport>
    {
        public NoOutReportMap()
        {
            ToTable("zhxy_no_out_report");
            HasKey(p => p.Id);

            Property(p => p.Id).HasColumnName("F_Id");
            Property(p => p.Account).HasColumnName("F_Account");
            Property(p => p.Name).HasColumnName("F_Name");
            Property(p => p.College).HasColumnName("F_College");
            Property(p => p.ClassId).HasColumnName("F_Class");
            Property(p => p.DormId).HasColumnName("F_Dorm");
            Property(p => p.StudentId).HasColumnName("F_StudentId");
            Property(p => p.CreatedTime).HasColumnName("F_CreatorTime");



            Property(p => p.Time).HasColumnName("F_Time");
            Property(p => p.InTime).HasColumnName("F_InTime");


            HasOptional(p => p.Organ).WithMany().HasForeignKey(p => p.ClassId);
            HasOptional(p => p.Dorm).WithMany().HasForeignKey(p => p.DormId);
        }
    }
}
