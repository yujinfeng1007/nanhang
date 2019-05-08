/*******************************************************************************
 * Author: mario
 * Description: School_RollChangeList  Controller类
********************************************************************************/

using NFine.Application.SchoolManage;
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Code.Excel;
using NFine.Domain.Entity.SchoolManage;
using NFine.Domain.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    //记录费用临时调整
    public class School_RollChangeListController : ControllerBase
    {
        public ActionResult Auditor()
        {
            return View();
        }

        private School_RollChangeList_App app = new School_RollChangeList_App();

        /// <summary>
        /// </summary>
        /// <param name="pagination">      </param>
        /// <param name="keyword">         </param>
        /// <param name="F_Auditor_Name">  </param>
        /// <param name="F_Change_Type">   </param>
        /// <param name="F_Status">        </param>
        /// <param name="F_Student_Id">    </param>
        /// <param name="xjzt">           学籍状态 </param>
        /// <returns>  </returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_Auditor_Name, string F_Change_Type, string F_Status, string F_Student_Id, string xjzt)
        {
            var data = new
            {
                rows = app.GetList(pagination, keyword, F_Auditor_Name, F_Change_Type, F_Status, F_Student_Id, xjzt),
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
        public ActionResult SubmitForm(RollChangeList entity, string keyValue)
        {
            Student stu = new School_Students_App().GetForm(entity.F_Student_Id);
            EntrySignUp entry = new School_EntrySignUp_App().GetFormByF_InitNumI(stu.F_InitNum);
            if (entry.F_Statu == "7")
            {
                throw new Exception("当前协议正在调整中，请稍后操作！");
            }
            entity.F_Student_Id = stu.F_Id;
            entity.F_Student_Name = stu.F_Name;
            entity.F_Student_Num = stu.F_StudentNum;
            entity.F_Old_Subjects_ID = stu.F_Subjects_ID;
            entity.F_Sign_Id = entry.F_Id;
            entity.F_Sign_Id_Statu = entry.F_Statu;
            entry.F_Statu = "8";
            var expression = ExtLinq.True<RollChangeList>();
            expression = expression.And(t => t.F_Student_Id == entity.F_Student_Id);
            expression = expression.And(t => t.F_Status == "4");
            var datas = app.GetList(expression);
            if (datas.Count() > 0)
            {
                throw new Exception("已有申请信息正在审核中！");
            }
            app.SubmitForm(entity, keyValue, entry);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult AuditorForm(RollChangeList entity, string keyValue)
        {
            Student stu = new School_Students_App().GetFormNoTracking(entity.F_Student_Id);
            EntrySignUp entry = new School_EntrySignUp_App().GetFormByF_InitNumI(stu.F_InitNum);
            EntrySignUp ent = new School_EntrySignUp_App().GetFormByF_InitNum(stu.F_InitNum);
            User user = new UserApp().GetForm(stu.F_Users_ID);
            //entry.F_Statu = "8";
            // 审核状态不为空
            //int i = 0;
            entity.F_Id = keyValue;
            entity.F_Audit_Time = DateTime.Now;
            entity.F_Auditor = OperatorProvider.Provider.GetCurrent().UserId;
            entity.F_Auditor_Name = OperatorProvider.Provider.GetCurrent().UserName;

            var data = app.GetFormI(keyValue);
            entry.F_Statu = data.F_Sign_Id_Statu;
            // 审核通过
            if (entity.F_Status == "5")
            {
                if (data.F_Change_Type == "1" || data.F_Change_Type == "2" || data.F_Change_Type == "4" || data.F_Change_Type == "5" || data.F_Change_Type == "6" || data.F_Change_Type == "7" || data.F_Change_Type == "8")
                {
                    stu.F_CurStatu = "4";
                    entry.F_Statu = "3";
                }
                else if (data.F_Change_Type == "3")
                {
                    stu.F_CurStatu = "6";
                }
                else if (data.F_Change_Type == "12")
                {
                    stu.F_CurStatu = "7";
                }
                else if (data.F_Change_Type == "13")
                {
                    stu.F_CurStatu = "5";
                    entry.F_Statu = "3";
                }
                else if (data.F_Change_Type == "9" || data.F_Change_Type == "10" || data.F_Change_Type == "11")
                {
                    stu.F_CurStatu = "1";
                    entry.F_Id = null;
                    entry.F_Year = Convert.ToInt32(data.F_Year);
                    entry.F_Divis_ID = data.F_Divis_ID;
                    entry.F_Grade_ID = data.F_Grade_ID;
                    //entry.F_Class_ID = data.F_Class_ID;
                    entry.F_Charge_mode = data.F_Charge_Type;
                    entry.F_Subjects_ID = data.F_New_Subjects_ID;
                    entry.F_INYear = data.F_INYear;
                    entry.F_InitNum = new School_No_Seed_App().getStuInNum(Convert.ToInt32(data.F_Year), data.F_Divis_ID);
                    entry.F_Statu = "4";
                    ent.F_Statu = "3";
                    //i = 1;
                    stu.F_Year = data.F_Year;
                    stu.F_Divis_ID = data.F_Divis_ID;
                    stu.F_Grade_ID = data.F_Grade_ID;
                    //stu.F_Charge_mode = data.F_Charge_Type;
                    stu.F_Subjects_ID = data.F_New_Subjects_ID;
                    stu.F_INYear = data.F_INYear;
                    stu.F_Class_ID = data.F_Class_ID;
                    stu.F_InitNum = entry.F_InitNum;
                    stu.F_DepartmentId = stu.F_Grade_ID;
                    user.F_DepartmentId = stu.F_Grade_ID;
                    //entry.F_Statu = "3";
                }
                //else if (data.F_Change_Type == "10")
                //{
                //    stu.F_CurStatu = "1";
                //    //entry.F_Statu = "3";
                //}
                //else if (data.F_Change_Type == "11")
                //{
                //    stu.F_CurStatu = "1";
                //    //entry.F_Statu = "3";
                //}
            }
            app.AuditorForm(entity, stu, entry, ent, user);
            //if (i == 1)
            //{
            //    new School_EntrySignUp_App().UpdateForm(ent);
            //}
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

            string exportSql = CreateExportSql("School_RollChangeList", parms);
            //string exportSql = "";
            //Console.WriteLine("exportSql==>" + exportSql);
            DataTable dt = app.getDataTable(exportSql, dbParameter);
            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(dt, "School_RollChangeList列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "School_RollChangeList列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
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
            //rules.Add("F_Id", new string[] { "F_Id", "" });
            //rules.Add("F_Student_Num", new string[] { "学号", "" });
            //rules.Add("F_Student_Id", new string[] { "学生", "" });
            //rules.Add("F_Sign_Id", new string[] { "协议", "" });
            //rules.Add("F_Old_Subjects_ID", new string[] { "原班级类型", "" });
            //rules.Add("F_New_Subjects_ID", new string[] { "新班级类型", "" });
            //rules.Add("F_Change_Type", new string[] { "调整类型", "" });
            //rules.Add("F_Divis_ID", new string[] { "变动学部", "" });
            //rules.Add("F_Grade_ID", new string[] { "变动年级", "" });
            //rules.Add("F_Class_ID", new string[] { "变动班级", "" });
            //rules.Add("F_Proof", new string[] { "调整凭证", "" });
            //rules.Add("F_Proof_Desc", new string[] { "调整原因", "" });
            //rules.Add("F_Bill_ChangeNo", new string[] { "账单变更操作编号", "" });
            //rules.Add("F_Bill_No", new string[] { "变更账单编号", "" });
            //rules.Add("F_Auditor", new string[] { "审核人", "" });
            //rules.Add("F_Auditor_Name", new string[] { "审核人姓名", "" });
            //rules.Add("F_Status", new string[] { "审核状态", "" });
            //rules.Add("F_Audit_Memo", new string[] { "审核意见", "" });
            //rules.Add("F_Creator_Name", new string[] { "申请人姓名", "" });
            //rules.Add("F_Audit_Time", new string[] { "审核时间", "" });
            //rules.Add("F_SortCode", new string[] { "序号", "" });
            //rules.Add("F_DepartmentId", new string[] { "所属部门", "" });
            //rules.Add("F_DeleteMark", new string[] { "删除标记", "" });
            //rules.Add("F_EnabledMark", new string[] { "启用标记", "" });
            //rules.Add("F_CreatorTime", new string[] { "申请时间", "" });
            //rules.Add("F_CreatorUserId", new string[] { "申请者", "" });
            //rules.Add("F_LastModifyTime", new string[] { "修改时间", "" });
            //rules.Add("F_LastModifyUserId", new string[] { "修改者", "" });
            //rules.Add("F_DeleteTime", new string[] { "删除时间", "" });
            //rules.Add("F_DeleteUserId", new string[] { "删除者", "" });
            //rules.Add("F_Year_Period", new string[] { "缴费学期", "" });

            //////////////////处理数据(机构 岗位 等字典替换，过滤不要的字段，修改表头)
            List<RollChangeList> list = ExcelToList<RollChangeList>(Server.MapPath(filePath), rules);

            ///////////////////入库
            if (list == null)
                return Error("导入失败");
            app.import(list);
            return Success("导入成功。");
        }
    }
}