using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class PersonalCommunicationMap : EntityTypeConfiguration<SchPersonalCommunication>
    {
        public PersonalCommunicationMap()
        {
            ToTable("School_PersonalCommunication");
            HasKey(t => t.F_Id);
        }
    }
}