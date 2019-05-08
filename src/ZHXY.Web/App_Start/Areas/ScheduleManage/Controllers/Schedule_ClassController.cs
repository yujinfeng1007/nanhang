/*
***************************************************************
* 公司名称    :天亿信达
* 系统名称    :ben
* 类 名 称    :Schedule_ClassMap
* 功能描述    :
* 业务描述    :
* 作 者 名    :易本辰
* 开发日期    :2018-09-28 17:01:38
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
using NFine.Domain.Model.ScheduleModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace NFine.Web.Areas.ScheduleManage.Controllers
{
    public class Schedule_ClassController : ControllerBase
    {
        private Schedule_Class_App app = new Schedule_Class_App();

        public ActionResult PTClassForm()
        {
            return View();
        }

        public ActionResult DTClassForm()
        {
            return View();
        }

        public ActionResult DClassForm()
        {
            return View();
        }

        public ActionResult SubmitDTClassForm(string F_TaskId, string F_ClassQTY, int F_StudentMaxQTY, int F_StudentMinQTY)
        {
            app.GenerateDTClass(F_TaskId, F_ClassQTY, F_StudentMaxQTY, F_StudentMinQTY);
            return Success("操作成功。");
        }

        public ActionResult SubmitDClassForm(string F_TaskId, string F_ClassQTY, int F_StudentMaxQTY, int F_StudentMinQTY)
        {
            app.GenerateDClass(F_TaskId, F_ClassQTY, F_StudentMaxQTY, F_StudentMinQTY);
            return Success("操作成功。");
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_Year, string F_Semester, string F_Divis, string F_Grade, string F_Class)
        {
            var data = new
            {
                rows = app.GetList(pagination, F_Year, F_Semester, F_Divis, F_Grade, F_Class),
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

        public ActionResult SubmitForm(SubmitPTClassParms parms)
        {
            //entity.F_DepartmentId = OperatorProvider.Provider.GetCurrent().DepartmentId;
            app.GeneratePTClass(parms);
            return Success("操作成功。");
        }

        public ActionResult GetSelectJson(string F_TaskId)
        {
            var data = app.GetList(t => t.F_TaskId == F_TaskId);
            List<object> list = new List<object>();
            foreach (Schedule_Class_Entity item in data)
            {
                list.Add(new { id = item.F_Id, text = item.F_Name });
            }
            return Content(list.ToJson());
        }

        public ActionResult GetGradeSelectJson(string F_Year, string F_Semester, string F_Divis, string F_Grade)
        {
            var data = app.GetList(t => t.F_Year == F_Year && t.F_SemesterId == F_Semester && t.F_DivisId == F_Divis && t.F_GradeId == F_Grade);
            List<object> list = new List<object>();
            foreach (Schedule_Class_Entity item in data)
            {
                list.Add(new { id = item.F_Id, text = item.F_Name });
            }
            return Content(list.ToJson());
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

        public ActionResult PublishClass(string keyValue)
        {
            app.PublishClass(keyValue);
            return Success("发布成功。");
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

        //         string exportSql = "";//createExportSql("Schedule_Class", parms);
        //         //string exportSql = "";
        //         //Console.WriteLine("exportSql==>" + exportSql);
        //         DataTable users = app.getDataTable(exportSql, dbParameter);
        //         ///////////////////写流
        //         MemoryStream ms = new NPOIExcel().ToExcelStream(users, "列表");
        //         ms.Seek(0, SeekOrigin.Begin);
        //         string filename = "列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
        //         return File(ms, "application/ms-excel", filename);
        //     }

        ////导入excel
        //[HttpPost]
        //[HandlerAjaxOnly]
        //[HandlerAuthorize]
        //[ValidateAntiForgeryToken]
        //public ActionResult import(string filePath)
        //{
        //    IDictionary<string, string[]> rules = new Dictionary<string, string[]>();
        //        rules.Add("F_Id", new string[] { "报名ID", "" });
        //        rules.Add("F_SortCode", new string[] { "序号", "" });
        //        rules.Add("F_DepartmentId", new string[] { "所属部门", "" });
        //        rules.Add("F_DeleteMark", new string[] { "删除标记", "" });
        //        rules.Add("F_CreatorUserId", new string[] { "创建者", "" });
        //        rules.Add("F_Memo", new string[] { "备注", "" });
        //        rules.Add("F_Name", new string[] { "F_Name", "" });
        //        rules.Add("F_Type", new string[] { "F_Type", "" });
        //        rules.Add("F_Courses", new string[] { "F_Courses", "" });
        //        rules.Add("F_CourseCount", new string[] { "F_CourseCount", "" });
        //        rules.Add("F_MaxStudent", new string[] { "F_MaxStudent", "" });

        //    //////////////////处理数据(机构 岗位 等字典替换，过滤不要的字段，修改表头)
        //   List< Schedule_Class_Entity> list =ExportHtmlTableToExcel< Schedule_Class_Entity>(Server.MapPath(filePath), rules); //ExcelToList< Schedule_Class_Entity>(Server.MapPath(filePath), rules);

        //    ///////////////////入库
        //    if (list == null)
        //        return Error("导入失败");
        //    app.import(list);
        //    return Success("导入成功。");
        //}
    }
}