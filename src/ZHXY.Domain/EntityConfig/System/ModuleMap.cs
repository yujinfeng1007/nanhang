﻿
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class ModuleMap : EntityTypeConfiguration<Module>
    {
        public ModuleMap()
        {
            ToTable("zhxy_module");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.ParentId).HasColumnName("p_id");
            Property(p => p.Code).HasColumnName("code");
            Property(p => p.Name).HasColumnName("name");
            Property(p => p.Icon).HasColumnName("icon");
            Property(p => p.Url).HasColumnName("url");
            Property(p => p.Target).HasColumnName("target");
            Property(p => p.IsMenu).HasColumnName("is_menu");
            Property(p => p.IsExpand).HasColumnName("is_expand");
            Property(p => p.Sort).HasColumnName("sort");
            Property(p => p.Enabled).HasColumnName("enabled");
            Property(p => p.BelongSys).HasColumnName("belong_sys");
        }
    }
}