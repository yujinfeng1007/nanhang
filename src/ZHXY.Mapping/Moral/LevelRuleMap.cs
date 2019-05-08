using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class LevelRuleMap : EntityTypeConfiguration<LevelRule>
    {
        public LevelRuleMap()
        {
            ToTable("Moral_Level_Rule");
            HasKey(t => t.F_Id);
        }
    }
}