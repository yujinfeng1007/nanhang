using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class Schedule_Class_Map : EntityTypeConfiguration<Schedule_Class_Schedule_Entity>
    {
        public Schedule_Class_Map()
        {
            ToTable("Schedule_Class");
            HasKey(t => t.F_Id);
        }
    }
}