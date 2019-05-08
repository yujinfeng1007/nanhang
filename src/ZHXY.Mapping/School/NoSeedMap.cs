using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class NoSeedMap : EntityTypeConfiguration<NoSeed>
    {
        public NoSeedMap()
        {
            ToTable("School_No_Seed");
            HasKey(t => t.F_Id);
        }
    }
}