/*
***************************************************************
* 公司名称    :天亿信达
* 系统名称    :ben
* 类 名 称    :Schedule_WishCourseTaskMap
* 功能描述    :
* 业务描述    :
* 作 者 名    :易本辰
* 开发日期    :2018-09-28 17:01:39
***************************************************************
* 修改履历    :
* 修改编号    :
*--------------------------------------------------------------
* 修改日期    :YYYY/MM/DD
* 修 改 者    :XXXXXX
* 修改内容    :
***************************************************************
*/

using NFine.Application.ScheduleManage;
using NFine.Code;
using NFine.Domain.Entity.ScheduleManage;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace NFine.Web.Areas.ScheduleManage.Controllers
{
    public class Schedule_WishCourseTaskController : ControllerBase
    {
        private Schedule_WishCourseTask_App app = new Schedule_WishCourseTask_App();
        private Schedule_WCTask_Group_App groupApp = new Schedule_WCTask_Group_App();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_Year, string F_Semester, string F_Divis, string F_Grade, string F_Class, int F_Type)
        {
            var data = new
            {
                rows = app.GetList(pagination, F_Year, F_Semester, F_Divis, F_Grade, keyword, F_Type),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

        public ActionResult GetSelectJson(string F_Year, string F_Semester, string F_Divis, string F_Grade, int F_Type)
        {
            var data = app.GetList(F_Year, F_Semester, F_Divis, F_Grade, F_Type);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = app.GetForm(keyValue);
            //将用户id替换成姓名
            // var creator = new object();
            // var modifier = new object();
            // Dictionary<string, object>  dic = CacheFactory.Cache().GetCache<Dictionary<string, object>>(Cons.USERS);
            //if (data.F_CreatorUserId != null && dic.TryGetValue(data.F_CreatorUserId,out creator)) {
            //     data.F_CreatorUserId = creator.GetType().GetProperty("fullname").GetValue(creator, null).ToString();
            // }
            // if (data.F_LastModifyUserId != null &&　dic.TryGetValue(data.F_LastModifyUserId, out modifier))
            // {
            //     data.F_LastModifyUserId = modifier.GetType().GetProperty("fullname").GetValue(modifier, null).ToString();
            // }
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Schedule_WishCourseTask_Entity entity, string keyValue)
        {
            //entity.F_DepartmentId = OperatorProvider.Provider.GetCurrent().DepartmentId;
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

        ////导出excel
        //     [HttpGet]
        //     [HandlerAuthorize]
        //     public FileResult export(string keyword)
        //     {
        //         //参数 字段名->string[]{"F_Id",value}
        //         IDictionary<string, string> parms = new Dictionary<string, string>();
        //         //过滤条件
        //         if(!Ext.IsEmpty(keyword))
        //             parms.Add("F_RealName", keyword);

        //         DbParameter[] dbParameter = createParms(parms);

        //         string exportSql = "";//createExportSql("Schedule_WishCourseTask", parms);
        //         //string exportSql = "";
        //         //Console.WriteLine("exportSql==>" + exportSql);
        //         DataTable users = app.getDataTable(exportSql, dbParameter);
        //         ///////////////////写流
        //         MemoryStream ms = new NPOIExcel().ToExcelStream(users, "列表");
        //         ms.Seek(0, SeekOrigin.Begin);
        //         string filename = "列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
        //         return File(ms, "application/ms-excel", filename);
        //     }

        //     //导入excel
        //     [HttpPost]
        //     [HandlerAjaxOnly]
        //     [HandlerAuthorize]
        //     [ValidateAntiForgeryToken]
        //     public ActionResult import(string filePath)
        //     {
        //         IDictionary<string, string[]> rules = new Dictionary<string, string[]>();
        //             rules.Add("F_Id", new string[] { "报名ID", "" });
        //             rules.Add("F_SortCode", new string[] { "序号", "" });
        //             rules.Add("F_DepartmentId", new string[] { "所属部门", "" });
        //             rules.Add("F_DeleteMark", new string[] { "删除标记", "" });
        //             rules.Add("F_CreatorUserId", new string[] { "创建者", "" });
        //             rules.Add("F_Memo", new string[] { "备注", "" });
        //             rules.Add("F_TaskName", new string[] { "F_TaskName", "" });
        //             rules.Add("F_WishCourseGrade", new string[] { "F_WishCourseGrade", "" });
        //             rules.Add("F_StartTime", new string[] { "F_StartTime", "" });
        //             rules.Add("F_EndTime", new string[] { "F_EndTime", "" });
        //             rules.Add("F_Status", new string[] { "F_Status", "" });
        //             rules.Add("F_TaskType", new string[] { "F_TaskType", "" });
        //             rules.Add("F_SourceTaskId", new string[] { "F_SourceTaskId", "" });

        //         //////////////////处理数据(机构 岗位 等字典替换，过滤不要的字段，修改表头)
        //        List< Schedule_WishCourseTask_Entity> list =ExportHtmlTableToExcel< Schedule_WishCourseTask_Entity>(Server.MapPath(filePath), rules); //ExcelToList< Schedule_WishCourseTask_Entity>(Server.MapPath(filePath), rules);

        //         ///////////////////入库
        //         if (list == null)
        //             return Error("导入失败");
        //         app.import(list);
        //         return Success("导入成功。");
        //     }

        //设置预选科目
        public ActionResult CourseGroupForm()
        {
            return View();
        }

        public ActionResult GetWishCourseGroupFormJson(string keyValue)
        {
            var datas = groupApp.GetList(t => t.F_TaskId == keyValue);
            return Content(datas.ToJson());
        }

        public ActionResult SetCourseGroup(string F_TaskId, string F_CourseGroupIds)
        {
            var taskgroup = groupApp.GetList(t => t.F_TaskId == F_TaskId);
            taskgroup.ForEach(p =>
            {
                if (!F_CourseGroupIds.Contains(p.F_CourseGroupId))
                    groupApp.Delete(p.F_Id);
            });
            var courseGroupIds = F_CourseGroupIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var courseGroupId in courseGroupIds)
            {
                if (taskgroup.Where(p => p.F_CourseGroupId == courseGroupId).FirstOrDefault() == null)
                {
                    groupApp.SubmitForm(new Schedule_WCTask_Group_Entity
                    {
                        F_CourseGroupId = courseGroupId,
                        F_TaskId = F_TaskId
                    }, null);
                }
            }
            return Success("操作成功。");
        }

        public ActionResult ToTaskForm()
        {
            return View();
        }

        public ActionResult ToTaskSubmit(string F_TaskId, string F_TaskName, string F_StartTime, string F_EndTime, string F_CourseGroupIds)
        {
            var withTask = app.GetForm(F_TaskId);
            var task = new Schedule_WishCourseTask_Entity();
            task.F_TaskType = 1;
            task.F_TaskName = F_TaskName;
            task.F_StartTime = Convert.ToDateTime(F_StartTime);
            task.F_EndTime = Convert.ToDateTime(F_EndTime);
            task.F_DivisId = withTask.F_DivisId;
            task.F_GradeId = withTask.F_GradeId;
            task.F_SemesterId = withTask.F_SemesterId;
            task.F_SourceTaskId = withTask.F_Id;
            task.F_Year = withTask.F_Year;
            app.SubmitForm(task, null);

            var courseGroupIds = F_CourseGroupIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var courseGroupId in courseGroupIds)
            {
                groupApp.SubmitForm(new Schedule_WCTask_Group_Entity
                {
                    F_CourseGroupId = courseGroupId,
                    F_TaskId = task.F_Id
                }, null);
            }
            return Success("操作成功。");
        }
    }
}