using ZHXY.Domain;
using System;
using System.Collections.Generic;
using ZHXY.Common;
using System.Linq;

namespace ZHXY.Application
{
    /// <summary>
    /// 未出报表服务
    /// </summary>
    public class NoOutReportService: AppService
    {
        public NoOutReportService(IZhxyRepository r) : base(r) { }
        public List<NoOutReport> GetList(Pagination pagination, string startTime, string endTime, string classId)
        {
            pagination.Sord = "desc";
            pagination.Sidx = "F_CreatorTime";
            var expression = ExtLinq.True<NoOutReport>();
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
            return Read(expression).Paging(pagination).ToList();
        }
        public List<NoOutReport> GetList(string startTime, string endTime)
        {

            var expression = ExtLinq.True<NoOutReport>();
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
        public List<NoOutReport> GetList(string classId,string keyboard, string startTime, string endTime )
        {

            var expression = ExtLinq.True<NoOutReport>();
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
    }
}
