using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class PresenceMap : EntityTypeConfiguration<SchPresence>
    {
        public PresenceMap()
        {
            ToTable("School_Presence_Info");
            HasKey(t => t.F_Id);
        }
    }
}