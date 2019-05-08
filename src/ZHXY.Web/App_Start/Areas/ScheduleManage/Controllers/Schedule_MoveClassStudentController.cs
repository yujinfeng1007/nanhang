/*
***************************************************************
* 公司名称    :天亿信达
* 系统名称    :ben
* 类 名 称    :Schedule_MoveClassStudentMap
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
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace NFine.Web.Areas.ScheduleManage.Controllers
{
    public class Schedule_MoveClassStudentController : ControllerBase
    {
        private School_Teachers_App teacherApp = new School_Teachers_App();

        private Schedule_MoveClassStudent_App app = new Schedule_MoveClassStudent_App();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_Year, string F_Semester, string F_Divis, string F_Grade, string F_Class)
        {
            var data = new
            {
                rows = app.GetList(pagination, F_Year, F_Semester, F_Divis, F_Grade, F_Class).Select(p =>
                {
                    var teacher = teacherApp.GetForm(p.Schedule_MoveClass_Entity.F_TeacherId);
                    return new
                    {
                        F_Year = F_Year,/// p.Schedule_Class_Entity == null ? "" : p.Schedule_Class_Entity.F_Year,
                        F_SemesterId = F_Semester,// p.Schedule_Class_Entity == null ? "" : p.Schedule_Class_Entity.F_SemesterId,
                        F_DivisId = F_Divis,// p.Schedule_Class_Entity == null ? "" : p.Schedule_Class_Entity.F_DivisId,
                        F_GradeId = F_Grade,// p.Schedule_Class_Entity == null ? "" : p.Schedule_Class_Entity.F_GradeId,
                        F_Name = p.Schedule_MoveClass_Entity == null ? "不走班" : p.Schedule_MoveClass_Entity.F_Name,
                        F_StudentName = p.School_Students_Entity.F_Name,
                        F_TeacherName = (teacher == null ? "" : teacher.F_Name),
                        F_CreatorTime = p.F_CreatorTime,
                        F_ClassId = p.F_MoveClassId,
                        F_Id = p.F_Id
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

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Schedule_MoveClassStudent_Entity entity, string keyValue)
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

        //    string exportSql = "";//createExportSql("Schedule_MoveClassStudent", parms);
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
        //    rules.Add("F_StudentId", new string[] { "F_StudentId", "" });
        //    rules.Add("F_MoveClassId", new string[] { "F_MoveClassId", "" });

        //    //////////////////处理数据(机构 岗位 等字典替换，过滤不要的字段，修改表头)
        //    List<Schedule_MoveClassStudent_Entity> list = ExportHtmlTableToExcel<Schedule_MoveClassStudent_Entity>(Server.MapPath(filePath), rules); //ExcelToList< Schedule_MoveClassStudent_Entity>(Server.MapPath(filePath), rules);

        //    ///////////////////入库
        //    if (list == null)
        //        return Error("导入失败");
        //    app.import(list);
        //    return Success("导入成功。");
        //}
    }
}