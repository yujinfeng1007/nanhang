using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class LateReturnReportMap : EntityTypeConfiguration<LateReturnReport>
    {
        public LateReturnReportMap()
        {
            ToTable("zhxy_late_return_report");
            HasKey(p => p.Id);

            Property(p => p.Id).HasColumnName("F_Id");
            Property(p => p.Account).HasColumnName("F_Account");
            Property(p => p.Name).HasColumnName("F_Name");
            Property(p => p.College).HasColumnName("F_College");
            Property(p => p.Class).HasColumnName("F_Class");
            Property(p => p.DormId).HasColumnName("F_Dorm");
            Property(p => p.StudentId).HasColumnName("F_StudentId");
            Property(p => p.CreatedTime).HasColumnName("F_CreatorTime");

            Property(p => p.InTime).HasColumnName("F_InTime");
            Property(p => p.F_Time).HasColumnName("F_Time");


            HasOptional(p => p.Class).WithMany().HasForeignKey(p => p.Class);
            HasOptional(p => p.DormId).WithMany().HasForeignKey(p => p.DormId);

        }
    }
}
