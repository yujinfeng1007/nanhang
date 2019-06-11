using ZHXY.Domain;
using System;
using System.Collections.Generic;
using ZHXY.Common;
using System.Linq;
using System.Data.Entity;
using ZHXY.Web.Shared;

namespace ZHXY.Application
{
    /// <summary>
    /// 未归报表服务
    /// </summary>
    public class NoReturnReportService : AppService
    {
        public NoReturnReportService(DbContext r) : base(r) { }

        public List<NoReturnReport> GetList(Pagination pag, string startTime, string endTime, string classId)
        {
            pag.Sord = "desc";
            pag.Sidx = "CreatedTime";
            var expression = ExtLinq.True<NoReturnReport>();
            if (!string.IsNullOrEmpty(classId))
                expression = expression.And(p => p.ClassId.Equals(classId));
            if (!string.IsNullOrEmpty(startTime))
            {
                var start = Convert.ToDateTime(startTime + " 00:00:00");
                expression = expression.And(p => p.CreatedTime >= start);
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                var end = Convert.ToDateTime(endTime + " 23:59:59");
                expression = expression.And(p => p.CreatedTime <= end);
            }
            return Read(expression).Paging(pag).ToListAsync().Result;
        }
        public List<NoReturnReport> GetList(string startTime, string endTime)
        {
            var expression = ExtLinq.True<NoReturnReport>();
            if (!string.IsNullOrEmpty(startTime))
            {
                var start = Convert.ToDateTime(startTime + " 00:00:00");
                expression = expression.And(p => p.CreatedTime >= start);
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                var end = Convert.ToDateTime(endTime + " 23:59:59");
                expression = expression.And(p => p.CreatedTime <= end);
            }
            return Read(expression).ToList();
        }
        public List<NoReturnReport> GetList(string classId, string keyboard, string startTime, string endTime)
        {
            var expression = ExtLinq.True<NoReturnReport>();
            if (!string.IsNullOrEmpty(classId))
            {
                var OrgList = new List<string> { classId };
                this.GetChildOrg(classId, OrgList);
                expression = expression.And(p => OrgList.Contains(p.ClassId));
            }
            if (!string.IsNullOrEmpty(keyboard))
            {
                expression = expression.And(p => p.Name.Contains(keyboard));
            }
            if (!string.IsNullOrEmpty(startTime))
            {
                var start = Convert.ToDateTime(startTime + " 00:00:00");
                expression = expression.And(p => p.CreatedTime >= start);
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                var end = Convert.ToDateTime(endTime + " 23:59:59");
                expression = expression.And(p => p.CreatedTime <= end);
            }
            return Read(expression).ToList();
        }
        //根据学生ID获取未归记录
        public List<NoReturnReport> GetNoReturnListByStuId(string studentId, string startTime, string endTime)
        {
            var expression = ExtLinq.True<NoReturnReport>();
            if (!string.IsNullOrEmpty(studentId))
                expression = expression.And(p => p.StudentId.Equals(studentId));           
            if (!string.IsNullOrEmpty(startTime))
            {
                var start = Convert.ToDateTime(startTime + " 00:00:00");
                expression = expression.And(p => p.CreatedTime >= start);
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                var end = Convert.ToDateTime(endTime + " 23:59:59");
                expression = expression.And(p => p.CreatedTime <= end);
            }
            return Read(expression).ToList();
        }
    }
}
