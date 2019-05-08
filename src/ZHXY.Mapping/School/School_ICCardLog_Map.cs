/*
***************************************************************
* 公司名称    :杭州华网
* 系统名称    :ben
* 类 名 称    :School_ICCardLogMap
* 功能描述    :
* 业务描述    :
* 作 者 名    :易本辰
* 开发日期    :2019-01-02 17:11:16
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
    public class School_ICCardLog_Map : EntityTypeConfiguration<ICCardLog>
    {
        public School_ICCardLog_Map()
        {
            ToTable("School_ICCardLog");
            HasKey(t => t.F_Id);
        }
    }
}