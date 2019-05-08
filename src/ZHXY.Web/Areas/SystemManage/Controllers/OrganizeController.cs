using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Domain;
using ZHXY.Common;

namespace ZHXY.Web.SystemManage.Controllers
{


    /// <summary>
    /// 机构管理
    /// </summary>
    public class OrganizeController : ZhxyWebControllerBase
    {
        private SysOrganizeAppService App { get; }

        public OrganizeController(SysOrganizeAppService app) => App = app;

        [HttpGet]
        public ActionResult GetSelectJson(string F_OrgId)
        {
            var list = new List<object>();
            var data = App.GetListByOrgId(F_OrgId);
            foreach (var item in data)
            {
                list.Add(new { id = item.F_Id, text = item.F_FullName });
            }
            return Content(list.ToJson());
        }

     

        [HttpGet]
        public ActionResult GetFullNameById(string F_Id)
        {
            var data = App.GetListById(F_Id);
            return Content(data.ToJson());
        }

        [HttpGet]
        public ActionResult GetTreeSelectJson()
        {
            var data = App.GetList();
            var treeList = new List<TreeSelectModel>();
            foreach (var item in data)
            {
                var treeModel = new TreeSelectModel
                {
                    id = item.F_Id,
                    text = item.F_FullName,
                    parentId = item.F_ParentId,
                    data = item
                };
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }

        /// <summary>
        /// 根据类别出机构下拉（非树状结构）
        /// </summary>
        /// <param name="parentId"> 父机构ID </param>
        /// <param name="keyword">  机构类别ID </param>
        /// <returns>  </returns>
        [HttpGet]
        public ActionResult GetSelectJsonByCategoryId(string keyword, string parentId)
        {
            List<Organize> data = App.GetList();
            if (!string.IsNullOrEmpty(keyword))
                data = data.Where(t => t.F_CategoryId == keyword).ToList();
            if (!parentId.IsEmpty())
                data = data.Where(t => t.F_ParentId == parentId).ToList();
            var list = new List<object>();
            foreach (var item in data)
            {
                list.Add(new { id = item.F_Id, text = item.F_FullName, template = item.F_Template, year = item.F_Year });
            }
            return Content(list.ToJson());
        }

        [HttpGet]
        public ActionResult GetTreeJson(string keyword)
        {
            var data = App.GetList();
            var data_deeps = "";
            var role = new SysRoleAppService().Get(keyword);
            if (!role.IsEmpty())
            {
                data_deeps = role.F_Data_Deps;
            }
            else
            {
                var user = new SysUserAppService().Get(keyword);
                if (!user.IsEmpty())
                    data_deeps = user.F_Data_Deps;
            }

            var treeList = new List<TreeViewModel>();
            foreach (var item in data)
            {
                var tree = new TreeViewModel();
                var hasChildren = data.Count(t => t.F_ParentId == item.F_Id) != 0;
                tree.id = item.F_Id;
                tree.text = item.F_FullName;
                tree.value = item.F_EnCode;
                tree.parentId = item.F_ParentId;
                tree.isexpand = true;
                tree.complete = false;
                tree.hasChildren = hasChildren;
                tree.showcheck = true;
                //tree.img = "";
                if (!data_deeps.IsEmpty() && (data_deeps.IndexOf(item.F_Id, StringComparison.Ordinal) != -1))
                    tree.checkstate = 1;
                else
                    tree.checkstate = 0;
                treeList.Add(tree);
            }
            return Content(treeList.TreeViewJson());
        }

        [HttpGet]
        public ActionResult GetTreeGridJson(string keyword, string divisId, string gradeId, string classId, string schoolId)
        {
            var data = App.GetList();
            if (!string.IsNullOrEmpty(keyword))
                data = data.TreeWhere(t => t.F_FullName.Contains(keyword));
            var treeList = new List<TreeGridModel>();
            foreach (var item in data)
            {
                var treeModel = new TreeGridModel();
                var hasChildren = data.Count(t => t.F_ParentId == item.F_Id) != 0;
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
        /// 获取学校
        /// </summary>
        /// <param name="keyword">  </param>
        /// <returns>  </returns>
        [HttpGet]
        public ActionResult GetSchoolOrGradeTreeGridJson(string keyword)
        {
            var data = App.GetList();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = data.TreeWhere(t => t.F_CategoryId.Contains(keyword));
            }
            var treeList = new List<TreeGridModel>();
            foreach (var item in data)
            {
                var treeModel = new TreeGridModel();
                var hasChildren = data.Count(t => t.F_ParentId == item.F_Id) != 0;
                treeModel.id = item.F_Id;
                treeModel.isLeaf = hasChildren;
                treeModel.parentId = item.F_ParentId;
                treeModel.expanded = false;
                treeModel.entityJson = item.ToJson();
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeGridJson());
        }

        
        [HttpGet]
        public JsonResult GetDivisGradeClass(string keyValue)
        {
            var DivsGradeClass = "";
            var Dataclass = App.GetById(keyValue);
            if (Dataclass == null) return Json(DivsGradeClass, JsonRequestBehavior.AllowGet);
            var Gradedata = Dataclass.Parent;
            if (Gradedata == null) return Json(DivsGradeClass, JsonRequestBehavior.AllowGet);
            var Divis = Gradedata.Parent;
            if (Divis != null)
            {
                DivsGradeClass = Divis.F_FullName + Gradedata.F_FullName + Dataclass.F_FullName;
            }
            return Json(DivsGradeClass, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = App.GetById(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Organize organizeEntity, string keyValue)
        {
            App.CreateOrUpdate(organizeEntity, keyValue);
            CacheFactory.Cache().RemoveCache(SmartCampusConsts.ORGANIZE);
            CacheFactory.Cache().WriteCache(SysCacheAppService.GetOrganizeList(), SmartCampusConsts.ORGANIZE);
            return Message("操作成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            App.Delete(keyValue);
            //new ClassAppService().DeleteByClassId(keyValue);
            CacheFactory.Cache().RemoveCache(SmartCampusConsts.ORGANIZE);
            CacheFactory.Cache().WriteCache(SysCacheAppService.GetOrganizeList(), SmartCampusConsts.ORGANIZE);
            return Message("删除成功。");
        }

        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Divis_Form() => View();

        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Class_Form() => View();

        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Grade_Form() => View();

        [HttpGet]
        public virtual ActionResult EditForm(string keyValue)
        {
            var org = App.GetById(keyValue);
            switch (org.F_CategoryId)
            {
                case "Division":
                    return View("Divis_Form");
                case "Grade":
                    return View("Grade_Form");
                case "Class":
                    return View("Class_Form");
                default:
                    return View("Form");
            }
        }

        [HttpGet]
        public virtual ActionResult EditDetails(string keyValue)
        {
            var org = App.GetById(keyValue);
            switch (org.F_CategoryId)
            {
                case "Division":
                    return View("Divis_Details");

                case "Grade":
                    return View("Grade_Details");

                case "Class":
                    return View("Class_Details");
                default:
                    return View("Details");
            }
        }

        [HttpGet]
        [HandlerAuthorize]
        public FileResult Export(string keyword, string F_DepartmentId, string F_Grade, string F_Class, string F_Year)
        {
            IDictionary<string, string> parms = new Dictionary<string, string>();

            var dbParameter = CreateParms(parms);

            var exportSql = CreateExportSql("Sys_Organize", parms);
            exportSql += " order by t.F_Id";
            var users = App.GetDataTable(App.DataScopeFilter(exportSql), dbParameter);

            var ms = new NPOIExcel().ToExcelStream(users, "组织列表");
            ms.Seek(0, SeekOrigin.Begin);
            var filename = "组织列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }

        [HttpPost]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Import(string filePath)
        {
            var list = ExcelToList<Organize>(Server.MapPath(filePath), "Sys_Organize");
            if (list == null)
                return Error("导入失败");
            App.Import(list);
            return Message("导入成功。");
        }

        /// <summary>
        /// 取到学部对应
        /// </summary>
        [HttpGet]
        public ActionResult GetOrgDics()
        {
            // 获取所有学部
            var orgList = App.GetList().Where(t => t.F_CategoryId == "Division").ToList();
            var orgs = new Dictionary<string, Dictionary<string, string[]>>();
            //入学年段
            var tmp = new Dictionary<string, string[]>();
            ////就读方式
            //Dictionary <string, string[]> F_SchoolType = new Dictionary<string, string[]>();
            ////来源类型
            //Dictionary<string, string[]> F_ComeFrom_Type = new Dictionary<string, string[]>();
            foreach (var org in orgList)
            {
                switch (org.F_EnCode)
                {
                    //精品小学
                    case "01":
                    case "04":
                        tmp = new Dictionary<string, string[]>();
                        tmp.Add("F_InYear", new string[] { "一年级", "二年级", "三年级", "四年级", "五年级", "六年级" });
                        tmp.Add("F_SchoolType", new string[] { "住校", "走读", "陪读" });
                        tmp.Add("F_ComeFrom_Type", new string[] { "校外转入", "校内直升", "无学籍" });
                        orgs.Add(org.F_Id, tmp);
                        break;

                    case "02":
                    case "05":
                        tmp = new Dictionary<string, string[]>();
                        tmp.Add("F_InYear", new string[] { "初一", "初二", "初三" });
                        tmp.Add("F_SchoolType", new string[] { "住校", "走读", "陪读" });
                        tmp.Add("F_ComeFrom_Type", new string[] { "校外转入", "校内直升" });
                        orgs.Add(org.F_Id, tmp);
                        break;

                    case "03":
                    case "06":
                        tmp = new Dictionary<string, string[]>();
                        tmp.Add("F_InYear", new string[] { "高一", "高二", "高三" });
                        tmp.Add("F_SchoolType", new string[] { "住校", "走读", "陪读" });
                        tmp.Add("F_ComeFrom_Type", new string[] { "校外转入", "校内直升" });
                        orgs.Add(org.F_Id, tmp);
                        break;

                    case "07":
                        tmp = new Dictionary<string, string[]>();
                        tmp.Add("F_InYear", new string[] { "小学", "初中", "高中" });
                        tmp.Add("F_SchoolType", new string[] { "住校", "走读", "陪读" });
                        tmp.Add("F_ComeFrom_Type", new string[] { "校外转入", "校内直升" });
                        orgs.Add(org.F_Id, tmp);
                        break;

                    case "08":
                        tmp = new Dictionary<string, string[]>();
                        tmp.Add("F_InYear", new string[] { "小一", "小二", "小三", "小四", "小五", "小六" });
                        tmp.Add("F_SchoolType", new string[] { "住校" });
                        tmp.Add("F_ComeFrom_Type", new string[] { "校外转入", "校内直升" });
                        orgs.Add(org.F_Id, tmp);
                        break;

                    case "11":
                    case "12":
                    case "13":
                    case "14":
                    case "15":
                    case "16":
                        tmp = new Dictionary<string, string[]>();
                        tmp.Add("F_InYear", new string[] { "小小班", "小班", "中班", "大班" });
                        tmp.Add("F_SchoolType", new string[] { "日托", "全日托", "周托", "全托" });
                        tmp.Add("F_ComeFrom_Type", new string[] { "校外转入", "无学籍" });
                        orgs.Add(org.F_Id, tmp);
                        break;
                }
            }
            return Content(orgs.ToJson());
        }



        /// <summary>
        /// 获取老师机构
        /// author:yujf
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetTeacherOrg(string nodeId="3",int n_level=0) => await Task.Run(() =>
        {
            var result = App.GetTeacherOrg(nodeId, n_level);
            return Resultaat.Success(result);
        });

        /// <summary>
        /// 获取老师机构
        /// author:yujf
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetStudentOrg(string nodeId = "2", int n_level = 0) => await Task.Run(() =>
        {
            var result = App.GetTeacherOrg(nodeId, n_level);
            return Resultaat.Success(result);
        });

        
    }
}