using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class LogMap : EntityTypeConfiguration<SysLog>
    {
        public LogMap()
        {
            ToTable("Sys_Log");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("F_Id");
            Property(p => p.CreateTime).HasColumnName("F_Date");
            Property(p => p.Account).HasColumnName("F_Account");
            Property(p => p.UserId).HasColumnName("F_CreatorUserId").IsRequired();
            Property(p => p.NickName).HasColumnName("F_NickName");
            Property(p => p.Type).HasColumnName("F_Type");
            Property(p => p.IPAddress).HasColumnName("F_IPAddress");
            Property(p => p.IPAddressName).HasColumnName("F_IPAddressName");
            Property(p => p.ModuleId).HasColumnName("F_ModuleId");
            Property(p => p.ModuleName).HasColumnName("F_ModuleName");
            Property(p => p.Result).HasColumnName("F_Result");
            Property(p => p.Description).HasColumnName("F_Description");
        }
    }
}