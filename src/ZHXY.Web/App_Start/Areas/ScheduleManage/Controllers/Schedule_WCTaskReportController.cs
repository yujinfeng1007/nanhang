using NFine.Application.ScheduleManage;
using NFine.Code;
using System.Web.Mvc;

namespace NFine.Web.Areas.ScheduleManage.Controllers
{
    public class Schedule_WCTaskReportController : ControllerBase
    {
        private Schedule_WishCourseGroup_App app = new Schedule_WishCourseGroup_App();

        // 班级选科情况
        public ActionResult GetClassCourseGridJson(Pagination pagination, string F_TaskId, string F_Year, string F_Semester, string F_Divis, string F_Grade, string F_Class)
        {
            var data = app.GetClassCourseList(pagination, F_TaskId);
            return Content(data.ToJson());
        }

        /// <summary>
        /// 单科选科人数统计
        /// </summary>
        /// <returns></returns>
        public ActionResult WCTaskOneCourseReport()
        {
            return View();
        }

        public ActionResult GetOneCourseGridJson(Pagination pagination, string F_TaskId, string F_Year, string F_Semester, string F_Divis, string F_Grade, string F_Class)
        {
            var data = app.GetOneCourseList(pagination, F_TaskId);
            return Content(data.ToJson());
        }

        /// <summary>
        /// 双科选科人数统计
        /// </summary>
        /// <returns></returns>
        public ActionResult WCTaskTwoCourseReport()
        {
            return View();
        }

        public ActionResult GetTwoCourseGridJson(Pagination pagination, string F_TaskId, string F_Year, string F_Semester, string F_Divis, string F_Grade, string F_Class)
        {
            var data = app.GetTwoCourseList(pagination, F_TaskId);
            return Content(data.ToJson());
        }

        /// <summary>
        /// 三科选科人数统计
        /// </summary>
        /// <returns></returns>
        public ActionResult WCTaskThreeCourseReport()
        {
            return View();
        }

        public ActionResult GetThreeCourseGridJson(Pagination pagination, string F_TaskId, string F_Year, string F_Semester, string F_Divis, string F_Grade, string F_Class)
        {
            var data = app.GetThreeCourseList(pagination, F_TaskId);
            return Content(data.ToJson());
        }
    }
}