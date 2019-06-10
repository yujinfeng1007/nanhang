using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class LeaveSuspendMap : EntityTypeConfiguration<LeaveSuspend>
    {
        public LeaveSuspendMap()
        {
            ToTable("zhxy_leave_suspend");
            HasKey(p => p.OrderId);

            Property(p => p.OrderId).HasColumnName("order_id");
            Property(p => p.Days).HasColumnName("days");
            Property(p => p.Time).HasColumnName("time");
        }
    }
}
