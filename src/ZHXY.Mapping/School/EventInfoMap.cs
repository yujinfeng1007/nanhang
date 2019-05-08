using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class EventInfoMap : EntityTypeConfiguration<SchActivity>
    {
        public EventInfoMap()
        {
            ToTable("School_Event_Info");
            HasKey(t => t.F_Id);
        }
    }
}