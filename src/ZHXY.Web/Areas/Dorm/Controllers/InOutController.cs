using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Domain;

namespace ZHXY.Web.Dorm.Controllers
{
    public class InOutController : ZhxyController
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
            if (string.IsNullOrEmpty(parms.date))  throw new System.Exception("请输入日期");
            var pagination = new Pagination() { Page = parms.PageIndex, Rows = parms.PageSize };
            var list = OriginalReportApp.GetOriginalListBydate(pagination, parms.userId, parms.date);
            return Result.PagingRst(list, pagination.Records,pagination.Total);
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
            if (string.IsNullOrEmpty(date)) throw new System.Exception("请输入日期");
            if (string.IsNullOrEmpty(studentId)) throw new System.Exception("请输入学生ID");
            var student = new StudentService().GetById(studentId);
            if (student == null) throw new System.Exception("未找到学生");
            var classInfo = new OrgService().GetById(student.ClassId);
            var list = OriginalReportApp.GetOriginalListBydate(studentId, date);
            var data = new
            {
                name = student.Name,
                classname = classInfo.Name,
                records = list.Select(p => new { p.InOut, p.Date, p.ChannelName })
            };
            return Result.Success(data);

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
                    inTime = p.InTime,
                    count = p.F_Time
                });
            return Result.Success(list);
        }

        /// <summary>
        /// 根据学生ID获取晚归记录
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetLateListByStuId(string studentId, string startTime, string endTime)
        {
            if (string.IsNullOrEmpty(studentId)) throw new System.Exception("请输入学生ID");
            var student = new StudentService().GetById(studentId);
            if (student == null) throw new System.Exception("未找到学生");
            var list = LateReturnReportApp.GetLateListByStuId(studentId, startTime, endTime).Select(p =>
                new
                {
                    inTime = p.InTime,
                    count = p.F_Time
                });
            return Result.Success(list);
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
                   outTime = p.OutTime,
                   count = p.DayCount
               });
            return Result.Success(list);
        }

        /// <summary>
        /// 根据学生ID获取未归记录
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetNoReturnListByStuId(string studentId, string startTime, string endTime)
        {
            if (string.IsNullOrEmpty(studentId)) throw new System.Exception("请输入学生ID");
            var student = new StudentService().GetById(studentId);
            if (student == null) throw new System.Exception("未找到学生");
            var list = NoReturnReportApp.GetNoReturnListByStuId(studentId,startTime, endTime).Select(p =>
               new
               {
                   outTime = p.OutTime,
                   count = p.DayCount
               });
            return Result.Success(list);
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
                   inTime = p.InTime,
                   count = p.Time
               });
            return Result.Success(list);
        }


        /// <summary>
        /// 根据学生ID获取未出记录
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetNoOutListByStuId(string studentId, string startTime, string endTime)
        {
            if (string.IsNullOrEmpty(studentId)) throw new System.Exception("请输入学生ID");
            var student = new StudentService().GetById(studentId);
            if (student == null) throw new System.Exception("未找到学生");
            var list = NoOutReportApp.GetNoOutListByStuId(studentId, startTime, endTime).Select(p =>
               new
               {
                   inTime = p.InTime,
                   count = p.Time
               });
            return Result.Success(list);
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
                 name = p.Name,
                 className = p.Organ?.Name,
                 // address = p.Dorm?.Area + p.Dorm?.UnitNumber+ p.Dorm?.BuildingId + p.Dorm?.FloorNumber+p.Dorm?.Title,
                 address = p.Dorm?.Title,
                 date = p.InTime,
                 record = p.F_Time
             });
            return Result.Success(list);
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
                  name = p.Name,
                  className = p.Organ.Name,
                  //address = p.Dorm?.Area + p.Dorm?.UnitNumber + p.Dorm?.BuildingId + p.Dorm?.FloorNumber + p.Dorm?.Title,
                  address = p.Dorm?.Title,
                  date = p.OutTime,
                  count = p.DayCount
              });
            return Result.Success(list);
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
                  name = p.Name,
                  className = p.Organ.Name,
                  //address = p.Dorm?.Area + p.Dorm?.UnitNumber + p.Dorm?.BuildingId + p.Dorm?.FloorNumber + p.Dorm?.Title,
                  address = p.Dorm?.Title,
                  date = p.InTime,
                  count = p.Time
              });
            return Result.Success(list);
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
            var classList = new List<Org>();
            var name = sysApp.GetClassInfosByDivisId(divisId,ref classList);
            var list = LateReturnReportApp.GetListByClassList(classList.Select(p=>p.Id).ToList(), startTime, endTime);
            var group = list.GroupBy(p => p.Class);
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
            return Result.Success(list);
        }
        [HttpGet]
        public ActionResult GetLateListByGrade(string gradeId, string startTime, string endTime)
        {
            var objlist = new List<object>();
            var sysApp = new OrgService();
            var classList = new List<Org>();
            var divisList = new List<Org>();
            sysApp.GetClassInfosByGradeId(gradeId, ref classList,ref divisList);
            var data = LateReturnReportApp.GetListByDivisList(divisList,classList, startTime, endTime);
            return Result.Success(data);
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