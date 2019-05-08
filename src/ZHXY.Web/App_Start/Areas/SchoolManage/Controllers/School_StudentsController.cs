/*
***************************************************************
* 公司名称    :天亿信达
* 系统名称    :NFine
* 类 名 称    :HomeController
* 功能描述    :
* 业务描述    :
* 作 者 名    :易本辰
* 开发日期    :2018-01-08 10:09:24
***************************************************************
* 修改履历    :
* 修改编号    :
*--------------------------------------------------------------
* 修改日期    :YYYY/MM/DD
* 修 改 者    :XXXXXX
* 修改内容    :
***************************************************************
*/

using NFine.Application;
using NFine.Application.SchoolManage;
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Code.Excel;
using NFine.Domain.Entity.SchoolManage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    public class School_StudentsController : ControllerBase
    {
        public ActionResult UpdSchool_Index()
        {
            return View();
        }

        private School_Students_App app = new School_Students_App();
        private School_Students_StatusLog_App logapp = new School_Students_StatusLog_App();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_DepartmentId, string F_Grade, string F_Class, string F_Year)
        {
            var data = new
            {
                rows = app.GetList(pagination, keyword, F_DepartmentId, F_Grade, F_Class, F_Year),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridSelect()
        {
            //var data = new School_Students_Entity();
            List<Student> list = new List<Student>();
            foreach (var item in OperatorProvider.Provider.GetCurrent().Classes)
            {
                foreach (var item1 in app.GetListByF_Class_ID(item.Key))
                {
                    list.Add(item1);
                }
            }
            //School_Teachers_Entity Teacher = new School_Teachers_App().GetFormByF_Num("12120180001");
            //if (Teacher!=null)
            //{
            //    string F_TeacherId = Teacher.F_Id;
            //    var classteacher = new School_Class_Info_Teacher_App().GetFormByF_Leader_Tea(F_TeacherId);
            //    if (classteacher.Count>0)
            //    {
            //        string F_ClassID = classteacher.First().F_ClassID;
            //        return Content(app.GetListByF_Class_ID(F_ClassID).ToJson());
            //    }
            //}
            return Content(list.ToJson());
        }

        /// <summary>
        /// 根据班级ID查学生
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridSelectByClassId(string F_ClassID)
        {
            var list = app.GetListByF_Class_ID(F_ClassID);
            return Content(list.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = app.GetForm(keyValue);

            //var obj = new object();
            //var modifier = new UserEntity();
            //Dictionary<string, object> dic = CacheFactory.Cache().GetCache<Dictionary<string, object>>(Cons.ORGANIZE);
            //if (data.F_Class_ID != null && dic.TryGetValue(data.F_Class_ID, out obj))
            //{
            //    data.F_Class_ID = GetPropertyValue(obj, "fullname").ToString();
            //}
            //if (data.F_Divis_ID != null && dic.TryGetValue(data.F_Divis_ID, out obj))
            //{
            //    data.F_Divis_ID = GetPropertyValue(obj, "fullname").ToString();
            //}
            //if (data.F_Grade_ID != null && dic.TryGetValue(data.F_Grade_ID, out obj))
            //{
            //    data.F_Grade_ID = GetPropertyValue(obj, "fullname").ToString();
            //}
            return Content(data.ToJson());
        }

        /// <summary>
        /// </summary>
        /// <param name="keyValue"> 学号、id </param>
        /// <returns>  </returns>
        [HttpGet]
        //[HandlerAuthorize]
        [HandlerAjaxOnly]
        public ActionResult GetFormJsonByStuNo(string keyValue)
        {
            var data = app.GetFormBykeyValue(keyValue);

            //var obj = new object();
            //var modifier = new UserEntity();
            //Dictionary<string, object> dic = CacheFactory.Cache().GetCache<Dictionary<string, object>>(Cons.ORGANIZE);
            //if (data.F_Class_ID != null && dic.TryGetValue(data.F_Class_ID, out obj))
            //{
            //    data.F_Class_ID = GetPropertyValue(obj, "fullname").ToString();
            //}
            //if (data.F_Divis_ID != null && dic.TryGetValue(data.F_Divis_ID, out obj))
            //{
            //    data.F_Divis_ID = GetPropertyValue(obj, "fullname").ToString();
            //}
            //if (data.F_Grade_ID != null && dic.TryGetValue(data.F_Grade_ID, out obj))
            //{
            //    data.F_Grade_ID = GetPropertyValue(obj, "fullname").ToString();
            //}
            return Content(data.ToJson());
        }

        public ActionResult GetDetailsJson(string keyValue)
        {
            var data = app.GetForm(keyValue);

            var obj = new object();
            //var modifier = new UserEntity();
            Dictionary<string, object> dic = CacheFactory.Cache().GetCache<Dictionary<string, object>>(Cons.ORGANIZE);
            if (data.F_Class_ID != null && dic.TryGetValue(data.F_Class_ID, out obj))
            {
                data.F_Class_ID = GetPropertyValue(obj, "fullname").ToString();
            }
            if (data.F_Divis_ID != null && dic.TryGetValue(data.F_Divis_ID, out obj))
            {
                data.F_Divis_ID = GetPropertyValue(obj, "fullname").ToString();
            }
            if (data.F_Grade_ID != null && dic.TryGetValue(data.F_Grade_ID, out obj))
            {
                data.F_Grade_ID = GetPropertyValue(obj, "fullname").ToString();
            }
            string t = data.ToJson();
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Student entity, string keyValue)
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

        public ActionResult Details(string keyValue)
        {
            return View();
        }

        //导出excel
        [HttpGet]
        [HandlerAuthorize]
        public FileResult export(string keyword, string F_DepartmentId, string F_Grade, string F_Class, string F_Year)
        {
            //参数 字段名->string[]{"F_Id",value}
            IDictionary<string, string> parms = new Dictionary<string, string>();
            //过滤条件
            //if (!Ext.IsEmpty(keyword))
            //    parms.Add("F_Name", keyword);
            if (!Ext.IsEmpty(F_DepartmentId))
                parms.Add("F_Divis_ID", F_DepartmentId);
            if (!Ext.IsEmpty(F_Grade))
                parms.Add("F_Grade_ID", F_Grade);
            if (!Ext.IsEmpty(F_Class))
                parms.Add("F_Class_ID", F_Class);
            if (!Ext.IsEmpty(F_Year))
                parms.Add("F_Year", F_Year);

            var dbParameter = CreateParms(parms);

            string exportSql = CreateExportSql("School_Students", parms);
            if (!string.IsNullOrEmpty(keyword))
            {
                exportSql += " and F_StudentNum like '%" + keyword + "%' or F_Name like '%" + keyword + "%'";
            }
            //string exportSql = "";
            //Console.WriteLine("exportSql==>" + exportSql);
            var users = app.getDataTable(new BaseApp().dataScopeFilter(exportSql), dbParameter);
            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(users, "用户列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "学生档案列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }

        //导入excel
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult import(string filePath)
        {
            var list = ExcelToList<StudentsImport>(Server.MapPath(filePath), "School_SEB");
            ///////////////////入库
            if (list == null)
                return Error("导入失败");
            app.import(list);
            return Success("导入成功。");
        }

        //导入excel
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult import_two(string filePath)
        {

            var list = ExcelToList<StudentsImportTwo>(Server.MapPath(filePath), "School_Students_Two");
            ///////////////////入库
            if (list == null)
                return Error("导入失败");
            app.import_two(list);
            return Success("导入成功。");
        }

        /// <summary>
        /// 调整在线状态
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public ActionResult UpdCurStatu()
        {
            return View();
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitCurStatu(string keyValue, string F_CurStatu)
        {
            app.UpdCurStatu(keyValue, F_CurStatu);
            return Success("调整在线状态成功。");
        }

        public ActionResult GetStudentsStatuLog(Pagination pagination, string keyword)
        {
            var data = new
            {
                rows = logapp.GetList(pagination, keyword, string.Empty, string.Empty, string.Empty, string.Empty),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult getStudentDetailInfo(string F_StudentId)
        {
            var entity = app.GetForm(F_StudentId);
            StudentModel stu = new StudentModel();
            if (entity != null)
            {
                stu.F_Id = entity.F_Id;
                stu.F_Name = entity.F_Name;
                stu.F_StudentNum = entity.F_StudentNum;
                stu.F_HeadPic = entity.studentSysUser.F_HeadIcon;
                stu.F_Sex = entity.F_Gender;
                stu.F_CardNo = entity.F_CredNum;
                stu.F_Birthday = entity.F_Birthday;
                stu.F_Introduction = entity.F_Introduction;
                var stuhonor = new School_Stu_Honor_App().GetFormByF_Student(F_StudentId);

                List<Honor_Info> honor = new List<Honor_Info>();
                if (stuhonor.Count > 0)
                {
                    foreach (var item in stuhonor)
                    {
                        Honor_Info stuhon = new Honor_Info();
                        stuhon.F_Id = item.F_Id;
                        stuhon.F_Title = item.F_Title;
                        stuhon.F_Content = item.F_Content;
                        stuhon.F_Covers = item.F_Covers;
                        stuhon.F_Date = item.F_Date;
                        honor.Add(stuhon);
                    }
                }
                stu.F_honorInfo = honor;
            }
            return Content(stu.ToJson());
        }

        #region Model

        public class Honor_Info
        {
            public string F_Id { get; set; }
            public string F_Title { get; set; }
            public string F_Covers { get; set; }
            public string F_Content { get; set; }
            public DateTime? F_Date { get; set; }
        }

        public class StudentModel
        {
            public string F_Id { get; set; }
            public string F_Name { get; set; }
            public string F_StudentNum { get; set; }
            public string F_CardNo { get; set; }
            public string F_HeadPic { get; set; }
            public string F_Sex { get; set; }
            public DateTime? F_Birthday { get; set; }
            public string F_Introduction { get; set; }
            public List<Honor_Info> F_honorInfo { get; set; }
        }

        #endregion Model
    }
}