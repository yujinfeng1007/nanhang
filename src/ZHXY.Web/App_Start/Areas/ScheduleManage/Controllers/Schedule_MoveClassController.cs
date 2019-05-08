/*
***************************************************************
* 公司名称    :天亿信达
* 系统名称    :ben
* 类 名 称    :Schedule_MoveClassMap
* 功能描述    :
* 业务描述    :
* 作 者 名    :易本辰
* 开发日期    :2018-11-02 18:01:46
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
using NFine.Web.Areas.SchoolManage;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace NFine.Web.Areas.ScheduleManage.Controllers
{
    public class Schedule_MoveClassController : ControllerBase
    {
        private School_Teachers_App teacherApp = new School_Teachers_App();

        private Schedule_MoveClass_App app = new Schedule_MoveClass_App();
        private Schedule_MoveClassStudent_App studentApp = new Schedule_MoveClassStudent_App();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_Year, string F_Semester, string F_Divis, string F_Grade, string F_Class)
        {
            var data = new
            {
                rows = app.GetList(pagination, F_Year, F_Semester, F_Divis, F_Grade, keyword)
                .Select(t =>
                {
                    var teacher = teacherApp.GetForm(t.F_TeacherId);

                    return new
                    {
                        F_Id = t.F_Id,
                        F_Year = t.F_Year,
                        F_SemesterId = t.F_SemesterId,
                        F_DivisId = t.F_DivisId,
                        F_GradeId = t.F_GradeId,
                        F_TeacherId = t.F_TeacherId,
                        F_TeacherName = (teacher == null ? "" : teacher.F_Name),
                        F_ClassIds = t.F_ClassIds,
                        F_CourseId = t.F_CourseId,
                        F_ParentCourseId = t.F_ParentCourseId,
                        F_CreatorTime = t.F_CreatorTime,
                        F_Name = t.F_Name,
                        F_CourseTime = t.F_CourseTime,
                        F_StudentQTY = studentApp.GetList(q => q.F_MoveClassId == t.F_Id).Count
                    };
                }),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
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

        public ActionResult GetGradeSelectJson(string F_Year, string F_Semester, string F_Divis, string F_Grade)
        {
            var data = app.GetList(t => t.F_Year == F_Year && t.F_SemesterId == F_Semester && t.F_DivisId == F_Divis && t.F_GradeId == F_Grade);
            List<object> list = new List<object>();
            foreach (Schedule_MoveClass_Entity item in data)
            {
                list.Add(new { id = item.F_Id, text = item.F_Name });
            }
            return Content(list.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Schedule_MoveClass_Entity entity, string keyValue)
        {
            entity.F_TeacherId = entity.F_TeacherId.Split(',')[0];
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
        //[HttpGet]
        //[HandlerAuthorize]
        //public FileResult export(string keyword)
        //{
        //    //参数 字段名->string[]{"F_Id",value}
        //    IDictionary<string, string> parms = new Dictionary<string, string>();
        //    //过滤条件
        //    if (!Ext.IsEmpty(keyword))
        //        parms.Add("F_RealName", keyword);

        //    DbParameter[] dbParameter = createParms(parms);

        //    string exportSql = "";//createExportSql("Schedule_MoveClass", parms);
        //    //string exportSql = "";
        //    //Console.WriteLine("exportSql==>" + exportSql);
        //    DataTable users = app.getDataTable(exportSql, dbParameter);
        //    ///////////////////写流
        //    MemoryStream ms = new NPOIExcel().ToExcelStream(users, "列表");
        //    ms.Seek(0, SeekOrigin.Begin);
        //    string filename = "列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
        //    return File(ms, "application/ms-excel", filename);
        //}

        ////导入excel
        //[HttpPost]
        //[HandlerAjaxOnly]
        //[HandlerAuthorize]
        //[ValidateAntiForgeryToken]
        //public ActionResult import(string filePath)
        //{
        //    IDictionary<string, string[]> rules = new Dictionary<string, string[]>();
        //    rules.Add("F_Id", new string[] { "F_Id", "" });
        //    rules.Add("F_SortCode", new string[] { "F_SortCode", "" });
        //    rules.Add("F_DepartmentId", new string[] { "F_DepartmentId", "" });
        //    rules.Add("F_DeleteMark", new string[] { "F_DeleteMark", "" });
        //    rules.Add("F_CreatorUserId", new string[] { "F_CreatorUserId", "" });
        //    rules.Add("F_Memo", new string[] { "F_Memo", "" });
        //    rules.Add("F_Name", new string[] { "F_Name", "" });
        //    rules.Add("F_CourseId", new string[] { "F_CourseId", "" });
        //    rules.Add("F_TeacherId", new string[] { "F_TeacherId", "" });
        //    rules.Add("F_ClassIds", new string[] { "F_ClassIds", "" });
        //    rules.Add("F_GradeId", new string[] { "F_GradeId", "" });

        //    //////////////////处理数据(机构 岗位 等字典替换，过滤不要的字段，修改表头)
        //    List<Schedule_MoveClass_Entity> list = ExportHtmlTableToExcel<Schedule_MoveClass_Entity>(Server.MapPath(filePath), rules); //ExcelToList< Schedule_MoveClass_Entity>(Server.MapPath(filePath), rules);

        //    ///////////////////入库
        //    if (list == null)
        //        return Error("导入失败");
        //    app.import(list);
        //    return Success("导入成功。");
        //}
    }
}