using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ZHXY.Application
{
    public class ReportHelper
    {
        public string CreateSql(string table, string classId, string startTime, string endTime, ref IDictionary<string, string> parms)
        {
            var sql = new StringBuilder();
            sql.Append(" select a.F_Account as '学号',a.F_Name as '姓名',b.F_Name as '班级',a.F_College as '院校',d.F_Title as '宿舍号',a.F_InTime as '进宿舍时间',a.F_OutTime as '出宿舍时间'");
            sql.Append(" from " + table + " as a");
            sql.Append(",School_Class_Info as b,Dorm_Dorm as d");
            sql.Append(" where");
            sql.Append(" a.F_Class = b.F_Id and a.F_Dorm = d.F_Id");

            if (!string.IsNullOrEmpty(classId))
            {
                parms.Add("ClassId", classId);
                sql.Append(" and a.F_Class=@ClassId");
            }
            if (!string.IsNullOrEmpty(startTime))
            {
                parms.Add("StartTime", startTime + " 00:00:00");
                sql.Append(" and a.F_CreatorTime>=@StartTime");
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                parms.Add("EndTime", endTime + " 23:59:59");
                sql.Append(" and a.F_CreatorTime<= @EndTime");
            }
            return sql.ToString();
        }
        public void WriteXml(string cron, string xmlPath)
        {
            var document = new XmlDocument();
            try
            {
                document.Load(xmlPath);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "请修改system配置文件重新配置绝对路径");
            }
            var xe = document.DocumentElement;
            var xe1 = (XmlElement)xe.GetElementsByTagName("trigger").Item(3);
            var xe2 = (XmlElement)xe1.GetElementsByTagName("name").Item(0);
            if (!xe2.InnerText.Equals("ReportJobTrigger")) throw new Exception("ReportJobTrigger位置错误,请更改位置后重试");
            var xe3 = (XmlElement)xe1.GetElementsByTagName("cron-expression").Item(0);
            if (!xe3.InnerText.Equals(cron))
            {
                xe3.InnerText = cron;
                document.Save(xmlPath);
            }
        }
    }
}
