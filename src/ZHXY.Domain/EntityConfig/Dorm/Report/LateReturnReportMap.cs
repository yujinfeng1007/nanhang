using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class LateReturnReportMap : EntityTypeConfiguration<LateReturnReport>
    {
        public LateReturnReportMap()
        {
            ToTable("Dorm_LateReturnReport");
            HasKey(p => p.F_Id);
            // Property(p => p.F_StudentId).HasColumnName("F_StudentId");
            HasOptional(p => p.Class).WithMany().HasForeignKey(p => p.F_Class);
            HasOptional(p => p.Dorm).WithMany().HasForeignKey(p => p.F_Dorm);

        }
    }
}
