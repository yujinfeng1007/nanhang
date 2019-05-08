/*******************************************************************************
 * Author: mario
 * Description: School_Course  Controller类
********************************************************************************/

using NFine.Code;
using NFine.Code.Excel;
using NFine.Domain.Entity.SchoolManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    //科目信息
    public class School_CourseController : ControllerBase
    {
        public ActionResult Teachers()
        {
            return View();
        }

        public ActionResult _FormOne()
        {
            return View();
        }

        public ActionResult _FormTwo()
        {
            return View();
        }

        private School_Course_App app = new School_Course_App();

        [HttpGet]
        //[HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            var datas = app.GetList(pagination, keyword);

            var treeList = new List<TreeGridModel>();
            foreach (var item in datas)
            {
                TreeGridModel treeModel = new TreeGridModel();
                bool hasChildren = datas.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
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
        [HandlerAjaxOnly]
        public ActionResult GetGridSelect()
        {
            var data = app.GetList();
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetCheckBoxJson(string tname, string keyword)
        {
            var data = app.GetList(t => !(t.F_CourseType == "4" && t.F_ParentId == "0"));
            var checkedBoxs = new School_Grade_Course_App().GetFormByF_Grade(keyword);
            List<CheckBoxSelectModel> list = new List<CheckBoxSelectModel>();
            foreach (Course r in data)
            {
                CheckBoxSelectModel fieldItem = new CheckBoxSelectModel();
                fieldItem.value = r.F_Id;
                fieldItem.text = r.F_Name;
                if (checkedBoxs.Count > 0)
                {
                    foreach (var item in checkedBoxs)
                    {
                        if (item.F_CourseId.IndexOf(r.F_Id) != -1)
                            fieldItem.ifChecked = true;
                    }
                }
                list.Add(fieldItem);
            }
            return Content(list.ToJson());
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
        public ActionResult SubmitForm(Course entity, string keyValue)
        {
            app.SubmitForm(entity, keyValue);
            // 如果是走班课，则加入课目A,课目B
            app.AddWalkCourse(entity, keyValue);
            CacheFactory.Cache().RemoveCache(Cons.COURSE);
            CacheFactory.Cache().WriteCache(CacheConfig.GetSchoolCourseList(), Cons.COURSE);
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
        public FileResult export(string keyword)
        {
            //参数 字段名->string[]{"F_Id",value}
            IDictionary<string, string> parms = new Dictionary<string, string>();
            //过滤条件
            if (!Ext.IsEmpty(keyword))
                parms.Add("F_RealName", keyword);

            DbParameter[] dbParameter = CreateParms(parms);

            string exportSql = CreateExportSql("School_Course", parms);
            //string exportSql = "";
            //Console.WriteLine("exportSql==>" + exportSql);
            DataTable dt = app.getDataTable(exportSql, dbParameter);
            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(dt, "科目列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "科目列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }

        //导入excel
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult import(string filePath)
        {
            var list = ExcelToList<Course>(Server.MapPath(filePath), "School_Course");
            if (list == null)
                return Error("导入失败");
            app.import(list);
            return Success("导入成功。");
        }
    }
}