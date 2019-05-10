using ZHXY.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using ZHXY.Common;

namespace ZHXY.Application
{
    /// <summary>
    /// 晚归报表服务
    /// </summary>
    public class LateReturnReportService: AppService
    {
        public LateReturnReportService(IZhxyRepository r) : base(r) { }
        public List<LateReturnReport> GetList(Pagination pagination,string startTime, string endTime, string classId)
        {
            pagination.Sord = "desc";
            pagination.Sidx = "F_CreatorTime";
            var expression = ExtLinq.True<LateReturnReport>();
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
        public List<LateReturnReport> GetListByClass(string classId, string startTime, string endTime)
        {
            var expression = ExtLinq.True<LateReturnReport>();
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
            return Read(expression).ToList();
        }

        public List<LateReturnReport> GetListByClassList(List<string> classIds, string startTime, string endTime)
        {
            var expression = ExtLinq.True<LateReturnReport>();
                expression = expression.And(p => classIds.Contains(p.F_Class));
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
            return  Read(expression).ToList();
        }
        public List<object> GetListByDivisList(List<Organize> divisList,List<Organize> classList, string startTime, string endTime)
        {
            var expression = ExtLinq.True<LateReturnReport>();
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
            var lateReturnlist = Read(expression).ToList();
            List<object> resObjList = new List<object>();
            foreach (var item in divisList)
            {
                var classIds= classList.Where(p => p.F_ParentId.Equals(item.F_Id)).Select(p=>p.F_Id).ToList();
                var stuLateList= lateReturnlist.Where(p => classIds.Contains(p.F_Class));
                if (stuLateList.Count() < 1) continue;
                var group = stuLateList.GroupBy(p => p.F_Class);
                List<object> classObjList = new List<object>(); ;
                foreach (var g in group)
                {

                    var classObj = new
                    {
                        classId = g.Key,
                        className = classList.FirstOrDefault(p => p.F_Id.Equals(g.Key)).F_FullName,
                        count = g.Count()
                    };
                    classObjList.Add(classObj);
                }
                object divisObj = new
                {
                    divisId = item.F_Id,
                    divisName = item.F_FullName,
                    list = classObjList
                };
                resObjList.Add(divisObj);
            }
            return resObjList;
        }
    }
}
