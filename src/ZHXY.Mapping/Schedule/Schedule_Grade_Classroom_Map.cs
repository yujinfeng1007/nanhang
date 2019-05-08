/*
***************************************************************
* 公司名称    :杭州华网
* 系统名称    :ben
* 类 名 称    :Schedule_Grade_ClassroomMap
* 功能描述    :
* 业务描述    :
* 作 者 名    :易本辰
* 开发日期    :2019-01-02 11:41:49
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
    public class Schedule_Grade_Classroom_Map : EntityTypeConfiguration<Schedule_Grade_Classroom_Entity>
    {
        public Schedule_Grade_Classroom_Map()
        {
            ToTable("Schedule_Grade_Classroom");
            HasKey(t => t.F_Id);

            HasOptional(t => t.School_Classroom_Entity)
              .WithMany()
              .HasForeignKey(t => t.F_ClassroomId)
              .WillCascadeOnDelete(false);
        }
    }
}