using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class SubjectArealimitMap : EntityTypeConfiguration<SubjectArealimit>
    {
        public SubjectArealimitMap()
        {
            ToTable("School_SubjectArealimit");
            HasKey(t => t.F_Id);
        }
    }
}