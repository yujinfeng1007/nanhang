using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class MsgBoxMap : EntityTypeConfiguration<MsgBox>
    {
        public MsgBoxMap()
        {
            ToTable("School_MsgBox");
            HasKey(t => t.F_Id);
            HasRequired(p => p.Accepter).WithMany().HasForeignKey(p => p.ToUser);
            HasRequired(p => p.Sender).WithMany().HasForeignKey(p => p.FromUser);
        }
    }
}