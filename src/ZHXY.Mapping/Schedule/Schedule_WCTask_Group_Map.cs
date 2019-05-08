/*
***************************************************************
* 公司名称    :天亿信达
* 系统名称    :ben
* 类 名 称    :Schedule_WCTask_GroupMap
* 功能描述    :
* 业务描述    :
* 作 者 名    :易本辰
* 开发日期    :2018-09-28 17:01:38
***************************************************************
* 修改履历    :
* 修改编号    :
*--------------------------------------------------------------
* 修改日期    :YYYY/MM/DD
* 修 改 者    :XXXXXX
* 修改内容    :
***************************************************************
*/

using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class Schedule_WCTask_Group_Map : EntityTypeConfiguration<Schedule_WCTask_Group_Entity>
    {
        public Schedule_WCTask_Group_Map()
        {
            ToTable("Schedule_WCTask_Group");
            HasKey(t => t.F_Id);
            HasRequired(t => t.Schedule_CourseGroup_Entity)
            .WithMany()
            .HasForeignKey(t => t.F_CourseGroupId)
            .WillCascadeOnDelete(false);
        }
    }
}