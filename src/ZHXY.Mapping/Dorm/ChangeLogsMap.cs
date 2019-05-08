
using System.Data.Entity.ModelConfiguration;
using ZHXY.Domain.Entity;

namespace ZHXY.Mapping
{

    public class ChangeLogsMap: EntityTypeConfiguration< DormChangeLog>
    {
        public ChangeLogsMap()
        {
            ToTable("Dorm_ChangeLogs");
            HasKey(t => t.F_Id);
        }
    }
}