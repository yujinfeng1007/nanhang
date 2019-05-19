using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain.EntityConfig.Dorm
{
    public class LDJCLSMap : EntityTypeConfiguration<LDJCLS>
    {
        public LDJCLSMap()
        {
            ToTable("zhxy_ldjcls");

            HasKey(p => p.FlowId);

            Property(p => p.FlowId).HasColumnName("flow_id");
            Property(p => p.BuildingNo).HasColumnName("building_no");
            Property(p => p.UserId).HasColumnName("user_id");
            Property(p => p.Direction).HasColumnName("direction");
            Property(p => p.OccurrenceTime).HasColumnName("occurrence_time");
            Property(p => p.IsAbnormal).HasColumnName("is_abnormal");
        }
    }
}
