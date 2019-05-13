using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Domain;
using ZHXY.Common;

namespace ZHXY.Web.Dorm.Controllers
{
    public class InOutController : ZhxyWebControllerBase
    {

        private LateReturnReportService LateReturnReportApp { get; }
        private OriginalReportService OriginalReportApp { get; }

        private NoReturnReportService NoReturnReportApp { get; }

        private NoOutReportService NoOutReportApp { get; }
        public InOutController(LateReturnReportService app_1, OriginalReportService app_2, NoReturnReportService app_3 , NoOutReportService app_4)
        {
            LateReturnReportApp = app_1;
            OriginalReportApp = app_2;
            NoReturnReportApp = app_3;
            NoOutReportApp = app_4;
        }
        [HttpGet]
        public ActionResult GetOriginalListBydate(GetOriginalListBydateParms parms)
        {
            if (string.IsNullOrEmpty(parms.date)) return Error("请输入日期");
            var pagination = new Pagination() { Page = parms.PageIndex, Rows = parms.PageSize };
            var list = OriginalReportApp.GetOriginalListBydate(pagination, parms.userId, parms.date);
            return PagingResult(list, pagination);
        }
        /// <summary>
        /// 获取学生出入记录
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDormRecords(string studentId, string date)
        {
            if (string.IsNullOrEmpty(date)) return Error("请输入日期");
            if (string.IsNullOrEmpty(studentId)) return Error("请输入日期");
            var student = new StudentAppService().GetOrDefault(studentId);
            if (student == null) return Error("未找到学生");
            var classInfo = new OrgService().GetById(student.F_Class_ID);
            var list = OriginalReportApp.GetOriginalListBydate(studentId, date);
            var data = new
            {
                name = student.F_Name,
                classname = classInfo.F_FullName,
                records = list.Select(p => new { p.InOut, p.Date, p.ChannelName })
            };
            return Result(data);

        }
        /// <summary>
        /// 获取班级下晚归统计
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetLateListByClass(string classId, string startTime, string endTime)
        {
            var list = LateReturnReportApp.GetListByClass(classId, startTime, endTime).Select(p =>
             new
             {
                 name = p.F_Name,
                 className = p.Class?.Name,
                 address = p.Dorm?.Area + p.Dorm?.UnitNumber+ p.Dorm?.BuildingId + p.Dorm?.FloorNumber+p.Dorm?.Title,
                 record = p.F_InTime
             });
            return Result(list);
        }
        /// <summary>
        /// 获取晚归记录
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetLateList(string startTime, string endTime)
        {
            var list = LateReturnReportApp.GetListByClass(null, startTime, endTime).Select(p =>
                new
                {
                    inTime = p.F_InTime,
                    count = p.F_Time
                });
            return Result(list);
        }
        /// <summary>
        /// 获取未归记录
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetNoReturnList(string startTime, string endTime)
        {
            var list = NoReturnReportApp.GetList(startTime, endTime).Select(p =>
               new
               {
                   outTime = p.F_OutTime,
                   count = p.F_DayCount
               });
            return Result(list);
        }
        /// <summary>
        /// 获取未出记录
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetNoOutList(string startTime, string endTime)
        {
            var list = NoOutReportApp.GetList(startTime, endTime).Select(p =>
               new
               {
                   inTime = p.F_InTime,
                   count = p.F_Time
               });
            return Result(list);
        }
        /// <summary>
        /// 获取未归统计
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="keyboard"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetNoReturnListByClass(string classId, string keyboard, string startTime, string endTime)
        {
            var list = NoReturnReportApp.GetList(classId, keyboard, startTime, endTime).Select(p =>
              new
              {
                  name = p.F_Name,
                  className = p.Class.Name,
                  address = p.Dorm?.Area + p.Dorm?.UnitNumber + p.Dorm?.BuildingId + p.Dorm?.FloorNumber + p.Dorm?.Title,
                  date = p.F_OutTime,
                  count = p.F_DayCount
              });
            return Result(list);
        }
        /// <summary>
        /// 获取未出统计
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="keyboard"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public ActionResult GetNoOutListByClass(string classId, string keyboard, string startTime, string endTime)
        {
            var list = NoOutReportApp.GetList(classId, keyboard, startTime, endTime).Select(p =>
              new
              {
                  name = p.F_Name,
                  className = p.Class.Name,
                  address = p.Dorm?.Area + p.Dorm?.UnitNumber + p.Dorm?.BuildingId + p.Dorm?.FloorNumber + p.Dorm?.Title,
                  date = p.F_InTime,
                  count = p.F_Time
              });
            return Result(list);
        }
        /// <summary>
        /// 获取学部下晚归统计
        /// 问题：底层的query方法不能懒加载出晚归报表对象的Class导航属性
        /// </summary>
        /// <param name="divisId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetLateListByDivis(string divisId, string startTime, string endTime)
        {
            var objlist = new List<object>();

            var sysApp = new OrgService();
            var classList = new List<Organ>();
            var name = sysApp.GetClassInfosByDivisId(divisId,ref classList);
            var list = LateReturnReportApp.GetListByClassList(classList.Select(p=>p.Id).ToList(), startTime, endTime);
            var group = list.GroupBy(p => p.F_Class);
            foreach (var item in group)
            {
                var obj = new
                {
                    classId=item.Key,
                    className = classList.FirstOrDefault(p => p.Id.Equals(item.Key)).Name,
                    count = item.Count()
                };
                objlist.Add(obj);
            }
            return Result(objlist);
        }
        [HttpGet]
        public ActionResult GetLateListByGrade(string gradeId, string startTime, string endTime)
        {
            var objlist = new List<object>();
            var sysApp = new OrgService();
            var classList = new List<Organ>();
            var divisList = new List<Organ>();
            sysApp.GetClassInfosByGradeId(gradeId, ref classList,ref divisList);
            var data = LateReturnReportApp.GetListByDivisList(divisList,classList, startTime, endTime);
            return Result(data);
        }
    }
    public class GetOriginalListBydateParms
    {
        /// <summary>
        /// 页数
        /// </summary>
        public int PageIndex { get; set; } = 1;
        /// <summary>
        /// 页面行数
        /// </summary>
        public int PageSize { get; set; } = 15;
        /// <summary>
        /// 日期
        /// </summary>
        public string date { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string userId { get; set; }
    }
}