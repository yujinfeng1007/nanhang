using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class CancelHolidayMap : EntityTypeConfiguration<CancelHoliday>
    {
        public CancelHolidayMap()
        {
            ToTable("School_CancelHoliday");

            HasKey(p => new { p.OrderId, p.OperatorId });

            Property(p => p.OrderId).HasColumnName("F_LeaveId");
            Property(p => p.OperatorId).HasColumnName("F_OperatorId");
            Property(p => p.OperationTime).HasColumnName("F_OperationTime");
            Property(p => p.Days).HasColumnName("F_Days");
        }
    }
}
