using NFine.Application;
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Code.Excel;
using NFine.Domain.Entity.SchoolManage;
using NFine.Domain.Entity.SystemManage;
using NFine.Web.Areas.SchoolManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace NFine.Web.Areas.SystemManage.Controllers
{
    public class OrganizeController : ControllerBase
    {
        private OrganizeApp organizeApp = new OrganizeApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetSelectJson(string F_OrgId)
        {
            //var data = organizeApp.GetList();
            //List<object> list = new List<object>();
            //foreach (OrganizeEntity item in data)
            //{
            //    list.Add(new { id = item.F_Id, text = item.F_FullName });
            //}
            List<object> list = new List<object>();
            var data = organizeApp.GetListByOrgId(F_OrgId);
            foreach (Organize item in data)
            {
                list.Add(new { id = item.F_Id, text = item.F_FullName });
            }
            return Content(list.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetCheckBoxJson(string tname, string keyword)
        {
            var data = organizeApp.GetList().Where(t => t.F_CategoryId == "Division");
            var checkedBoxs = new ComeBackRoute();
            var checkedBoxsTwo = new ComeBackRouteTwo();
            if (tname == "School_ComeBackRouteTwo")
                checkedBoxsTwo = new School_ComeBackRouteTwo_App().GetForm(keyword);
            else
                checkedBoxs = new School_ComeBackRoute_App().GetForm(keyword);
            List<CheckBoxSelectModel> list = new List<CheckBoxSelectModel>();
            foreach (Organize r in data)
            {
                CheckBoxSelectModel fieldItem = new CheckBoxSelectModel();
                fieldItem.value = r.F_Id;
                fieldItem.text = r.F_FullName;
                if (tname == "School_ComeBackRouteTwo")
                {
                    if (checkedBoxsTwo != null)
                    {
                        if (checkedBoxsTwo.F_Divis_ID.IndexOf(r.F_Id) != -1)
                            fieldItem.ifChecked = true;
                    }
                }
                else
                {
                    if (checkedBoxs != null)
                    {
                        if (checkedBoxs.F_Divis_ID.IndexOf(r.F_Id) != -1)
                            fieldItem.ifChecked = true;
                    }
                }
                list.Add(fieldItem);
            }
            return Content(list.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFullNameById(string F_Id)
        {
            var data = organizeApp.GetListById(F_Id);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeSelectJson()
        {
            var data = organizeApp.GetList();
            var treeList = new List<TreeSelectModel>();
            foreach (Organize item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.F_Id;
                treeModel.text = item.F_FullName;
                treeModel.parentId = item.F_ParentId;
                treeModel.data = item;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }

        /// <summary>
        /// 根据类别和父节点出机构下拉（树状结构）
        /// </summary>
        /// <param name="parentId">    </param>
        /// <param name="categoryId">  </param>
        /// <returns>  </returns>
        //[HttpGet]
        //[HandlerAjaxOnly]
        //public ActionResult GetTreeSelectJson(string parentId,string categoryId)
        //{
        //    var data = organizeApp.GetList();
        //    var treeList = new List<TreeSelectModel>();
        //    foreach (OrganizeEntity item in data)
        //    {
        //        TreeSelectModel treeModel = new TreeSelectModel();
        //        treeModel.id = item.F_Id;
        //        treeModel.text = item.F_FullName;
        //        treeModel.parentId = item.F_ParentId;
        //        treeModel.data = item;
        //        treeList.Add(treeModel);
        //    }
        //    return Content(treeList.TreeSelectJson());
        //}

        /// <summary>
        /// 根据类别出机构下拉（非树状结构）
        /// </summary>
        /// <param name="parentId"> 父机构ID </param>
        /// <param name="keyword">  机构类别ID </param>
        /// <returns>  </returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetSelectJsonByCategoryId(string keyword, string parentId)
        {
            var data = organizeApp.GetList().Where(t => t.F_CategoryId == keyword);
            if (!Ext.IsEmpty(parentId))
            {
                data = data.Where(t => t.F_ParentId == parentId);
            }
            List<object> list = new List<object>();
            foreach (Organize item in data)
            {
                list.Add(new { id = item.F_Id, text = item.F_FullName, template = item.F_Template, year = item.F_Year });
            }
            return Content(list.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeJson(string keyword)
        {
            var data = organizeApp.GetList();
            string data_deps = "";
            Role role = new RoleApp().GetForm(keyword);
            if (!Ext.IsEmpty(role))
            {
                data_deps = role.F_Data_Deps;
            }
            else
            {
                User user = new UserApp().GetForm(keyword);
                if (!Ext.IsEmpty(user))
                    data_deps = user.F_Data_Deps;
            }

            var treeList = new List<TreeViewModel>();
            foreach (Organize item in data)
            {
                TreeViewModel tree = new TreeViewModel();
                bool hasChildren = data.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
                tree.id = item.F_Id;
                tree.text = item.F_FullName;
                tree.value = item.F_EnCode;
                tree.parentId = item.F_ParentId;
                tree.isexpand = true;
                tree.complete = false;
                tree.hasChildren = hasChildren;
                tree.showcheck = true;
                //tree.img = "";
                if (!Ext.IsEmpty(data_deps) && (data_deps.IndexOf(item.F_Id) != -1))
                    tree.checkstate = 1;
                else
                    tree.checkstate = 0;
                treeList.Add(tree);
            }
            return Content(treeList.TreeViewJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeGridJson(string keyword)
        {
            var data = organizeApp.GetList();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.TreeWhere(t => t.F_FullName.Contains(keyword));
            }
            var treeList = new List<TreeGridModel>();
            foreach (var item in data)
            {
                TreeGridModel treeModel = new TreeGridModel();
                bool hasChildren = data.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
                treeModel.id = item.F_Id;
                treeModel.isLeaf = hasChildren;
                treeModel.parentId = item.F_ParentId;
                treeModel.expanded = false;
                treeModel.entityJson = item.ToJson();
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeGridJson());
        }

        private string GetLeaderTeacherNameByClassId(string classId)
        {
            var teacherId = new School_Class_Info_Teacher_App().Query(p => p.F_ClassID.Equals(classId) && !string.IsNullOrEmpty(p.F_Leader_Tea)).Select(p => p.F_Leader_Tea).FirstOrDefault();
            return new School_Teachers_App().Query(p => p.F_Id.Equals(teacherId) && !string.IsNullOrEmpty(p.F_Name)).Select(p => p.F_Name).FirstOrDefault();
        }

        private string GetLeaderTeacher2NameByClassId(string classId)
        {
            var teacherId = new School_Class_Info_Teacher_App().Query(p => p.F_ClassID.Equals(classId) && !string.IsNullOrEmpty(p.F_Leader_Tea2)).Select(p => p.F_Leader_Tea2).FirstOrDefault();
            return new School_Teachers_App().Query(p => p.F_Id.Equals(teacherId) && !string.IsNullOrEmpty(p.F_Name)).Select(p => p.F_Name).FirstOrDefault();
        }

        private string GetClassTeachersNameByClassId(string classId)
        {
            var teachesrId = new School_Class_Info_Teacher_App().Query(p => p.F_ClassID.Equals(classId) && !string.IsNullOrEmpty(p.F_Teacher)).Select(p => p.F_Teacher).ToArray();
            var arr = new School_Teachers_App().Query(p => teachesrId.Contains(p.F_Id) && !string.IsNullOrEmpty(p.F_Name)).Select(p => p.F_Name).ToArray();
            return string.Join(",", arr);
        }

        /// <summary>
        /// 获取学校
        /// </summary>
        /// <param name="keyword">  </param>
        /// <returns>  </returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetSchoolOrGradeTreeGridJson(string keyword)
        {
            var data = organizeApp.GetList();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.TreeWhere(t => t.F_CategoryId.Contains(keyword));
            }
            var treeList = new List<TreeGridModel>();
            foreach (Organize item in data)
            {
                TreeGridModel treeModel = new TreeGridModel();
                bool hasChildren = data.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
                treeModel.id = item.F_Id;
                treeModel.isLeaf = hasChildren;
                treeModel.parentId = item.F_ParentId;
                treeModel.expanded = false;
                treeModel.entityJson = item.ToJson();
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeGridJson());
        }

        /// <summary>
        /// 获取科目年级
        /// </summary>
        /// <param name="keyword">  </param>
        /// <returns>  </returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGradeTreeGridJson(string keyword)
        {
            var data = organizeApp.GetList();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.TreeWhere(t => t.F_CategoryId.Contains(keyword));
            }
            var treeList = new List<TreeGridModel>();
            foreach (Organize item in data)
            {
                var organizegrade = new OrganizeGrade();
                organizegrade = ExtObj.clonePropValueGrade(item, organizegrade);
                TreeGridModel treeModel = new TreeGridModel();
                bool hasChildren = data.Count(t => t.F_ParentId == organizegrade.F_Id) == 0 ? false : true;
                var GradeItem = new School_Grade_Course_App().GetFormByF_Grade(organizegrade.F_Id);
                string gradeid = "";
                foreach (var itemgrade in GradeItem)
                {
                    gradeid += new School_Course_App().GetForm(itemgrade.F_CourseId).F_Name + ",";
                }
                if (!string.IsNullOrEmpty(gradeid)) organizegrade.gradeid = gradeid.Substring(0, gradeid.Length - 1);
                else organizegrade.gradeid = gradeid;
                treeModel.id = organizegrade.F_Id;
                treeModel.isLeaf = hasChildren;
                treeModel.parentId = organizegrade.F_ParentId;
                treeModel.expanded = false;
                treeModel.entityJson = organizegrade.ToJson();
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeGridJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public JsonResult GetDivisGradeClass(string keyValue)
        {
            string DivisGradeClass = "";
            var Classdata = organizeApp.GetForm(keyValue);
            if (Classdata != null)
            {
                var Gradedata = organizeApp.GetForm(Classdata.F_ParentId);
                if (Gradedata != null)
                {
                    var Divis = organizeApp.GetForm(Gradedata.F_ParentId);
                    if (Divis != null)
                    {
                        DivisGradeClass = Divis.F_FullName + Gradedata.F_FullName + Classdata.F_FullName;
                    }
                }
            }
            return Json(DivisGradeClass, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = organizeApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Organize organizeEntity, string keyValue)
        {
            organizeApp.SubmitForm(organizeEntity, keyValue);
            CacheFactory.Cache().RemoveCache(Cons.ORGANIZE);
            CacheFactory.Cache().WriteCache(CacheConfig.GetOrganizeList(), Cons.ORGANIZE);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            organizeApp.DeleteForm(keyValue);
            CacheFactory.Cache().RemoveCache(Cons.ORGANIZE);
            CacheFactory.Cache().WriteCache(CacheConfig.GetOrganizeList(), Cons.ORGANIZE);
            return Success("删除成功。");
        }

        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Divis_Form()
        {
            return View();
        }

        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Class_Form()
        {
            return View();
        }

        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Grade_Form()
        {
            return View();
        }

        [HttpGet]
        public virtual ActionResult editForm(string keyValue)
        {
            Organize org = organizeApp.GetForm(keyValue);
            if ("Division".Equals(org.F_CategoryId))
                return View("Divis_Form");
            else if ("Grade".Equals(org.F_CategoryId))
                return View("Grade_Form");
            else if ("Class".Equals(org.F_CategoryId))
                return View("Class_Form");
            else
                return View("Form");
        }

        [HttpGet]
        public virtual ActionResult editDetails(string keyValue)
        {
            Organize org = organizeApp.GetForm(keyValue);
            if ("Division".Equals(org.F_CategoryId))
                return View("Divis_Details");
            else if ("Grade".Equals(org.F_CategoryId))
                return View("Grade_Details");
            else if ("Class".Equals(org.F_CategoryId))
                return View("Class_Details");
            else
                return View("Details");
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

            var dbParameter = CreateParms(parms);

            string exportSql = CreateExportSql("Sys_Organize", parms);
            exportSql += " order by t.F_Id";

            //string exportSql = "";
            //Console.WriteLine("exportSql==>" + exportSql);
            var users = organizeApp.GetDataTable(new BaseApp().dataScopeFilter(exportSql), dbParameter);
            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(users, "组织列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "组织列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }

        //导入excel
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult import(string filePath)
        {
            var list = ExcelToList<Organize>(Server.MapPath(filePath), "Sys_Organize");
            ///////////////////入库
            if (list == null)
                return Error("导入失败");
            organizeApp.import(list);
            return Success("导入成功。");
        }
    }
}