/*******************************************************************************
 * Author: mario
 * Description: School_Schedules_Time  Controller类
********************************************************************************/

using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.SchoolManage;
using NFine.Domain.Entity.SystemManage;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    //班级课程信息表
    public class School_Schedules_TimeController : ControllerBase
    {
        private School_Schedules_Time_App app = new School_Schedules_Time_App();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string F_Grande, string F_Divis, string F_Semester)
        {
            var data = new
            {
                rows = app.GetList(pagination, F_Grande, F_Divis, F_Semester),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        //[HandlerAjaxOnly]
        public ActionResult GetFormJson(string F_Semester, string F_Grande, string F_Divis, string F_School = "")
        {
            OperatorModel user = OperatorProvider.Provider.GetCurrent();
            var data = app.GetFormByOrg(F_Semester, F_Grande);
            return Content(data.ToJson());
        }

        [HttpGet]
        //[HandlerAjaxOnly]
        public ActionResult GetFormJsonKey(string keyValue)
        {
            var data = app.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(ScheduleTime entity, string keyValue)
        {
            app.SubmitForm(entity, keyValue);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            app.DeleteForm(keyValue);
            return Success("删除成功。");
        }

        [HttpGet]
        //[HandlerAjaxOnly]
        public ActionResult GetSchedulesTime(string F_Class)
        {
            //先算学期 再算学部
            Organize cla = new OrganizeApp().GetForm(F_Class);
            if (cla != null)
            {
                string gradeId = cla.F_ParentId;
                Semester se = new School_Semester_App().GetCurrentSemester();
                if (se != null)
                {
                    List<ScheduleTime> data = app.GetList(t => t.F_Grande == gradeId && t.F_Semester == se.F_Id);
                    if (data.Count() == 1)
                    {
                        object re = new object();
                        List<object> AM = new List<object>();
                        List<object> MN = new List<object>();
                        List<object> CM = new List<object>();
                        List<object> PM = new List<object>();
                        List<object> EN = new List<object>();
                        Dictionary<string, List<object>> dic = new Dictionary<string, List<object>>();
                        dic.Add("AM", AM); dic.Add("MN", MN); dic.Add("CM", CM); dic.Add("PM", PM); dic.Add("EN", EN);
                        ScheduleTime times = data.First();

                        if (times.F_TimeSpan1 != null)
                        {
                            object one = new
                            {
                                ClassHour = 1,
                                ClassHourCh = "第一节",
                                ClassTime = times.F_Course_StartTime1 + "~" + times.F_Course_EndTime1
                            };
                            dic[times.F_TimeSpan1].Add(one);
                        }

                        if (times.F_TimeSpan2 != null)
                        {
                            object one = new
                            {
                                ClassHour = 2,
                                ClassHourCh = "第二节",
                                ClassTime = times.F_Course_StartTime2 + "~" + times.F_Course_EndTime2
                            };
                            dic[times.F_TimeSpan2].Add(one);
                        }

                        if (times.F_TimeSpan3 != null)
                        {
                            object one = new
                            {
                                ClassHour = 3,
                                ClassHourCh = "第三节",
                                ClassTime = times.F_Course_StartTime3 + "~" + times.F_Course_EndTime3
                            };
                            dic[times.F_TimeSpan3].Add(one);
                        }

                        if (times.F_TimeSpan4 != null)
                        {
                            object one = new
                            {
                                ClassHour = 4,
                                ClassHourCh = "第四节",
                                ClassTime = times.F_Course_StartTime4 + "~" + times.F_Course_EndTime4
                            };
                            dic[times.F_TimeSpan4].Add(one);
                        }

                        if (times.F_TimeSpan5 != null)
                        {
                            object one = new
                            {
                                ClassHour = 5,
                                ClassHourCh = "第五节",
                                ClassTime = times.F_Course_StartTime5 + "~" + times.F_Course_EndTime5
                            };
                            dic[times.F_TimeSpan5].Add(one);
                        }

                        if (times.F_TimeSpan6 != null)
                        {
                            object one = new
                            {
                                ClassHour = 6,
                                ClassHourCh = "第六节",
                                ClassTime = times.F_Course_StartTime6 + "~" + times.F_Course_EndTime6
                            };
                            dic[times.F_TimeSpan6].Add(one);
                        }

                        if (times.F_TimeSpan7 != null)
                        {
                            object one = new
                            {
                                ClassHour = 7,
                                ClassHourCh = "第七节",
                                ClassTime = times.F_Course_StartTime7 + "~" + times.F_Course_EndTime7
                            };
                            dic[times.F_TimeSpan7].Add(one);
                        }

                        if (times.F_TimeSpan8 != null)
                        {
                            object one = new
                            {
                                ClassHour = 8,
                                ClassHourCh = "第八节",
                                ClassTime = times.F_Course_StartTime8 + "~" + times.F_Course_EndTime8
                            };
                            dic[times.F_TimeSpan8].Add(one);
                        }

                        if (times.F_TimeSpan9 != null)
                        {
                            object one = new
                            {
                                ClassHour = 9,
                                ClassHourCh = "第九节",
                                ClassTime = times.F_Course_StartTime9 + "~" + times.F_Course_EndTime9
                            };
                            dic[times.F_TimeSpan9].Add(one);
                        }

                        if (times.F_TimeSpan10 != null)
                        {
                            object one = new
                            {
                                ClassHour = 10,
                                ClassHourCh = "第十节",
                                ClassTime = times.F_Course_StartTime10 + "~" + times.F_Course_EndTime10
                            };
                            dic[times.F_TimeSpan10].Add(one);
                        }

                        if (times.F_TimeSpan11 != null)
                        {
                            object one = new
                            {
                                ClassHour = 11,
                                ClassHourCh = "第十一节",
                                ClassTime = times.F_Course_StartTime11 + "~" + times.F_Course_EndTime11
                            };
                            dic[times.F_TimeSpan11].Add(one);
                        }

                        if (times.F_TimeSpan12 != null)
                        {
                            object one = new
                            {
                                ClassHour = 12,
                                ClassHourCh = "第十二节",
                                ClassTime = times.F_Course_StartTime12 + "~" + times.F_Course_EndTime12
                            };
                            dic[times.F_TimeSpan12].Add(one);
                        }

                        return Content(dic.ToJson());
                    }
                    else
                    {
                        return Error("时间设置有误");
                    }
                }
                else
                {
                    return Error("学期设置有误");
                }
            }
            else
            {
                return Error("班级信息不存在");
            }
        }
    }
}