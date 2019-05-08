using System.Data.Entity.ModelConfiguration;
using ZHXY.Assists.Entity;

namespace ZHXY.Assists.Mapping
{
    public class QuestionMarkMap : EntityTypeConfiguration<QuestionMark>
    {
        public QuestionMarkMap()
        {
            ToTable("Assists_QuestionMark");

            Property(p => p.Name).HasColumnName("F_Name").HasColumnType("varchar").HasMaxLength(64).IsRequired();
        }
    }
}