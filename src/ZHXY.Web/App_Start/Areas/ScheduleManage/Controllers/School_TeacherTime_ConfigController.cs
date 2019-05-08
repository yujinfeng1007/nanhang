/*
***************************************************************
* 公司名称    :天亿信达
* 系统名称    :ben
* 类 名 称    :School_TeacherTime_ConfigMap
* 功能描述    :
* 业务描述    :
* 作 者 名    :易本辰
* 开发日期    :2018-07-23 13:33:46
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
using NFine.Code.Excel;
using NFine.Domain.Entity.ScheduleManage;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.Model;
using NFine.Web.Areas.SchoolManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.Mvc;

namespace NFine.Web.Areas.ScheduleManage.Controllers
{
    public class School_TeacherTime_ConfigController : ControllerBase
    {
        private School_TeacherTime_Config_App app = new School_TeacherTime_Config_App();
        private School_Course_App courseapp = new School_Course_App();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetSelectJsonByCategoryId(string keyword, string parentId)
        {
            var data = app.GetClassByGrade(parentId);
            List<object> list = new List<object>();
            foreach (Organize item in data)
            {
                list.Add(new { id = item.F_Id, text = item.F_FullName, template = item.F_Template, year = item.F_Year });
            }
            return Content(list.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_Divis, string F_Grade, string F_Year, string F_Class, string F_Semester, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            var data = app.GetList(F_Divis, F_Grade, F_Year, F_Semester);
            //new
            //{
            //    rows = app.GetList( keyword, F_Divis, F_Grade, F_Year, F_Class, F_Semester, F_CreatorTime_Start, F_CreatorTime_Stop),
            //    total = pagination.total,
            //    page = pagination.page,
            //    records = pagination.records
            //};
            return Content(data.ToJson());
        }

        // 获取年级所有课目课时
        public ActionResult GetAllCourseList(Pagination pagination, string keyword, string F_Divis, string F_Grade, string F_Year, string F_Class, string F_Semester, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            var data = new
            {
                rows = app.GetAllCourseList(pagination, keyword, F_Divis, F_Grade, F_Year, F_Class, F_Semester, F_CreatorTime_Start, F_CreatorTime_Stop),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

        public ActionResult UpdForm()
        {
            return View();
        }

        // 获取当前班级课时
        public ActionResult GetClassFormJson(string keyValue, string F_Divis, string F_Grade, string F_Year, string F_Semester)
        {
            var data = app.GetFormByClass(keyValue, F_Divis, F_Grade, F_Year, F_Semester);
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
        public ActionResult SubmitForm(Schedule_TeacherTime_Config_Entity entity, string keyValue)
        {
            //entity.F_DepartmentId = OperatorProvider.Provider.GetCurrent().DepartmentId;
            app.SubmitForm(entity, keyValue);
            return Success("操作成功。");
        }

        public ActionResult SubmitClassCourseTime(ClassTimetableModel model, string F_Divis, string F_Grade, string F_Year, string F_Class, string F_Semester)
        {
            model.F_ClassId = F_Class;
            app.SubmitClassCourseTime(model, F_Divis, F_Grade, F_Year, F_Class, F_Semester);
            return Success("操作成功。");
        }

        public ActionResult CopyForm()
        {
            return View();
        }

        public ActionResult SubmitCopy(string F_ClassId, string F_DClassIds, string F_Divis, string F_Grade, string F_Year, string F_Semester)
        {
            string[] F_DClasss = F_DClassIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            app.SubmitCopy(F_ClassId, F_DClasss, F_Divis, F_Grade, F_Year, F_Semester);
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

        //导出excel
        [HttpGet]
        [HandlerAuthorize]
        public FileResult export(string F_Divis, string F_Grade, string F_Year, string F_Class, string F_Semester, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            var data = app.GetList(F_Divis, F_Grade, F_Year, F_Semester);
            DataTable dt = ToExportDT(data);
            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(dt, "列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }

        private DataTable ToExportDT(List<CourseTimetableModel> data)
        {
            if (data == null || data.Count <= 0)
                return null;
            DataTable dt = new DataTable();
            dt.Columns.Add("年度", Type.GetType("System.String"));
            dt.Columns.Add("学期", Type.GetType("System.String"));
            dt.Columns.Add("学部", Type.GetType("System.String"));
            dt.Columns.Add("年级", Type.GetType("System.String"));
            dt.Columns.Add("科目", Type.GetType("System.String"));
            foreach (var citem in data[0].ClassItems)
            {
                dt.Columns.Add(citem.F_ClassName, Type.GetType("System.String"));
            }
            Dictionary<string, object> orgs = CacheConfig.GetOrganizeListByCache();
            foreach (var item in data)
            {
                DataRow dr = dt.NewRow();
                dr["年度"] = item.F_Year;
                dr["学期"] = item.F_SemesterId;
                dr["学部"] = GetPropertyValue(orgs[item.F_Divis_ID], "fullname");
                dr["年级"] = GetPropertyValue(orgs[item.F_GradeId], "fullname");
                dr["科目"] = item.F_Course;
                foreach (var citem in item.ClassItems)
                {
                    dr[citem.F_ClassName] = citem.F_CourseTime;
                    dr[citem.F_RepeatCount] = citem.F_RepeatCount;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

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
        //        rules.Add("F_DeleteMark", new string[] { "删除标记", "" });
        //        rules.Add("F_CreatorUserId", new string[] { "创建者", "" });
        //        rules.Add("F_Memo", new string[] { "备注", "" });
        //        rules.Add("F_Year", new string[] { "F_Year", "" });
        //        rules.Add("F_SemesterId", new string[] { "F_SemesterId", "" });
        //        rules.Add("F_GradeId", new string[] { "F_GradeId", "" });
        //        rules.Add("F_ClassId", new string[] { "F_ClassId", "" });
        //        rules.Add("F_CourseId", new string[] { "F_CourseId", "" });
        //        rules.Add("F_CourseTime", new string[] { "F_CourseTime", "" });
        //        rules.Add("F_UseCourseTime", new string[] { "F_UseCourseTime", "" });
        //        rules.Add("F_TeacherId", new string[] { "F_TeacherId", "" });

        // //////////////////处理数据(机构 岗位 等字典替换，过滤不要的字段，修改表头) List< School_TeacherTime_Config_Entity>
        // list =ExportHtmlTableToExcel< School_TeacherTime_Config_Entity>(Server.MapPath(filePath),
        // rules); //ExcelToList< School_TeacherTime_Config_Entity>(Server.MapPath(filePath), rules);

        //    ///////////////////入库
        //    if (list == null)
        //        return Error("导入失败");
        //    app.import(list);
        //    return Success("导入成功。");
        //}
    }
}