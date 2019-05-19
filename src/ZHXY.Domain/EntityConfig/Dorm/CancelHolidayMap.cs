using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class CancelHolidayMap : EntityTypeConfiguration<CancelHoliday>
    {
        public CancelHolidayMap()
        {
            ToTable("zhxy_cancel_holiday");

            HasKey(p => new { p.OrderId, p.OperatorId });

            Property(p => p.OrderId).HasColumnName("order_id");
            Property(p => p.OperatorId).HasColumnName("op_user_id");
            Property(p => p.OperationTime).HasColumnName("op_time");
            Property(p => p.Days).HasColumnName("days");
        }
    }
}
