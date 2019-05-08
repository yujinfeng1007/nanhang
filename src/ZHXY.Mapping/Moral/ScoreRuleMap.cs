using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class ScoreRuleMap : EntityTypeConfiguration<EvaluationRule>
    {
        public ScoreRuleMap()
        {
            ToTable("Moral_Score_Rule");

            HasKey(t => t.F_Id);
        }
    }
}