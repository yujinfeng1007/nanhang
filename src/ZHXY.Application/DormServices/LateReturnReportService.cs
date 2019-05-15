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

        //根据学生ID获取晚归记录
        public List<LateReturnReport> GetLateListByStuId(string studentId, string startTime, string endTime)
        {
            var expression = ExtLinq.True<LateReturnReport>();
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
        public List<object> GetListByDivisList(List<Organ> divisList,List<Organ> classList, string startTime, string endTime)
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
            var resObjList = new List<object>();
            foreach (var item in divisList)
            {
                var classIds= classList.Where(p => p.ParentId.Equals(item.Id)).Select(p=>p.Id).ToList();
                var stuLateList= lateReturnlist.Where(p => classIds.Contains(p.F_Class));
                if (stuLateList.Count() < 1) continue;
                var group = stuLateList.GroupBy(p => p.F_Class);
                var classObjList = new List<object>(); ;
                foreach (var g in group)
                {

                    var classObj = new
                    {
                        classId = g.Key,
                        className = classList.FirstOrDefault(p => p.Id.Equals(g.Key)).Name,
                        count = g.Count()
                    };
                    classObjList.Add(classObj);
                }
                object divisObj = new
                {
                    divisId = item.Id,
                    divisName = item.Name,
                    list = classObjList
                };
                resObjList.Add(divisObj);
            }
            return resObjList;
        }
    }
}
