/*******************************************************************************
 * Author: mario
 * Description: School_ParStu_Num  Controller类
********************************************************************************/

using NFine.Application.SchoolManage;
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Code.Excel;
using NFine.Domain.Entity.SchoolManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    //家长与学生关系表
    public class School_ParStu_NumController : ControllerBase
    {
        private School_ParStu_Num_App app = new School_ParStu_Num_App();

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
        public ActionResult SubmitForm(ParentStudent entity, string keyValue)
        {
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

            string exportSql = CreateExportSql("School_ParStu_Num", parms);
            //string exportSql = "";
            //Console.WriteLine("exportSql==>" + exportSql);
            DataTable dt = app.getDataTable(exportSql, dbParameter);
            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(dt, "School_ParStu_Num列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "School_ParStu_Num列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }

        //导入excel
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult import(string filePath)
        {
            //////////////////定义规则：字段名，表头名称，字典
            //字段名->string[]{表头,字典}，若是一般字段 字典为空字符串
            IDictionary<string, string[]> rules = new Dictionary<string, string[]>();
            //rules.Add("F_Id", new string[] { "编号", "" });
            //rules.Add("F_RealName", new string[] { "姓名", "" });
            //rules.Add("F_Gender", new string[] { "性别", "104" });
            //rules.Add("F_OrganizeId", new string[] { "公司", "F_OrganizeId" });
            //rules.Add("F_DepartmentId", new string[] { "部门", "F_DepartmentId" });
            //rules.Add("F_AreaId", new string[] { "地区", "F_AreaId" });
            //rules.Add("F_RoleId", new string[] { "角色", "F_RoleId" });
            //rules.Add("F_DutyId", new string[] { "岗位", "F_DutyId" });
            //rules.Add("F_CreatorTime", new string[] { "创建时间", "" });
            //rules.Add("F_HeadIcon", new string[] { "头像", "" });

            //所有字段代码
            //rules.Add("F_Id", new string[] { "Id", "" });
            //rules.Add("F_ParentId", new string[] { "家长Id", "" });
            //rules.Add("F_Parent_Phone", new string[] { "家长手机", "" });
            //rules.Add("F_ParentName", new string[] { "家长姓名", "" });
            //rules.Add("F_Parent_CardNo", new string[] { "家长证件号", "" });
            //rules.Add("F_Stu_Num", new string[] { "学号", "" });
            //rules.Add("F_Stu_Id", new string[] { "学生Id", "" });
            //rules.Add("F_Stu_Name", new string[] { "学生姓名", "" });
            //rules.Add("F_SortCode", new string[] { "序号", "" });
            //rules.Add("F_DepartmentId", new string[] { "所属部门", "" });
            //rules.Add("F_DeleteMark", new string[] { "删除标记", "" });
            //rules.Add("F_EnabledMark", new string[] { "启用标记", "" });
            //rules.Add("F_CreatorTime", new string[] { "创建时间", "" });
            //rules.Add("F_CreatorUserId", new string[] { "创建者", "" });
            //rules.Add("F_LastModifyTime", new string[] { "修改时间", "" });
            //rules.Add("F_LastModifyUserId", new string[] { "修改者", "" });
            //rules.Add("F_DeleteTime", new string[] { "删除时间", "" });
            //rules.Add("F_DeleteUserId", new string[] { "删除者", "" });

            //////////////////处理数据(机构 岗位 等字典替换，过滤不要的字段，修改表头
            List<ParentStudent> list = ExcelToList<ParentStudent>(Server.MapPath(filePath), rules);

            ///////////////////入库
            if (list == null)
                return Error("导入失败");
            app.import(list);
            return Success("导入成功。");
        }

        /// <summary>
        /// 家长微信端绑定学生 F_CardNo 证件号码
        /// </summary>
        /// <returns>  </returns>
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult bindStudent(string F_StudentName, string F_CardNo)
        {
            //家长
            OperatorModel parent = OperatorProvider.Provider.GetCurrent();
            if (Ext.IsEmpty(parent))
            {
                return Error("没有这个家长");
            }
            if ("student".Equals(parent.Duty))
            {
                return Error("没有这个家长");
            }
            //学生
            Student student = new School_Students_App().GetFormByNameAndCard(F_StudentName, F_CardNo);
            if (Ext.IsEmpty(student))
            {
                return Error("没有这个学生");
            }
            //绑定逻辑
            else
            {
                ParentStudent e = new ParentStudent();
                e.F_ParentId = parent.UserId;
                e.F_ParentName = parent.UserName;
                e.F_Parent_CardNo = string.Empty;
                e.F_Parent_Phone = parent.MobilePhone;
                e.F_Stu_Id = student.F_Id;
                //e.F_Stu_Name = student.F_Name;
                e.F_Stu_Num = student.F_StudentNum;
                try
                {
                    var stu = new School_Students_App().GetForm(e.F_Stu_Id);
                    var user = new UserApp().GetForm(stu.F_Users_ID);
                    var data = new
                    {
                        F_Stu_Id = e.F_Stu_Id,
                        studentName = student.F_Name,
                        studentNum = e.F_Stu_Num,
                        schoolName = OperatorProvider.Provider.GetCurrent().SchoolName,
                        headPic = user.F_HeadIcon,
                        className = new OrganizeApp().GetForm(stu.F_DepartmentId).F_FullName,
                        classId = stu.F_DepartmentId
                    };

                    app.SubmitForm(e, null);
                    return Json(new { data = data, state = ResultType.success.ToString(), message = "绑定成功。" });
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            //return Success("绑定成功。");
        }

        /// <summary>
        /// 家长微信端设置头像
        /// </summary>
        /// <returns>  </returns>
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult setParentIcon(string parentId, string filePath)
        {
            try
            {
                if (app.SetUserIcon(parentId, filePath))
                    return Success("操作成功");
                else
                    return Error("操作失败");
            }
            catch (Exception ex)
            {
                FileLog.Error(ex.Message, ex);
                return Success("操作成功");
            }
        }
    }
}