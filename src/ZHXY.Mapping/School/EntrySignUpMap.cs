using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class EntrySignUpMap : EntityTypeConfiguration<EntrySignUp>
    {
        public EntrySignUpMap()
        {
            ToTable("School_EntrySignUp");
            HasKey(t => t.F_Id);
        }
    }
}