/*
***************************************************************
* 公司名称    :天亿信达
* 系统名称    :ben
* 类 名 称    :Schedule_ClassStudentMap
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
using NFine.Domain.Entity.SchoolManage;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace NFine.Web.Areas.ScheduleManage.Controllers
{
    public class Schedule_ClassStudentController : ControllerBase
    {
        private Schedule_ClassStudent_App app = new Schedule_ClassStudent_App();
        private Schedule_WishCourseGroup_App stcgApp = new Schedule_WishCourseGroup_App();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_Year, string F_Semester, string F_Divis, string F_Grade, string F_Class)
        {
            var data = new
            {
                rows = app.GetList(pagination, F_Class).Select(p => new
                {
                    F_Year = p.Schedule_Class_Entity == null ? "" : p.Schedule_Class_Entity.F_Year,
                    F_SemesterId = p.Schedule_Class_Entity == null ? "" : p.Schedule_Class_Entity.F_SemesterId,
                    F_DivisId = p.Schedule_Class_Entity == null ? "" : p.Schedule_Class_Entity.F_DivisId,
                    F_GradeId = p.Schedule_Class_Entity == null ? "" : p.Schedule_Class_Entity.F_GradeId,
                    F_Name = p.Schedule_Class_Entity == null ? "未分班" : p.Schedule_Class_Entity.F_Name,
                    F_StudentName = p.School_Students_Entity.F_Name,
                    F_CreatorTime = p.F_CreatorTime,
                    F_Id = p.F_Id
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

        public ActionResult GetStudentGridJson(string F_TaskId)
        {
            var stcgDatas = stcgApp.GetList(t => t.F_TaskId == F_TaskId);
            var datas = app.GetList(t => t.F_TaskId == F_TaskId);
            List<Student> students = new List<Student>();
            stcgDatas.ForEach(p =>
            {
                if (datas.Where(t => t.F_StudentId == p.F_StudentID).Count() <= 0)
                {
                    students.Add(new Student
                    {
                        F_Id = p.F_StudentID,
                        F_Name = p.School_Students_Entity.F_Name
                    });
                }
            });
            return Content(students.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Schedule_ClassStudent_Entity entity, string keyValue)
        {
            //entity.F_DepartmentId = OperatorProvider.Provider.GetCurrent().DepartmentId;
            app.SubmitForm(entity, keyValue);
            return Success("操作成功。");
        }

        //[HttpPost]
        //[HandlerAuthorize]
        //[HandlerAjaxOnly]
        //[ValidateAntiForgeryToken]
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

        //         string exportSql = "";//createExportSql("Schedule_ClassStudent", parms);
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
        //             rules.Add("F_StudentId", new string[] { "F_StudentId", "" });
        //             rules.Add("F_ClassId", new string[] { "F_ClassId", "" });

        //         //////////////////处理数据(机构 岗位 等字典替换，过滤不要的字段，修改表头)
        //        List< Schedule_ClassStudent_Entity> list =ExportHtmlTableToExcel< Schedule_ClassStudent_Entity>(Server.MapPath(filePath), rules); //ExcelToList< Schedule_ClassStudent_Entity>(Server.MapPath(filePath), rules);

        //         ///////////////////入库
        //         if (list == null)
        //             return Error("导入失败");
        //         app.import(list);
        //         return Success("导入成功。");
        //     }
    }
}