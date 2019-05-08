/*
***************************************************************
* 公司名称    :杭州华网
* 系统名称    :ben
* 类 名 称    :School_Schedules_MoveCourseMap
* 功能描述    :
* 业务描述    :
* 作 者 名    :易本辰
* 开发日期    :2018-11-29 10:22:16
***************************************************************
* 修改履历    :
* 修改编号    :
*--------------------------------------------------------------
* 修改日期    :YYYY/MM/DD
* 修 改 者    :XXXXXX
* 修改内容    :
***************************************************************
*/

using System.Data.Entity.ModelConfiguration;
using ZHXY.Domain.Entity;

namespace ZHXY.Mapping
{
    public class School_Schedules_MoveCourse_Map : EntityTypeConfiguration<SchScheduleMoveCourse>
    {
        public School_Schedules_MoveCourse_Map()
        {
            ToTable("School_Schedules_MoveCourse");
            HasKey(t => t.F_Id);
            HasRequired(t => t.Course)
                .WithMany()
                .HasForeignKey(t => t.F_Course)
                .WillCascadeOnDelete(false);
            HasRequired(t => t.Teacher)
                .WithMany()
                .HasForeignKey(t => t.F_Teacher)
                .WillCascadeOnDelete(false);
        }
    }
}