using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class PointRuleMap : EntityTypeConfiguration<PointRule>
    {
        public PointRuleMap()
        {
            ToTable("Moral_Point_Rule");
            HasKey(t => t.F_Id);
        }
    }
}