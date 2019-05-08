using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class ScreenAdvertiseTypeMap : EntityTypeConfiguration<PropagandaMode>
    {
        public ScreenAdvertiseTypeMap()
        {
            ToTable("School_Screen_Advertise_Type");
            HasKey(t => t.F_Id);
        }
    }
}