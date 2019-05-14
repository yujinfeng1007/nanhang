using ZHXY.Domain;
using System;
using System.Collections.Generic;
using ZHXY.Common;
using System.Linq;
using System.Linq.Dynamic;
using System.Data.Entity;

namespace ZHXY.Application
{
    /// <summary>
    /// 未归报表服务
    /// </summary>
    public class NoReturnReportService : AppService
    {
        public NoReturnReportService(IZhxyRepository r) : base(r) { }

        public List<NoReturnReport> GetList(Pagination pag, string startTime, string endTime, string classId)
        {
            pag.Sord = "desc";
            pag.Sidx = "F_CreatorTime";
            var expression = ExtLinq.True<NoReturnReport>();
            if (!string.IsNullOrEmpty(classId))
                expression = expression.And(p => p.F_Class.Equals(classId));
            if (!string.IsNullOrEmpty(startTime))
            {
                var start = Convert.ToDateTime(startTime + " 00:00:00");
                expression = expression.And(p => p.F_CreatorTime >= start);
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                var end = Convert.ToDateTime(endTime + " 23:59:59");
                expression = expression.And(p => p.F_CreatorTime <= end);
            }
            return Read(expression).OrderBy($"{pag.Sidx} {pag.Sord}").Skip(pag.Skip).Take(pag.Rows).ToListAsync().Result;
        }
        public List<NoReturnReport> GetList(string startTime, string endTime)
        {
            var expression = ExtLinq.True<NoReturnReport>();
            if (!string.IsNullOrEmpty(startTime))
            {
                var start = Convert.ToDateTime(startTime + " 00:00:00");
                expression = expression.And(p => p.F_CreatorTime >= start);
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                var end = Convert.ToDateTime(endTime + " 23:59:59");
                expression = expression.And(p => p.F_CreatorTime <= end);
            }
            return Read(expression).ToList();
        }
        public List<NoReturnReport> GetList(string classId, string keyboard, string startTime, string endTime)
        {
            var expression = ExtLinq.True<NoReturnReport>();
            if (!string.IsNullOrEmpty(classId))
                expression = expression.And(p => p.F_Class.Equals(classId));
            if (!string.IsNullOrEmpty(keyboard))
            {
                expression = expression.And(p => p.F_Name.Contains(keyboard));
            }
            if (!string.IsNullOrEmpty(startTime))
            {
                var start = Convert.ToDateTime(startTime + " 00:00:00");
                expression = expression.And(p => p.F_CreatorTime >= start);
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                var end = Convert.ToDateTime(endTime + " 23:59:59");
                expression = expression.And(p => p.F_CreatorTime <= end);
            }
            return Read(expression).ToList();
        }
        //根据学生ID获取未归记录
        public List<NoReturnReport> GetNoReturnListByStuId(string studentId, string startTime, string endTime)
        {
            var expression = ExtLinq.True<NoReturnReport>();
            if (!string.IsNullOrEmpty(studentId))
                expression = expression.And(p => p.F_StudentId.Equals(studentId));           
            if (!string.IsNullOrEmpty(startTime))
            {
                var start = Convert.ToDateTime(startTime + " 00:00:00");
                expression = expression.And(p => p.F_CreatorTime >= start);
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                var end = Convert.ToDateTime(endTime + " 23:59:59");
                expression = expression.And(p => p.F_CreatorTime <= end);
            }
            return Read(expression).ToList();
        }
    }
}
