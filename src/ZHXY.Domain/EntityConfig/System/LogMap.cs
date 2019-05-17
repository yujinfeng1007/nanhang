using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class LogMap : EntityTypeConfiguration<SysLog>
    {
        public LogMap()
        {
            ToTable("zhxy_op_log");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.CreateTime).HasColumnName("op_time");
            Property(p => p.Account).HasColumnName("op_user_account");
            Property(p => p.UserId).HasColumnName("op_user_id").IsRequired();
            Property(p => p.NickName).HasColumnName("op_username");
            Property(p => p.Type).HasColumnName("op_type");
            Property(p => p.IPAddress).HasColumnName("from_ip");
            Property(p => p.IPAddressName).HasColumnName("from_location");
            Property(p => p.ModuleId).HasColumnName("module_id");
            Property(p => p.ModuleName).HasColumnName("module_name");
            Property(p => p.Result).HasColumnName("op_result");
            Property(p => p.Description).HasColumnName("description");
        }
    }
}