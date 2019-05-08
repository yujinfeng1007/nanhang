using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class FlagRuleMap : EntityTypeConfiguration<MobileRedFlagRule>
    {
        public FlagRuleMap()
        {
            HasKey(p => p.Id);

            Property(p => p.Title).HasColumnName("F_Title");
            Property(p => p.Area).HasColumnName("F_Area");
            Property(p => p.Period).HasColumnName("F_Period");
            Property(p => p.DivisionId).HasColumnName("F_DivisionId");
            Property(p => p.FlagQuantity).HasColumnName("F_FlagQuantity");
            Property(p => p.IntegrationProjectId).HasColumnName("F_IntegrationProject");
            Property(p => p.AutoCalculation).HasColumnName("F_AutoCalculation");

            HasOptional(p => p.IntegrationProject).WithMany().HasForeignKey(p => p.IntegrationProjectId);
        }
    }
}