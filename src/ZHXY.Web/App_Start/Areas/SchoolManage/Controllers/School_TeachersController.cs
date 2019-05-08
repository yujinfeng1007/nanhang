using NFine.Application;
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Code.Excel;
using NFine.Domain.Entity.SchoolManage;
using NFine.Domain.Entity.SystemManage;
using NFine.Web.Areas.SystemManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    public class School_TeachersController : ControllerBase
    {
        private Sys_User_Role_App userroleApp = new Sys_User_Role_App();
        private RoleApp roleApp = new RoleApp();
        private School_Teachers_App app = new School_Teachers_App();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_DepartmentId, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            var data = new
            {
                rows = app.GetList(pagination, keyword, F_DepartmentId, F_CreatorTime_Start, F_CreatorTime_Stop),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJsonSelect(string F_Teachers_ID)
        {
            var data = app.GetListSelect(F_Teachers_ID);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridSelect(string F_Name, string F_Divis_ID)
        {
            var data = app.GetSelect(F_Name, F_Divis_ID);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = app.GetForm(keyValue);
            ////将用户id替换成姓名
            //var creator = new UserEntity();
            //var modifier = new UserEntity();
            //Dictionary<string, UserEntity> dic = CacheFactory.Cache().GetCache<Dictionary<string, UserEntity>>(Cons.USERS);
            //if (data.F_CreatorUserId != null)
            //{
            //    if (dic.TryGetValue(data.F_CreatorUserId, out creator))
            //    {
            //        data.F_CreatorUserId = creator.F_RealName;
            //    }
            //}
            //if (data.F_LastModifyUserId != null)
            //{
            //    if (dic.TryGetValue(data.F_LastModifyUserId, out modifier))
            //    {
            //        data.F_LastModifyUserId = modifier.F_RealName;
            //    }
            //}
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJsonByF_Num(string F_Num)
        {
            var data = app.GetFormByF_Num(F_Num);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Teacher entity, string keyValue)
        {
            app.SubmitForm(entity, keyValue);
            CacheFactory.Cache().RemoveCache(Cons.CLASSTEACHERS);
            CacheFactory.Cache().WriteCache(CacheConfig.GetClassTeachers(), Cons.CLASSTEACHERS);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            app.DeleteForm(keyValue);
            CacheFactory.Cache().RemoveCache(Cons.CLASSTEACHERS);
            CacheFactory.Cache().WriteCache(CacheConfig.GetClassTeachers(), Cons.CLASSTEACHERS);
            return Success("删除成功。");
        }

        //导出excel
        [HttpGet]
        [HandlerAuthorize]
        public FileResult export(string keyword, string F_DepartmentId)
        {
            //参数 字段名->string[]{"F_Id",value}
            IDictionary<string, string> parms = new Dictionary<string, string>();
            //过滤条件
            //if (!Ext.IsEmpty(keyword))
            //    parms.Add("F_Name", keyword);
            if (!Ext.IsEmpty(F_DepartmentId))
                parms.Add("F_Divis_ID", F_DepartmentId);

            var dbParameter = CreateParms(parms);

            string exportSql = CreateExportSql("School_Teachers", parms);
            if (!Ext.IsEmpty(keyword))
            {
                exportSql += " and t.F_Name like '%" + keyword + "%' or t.F_Num like '%" + keyword + "%' ";
            }
            //string exportSql = "";
            //Console.WriteLine("exportSql==>" + exportSql);
            var users = app.GetDataTable(new BaseApp().dataScopeFilter(exportSql), dbParameter);
            Dictionary<string, object> roles = CacheConfig.GetRoleListByCache();

            foreach (DataRow item in users.Rows)
            {
                try
                {
                    User user = new UserApp().GetListBYF_Account(item["教师工号"].ToString());
                    var userroledata = userroleApp.GetListByUserId(user.F_Id);
                    string RoleId = string.Empty;
                    object tmp = string.Empty;
                    foreach (SysUserRole userroleentity in userroledata)
                    {
                        Role role = roleApp.GetForm(userroleentity.F_Role);
                        if (role != null && roles.TryGetValue(userroleentity.F_Role, out tmp))
                        {
                            RoleId += GetPropertyValue(tmp, "fullname") + ",";
                        }
                    }
                    item["角色主键"] = RoleId;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(users, "用户列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "教师信息列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }

        //导入excel
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult import(string filePath)
        {
            var list = ExcelToList<Teacher>(Server.MapPath(filePath), "School_Teachers");
            ///////////////////入库
            if (list == null)
                return Error("导入失败");
            app.import(list);
            return Success("导入成功。");
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult getTeacherDetailInfo(string F_TeacherId)
        {
            var entity = app.GetForm(F_TeacherId);
            TeacherModel tea = new TeacherModel();
            if (entity != null)
            {
                tea.F_Id = entity.F_Id;
                tea.F_Name = entity.F_Name;
                tea.F_HeadPic = entity.teacherSysUser.F_HeadIcon;
                tea.F_Sex = entity.F_Gender;
                tea.F_Birthday = entity.F_Birthday;
                tea.F_Introduction = entity.F_Introduction;
                tea.F_MobilePhone = entity.F_MobilePhone;
                var stuhonor = new School_Teacher_Honor_App().GetFormByF_Teacher(F_TeacherId);
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
                tea.F_honorInfo = honor;
            }
            return Content(tea.ToJson());
        }

        public ActionResult GetDivisTeacher(string F_Divis_ID)
        {
            var datas = app.GetList(t => t.F_Divis_ID == F_Divis_ID);
            return Content(datas.ToJson());
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

        public class TeacherModel
        {
            public string F_Id { get; set; }
            public string F_Name { get; set; }
            public string F_Subject { get; set; }
            public string F_HeadPic { get; set; }
            public string F_Sex { get; set; }
            public DateTime? F_Birthday { get; set; }
            public string F_Introduction { get; set; }
            public List<Honor_Info> F_honorInfo { get; set; }
            public string F_MobilePhone { get; set; }
        }

        #endregion Model
    }
}