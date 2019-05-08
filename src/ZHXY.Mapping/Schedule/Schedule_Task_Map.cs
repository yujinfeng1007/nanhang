/*
***************************************************************
* 公司名称    :杭州华网
* 系统名称    :ben
* 类 名 称    :Schedule_TaskMap
* 功能描述    :
* 业务描述    :
* 作 者 名    :易本辰
* 开发日期    :2018-12-12 09:24:58
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
    public class Schedule_Task_Map : EntityTypeConfiguration<Schedule_Task_Entity>
    {
        public Schedule_Task_Map()
        {
            ToTable("Schedule_Task");
            HasKey(t => t.F_Id);
        }
    }
}