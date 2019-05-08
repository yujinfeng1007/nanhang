/*
***************************************************************
* 公司名称    :天亿信达
* 系统名称    :ben
* 类 名 称    :School_PRule_ClassroomMap
* 功能描述    :
* 业务描述    :
* 作 者 名    :易本辰
* 开发日期    :2018-09-11 09:41:00
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
using System.Collections.Generic;
using System.Web.Mvc;

namespace NFine.Web.Areas.ScheduleManage.Controllers
{
    public class School_PRule_ClassroomController : ControllerBase
    {
        public class SubmitModel
        {
            public int value { get; set; }
            public int dateIndex { get; set; }
            public int lessonIndex { get; set; }
        }

        private School_PRule_Classroom_App app = new School_PRule_Classroom_App();

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

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Schedule_PRule_Classroom_Entity entity, string keyValue)
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

        //         string exportSql = "";//createExportSql("School_PRule_Classroom", parms);
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
        //             rules.Add("F_SortCode", new string[] { "F_SortCode", "" });
        //             rules.Add("F_DeleteMark", new string[] { "F_DeleteMark", "" });
        //             rules.Add("F_Id", new string[] { "F_Id", "" });
        //             rules.Add("F_CreatorUserId", new string[] { "F_CreatorUserId", "" });
        //             rules.Add("F_Memo", new string[] { "F_Memo", "" });
        //             rules.Add("F_Year", new string[] { "F_Year", "" });
        //             rules.Add("F_SemesterId", new string[] { "F_SemesterId", "" });
        //             rules.Add("F_GradeId", new string[] { "F_GradeId", "" });
        //             rules.Add("F_ClassroomId", new string[] { "F_ClassroomId", "" });
        //             rules.Add("F_WeekN", new string[] { "F_WeekN", "" });
        //             rules.Add("F_Rule1", new string[] { "F_Rule1", "" });
        //             rules.Add("F_Rule2", new string[] { "F_Rule2", "" });
        //             rules.Add("F_Rule3", new string[] { "F_Rule3", "" });
        //             rules.Add("F_Rule4", new string[] { "F_Rule4", "" });
        //             rules.Add("F_Rule5", new string[] { "F_Rule5", "" });
        //             rules.Add("F_Rule6", new string[] { "F_Rule6", "" });
        //             rules.Add("F_Rule7", new string[] { "F_Rule7", "" });
        //             rules.Add("F_Rule8", new string[] { "F_Rule8", "" });
        //             rules.Add("F_Rule9", new string[] { "F_Rule9", "" });
        //             rules.Add("F_Rule10", new string[] { "F_Rule10", "" });
        //             rules.Add("F_Rule11", new string[] { "F_Rule11", "" });
        //             rules.Add("F_Rule12", new string[] { "F_Rule12", "" });
        //             rules.Add("F_DepartmentId", new string[] { "F_DepartmentId", "" });

        //         //////////////////处理数据(机构 岗位 等字典替换，过滤不要的字段，修改表头)
        //        List< School_PRule_Classroom_Entity> list =ExportHtmlTableToExcel< School_PRule_Classroom_Entity>(Server.MapPath(filePath), rules); //ExcelToList< School_PRule_Classroom_Entity>(Server.MapPath(filePath), rules);

        //         ///////////////////入库
        //         if (list == null)
        //             return Error("导入失败");
        //         app.import(list);
        //         return Success("导入成功。");
        //     }
        public ActionResult GetClassroomRule(string F_Classroom, string F_Divis, string F_Grade, string F_Year, string F_Class, string F_Semester)
        {
            var data = GetRule(F_Year, F_Semester, F_Classroom);
            return Content(data);
        }

        public ActionResult SetClassroomRule(string F_Year, string F_Semester, string F_Classroom, List<SubmitModel> data)
        {
            SetRule(F_Year, F_Semester, F_Classroom, data);
            return Success("操作成功。");
        }

        public string GetRule(string F_Year, string F_Semester, string F_Classroom)
        {
            var data = app.GetList(F_Year, F_Semester, F_Classroom);
            return (data.ToJson());
        }

        public void SetRule(string F_Year, string F_Semester, string F_Classroom, List<SubmitModel> datas)
        {
            app.DeleteForm(F_Year, F_Semester, F_Classroom);
            if (datas == null)
                return;
            foreach (var data in datas)
            {
                Schedule_PRule_Classroom_Entity model = new Schedule_PRule_Classroom_Entity();
                model.Create();
                model.F_Year = F_Year;
                model.F_SemesterId = F_Semester;
                model.F_ClassroomId = F_Classroom;

                model.F_WeekN = data.dateIndex;
                model.F_CourseIndex = data.lessonIndex;
                model.F_Rule = data.value;
                app.AddEntity(model);
            }
        }
    }
}