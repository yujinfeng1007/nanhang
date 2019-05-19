using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class DormVisitLimitMap : EntityTypeConfiguration<DormVisitLimit>
    {
        public DormVisitLimitMap()
        {
            ToTable("zhxy_dorm_visit_limit");
            HasKey(t => t.StudentId);

            Property(p => p.StudentId).HasColumnName("student_id");
            Property(p => p.TotalLimit).HasColumnName("total_limit");
            Property(p => p.UsableLimit).HasColumnName("usable_limit");
            Property(p => p.IsAutoSet).HasColumnName("is_auto_set");
            Property(p => p.CreatedTime).HasColumnName("created_time");
            Property(p => p.UpdateTime).HasColumnName("update_time");
            Property(p => p.Enabled).HasColumnName("enabled");
        }
    }
}