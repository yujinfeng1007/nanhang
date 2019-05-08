/*******************************************************************************
 * Author: mario
 * Description: School_ExamSignUp  Controller类
********************************************************************************/

using NFine.Application;
using NFine.Application.SystemManage;
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
    //考试报名表
    public class School_ExamSignUpController : ControllerBase
    {
        private School_ExamSignUp_App app = new School_ExamSignUp_App();
        private School_Exam_App examapp = new School_Exam_App();
        private School_No_Seed_App seedapp = new School_No_Seed_App();

        public ActionResult Auditor()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string F_Statu, string keyword, string F_Divis_ID, string F_Year, string F_ZK_Id, string F_CreatorTime_Start, string F_CreatorTime_Stop, string kszt)
        {
            var data = new
            {
                rows = app.GetList(pagination, F_Statu, keyword, F_Divis_ID, F_Year, F_ZK_Id, F_CreatorTime_Start, F_CreatorTime_Stop, kszt),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            if (string.IsNullOrEmpty(F_Statu) || F_Statu == "1")
            {
                List<object> list = new List<object>();
                foreach (ExamSignUp item in data.rows)
                {
                    //School_Exam_Entity examentity = examapp.GetForm(item.F_ZK_Id);
                    //int F_ValidDays = Convert.ToInt32(examentity.F_ValidDays);
                    //DateTime F_Time = Convert.ToDateTime(item.F_CreatorTime).AddHours(F_ValidDays);
                    if (DateTime.Now > item.F_Time)
                    {
                        //作废
                        if (F_Statu != "1") list.Add(item);
                    }
                    else
                    {
                        //未缴费
                        if (F_Statu == "1") list.Add(item);
                    }
                }
                return Content(list.ToJson());
            }
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetJsonByYR(string F_Year)
        {
            var data = app.GetByYR(F_Year);
            //for (int i = 0; i < data.Count(); i++)
            //{
            //    if (DateTime.Now > data[i].F_Time && data[i].F_Statu == "1")
            //    {
            //        data[i].F_Statu = "5";
            //    }
            //}
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = app.GetForm(keyValue);
            //if (DateTime.Now > data.F_Time && data.F_Statu == "1")
            //{
            //    data.F_Statu = "5";
            //}
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        //[ValidateAntiForgeryToken]
        //[AjaxValidateAntiForgeryToken]
        public ActionResult SubmitForm(ExamSignUp entity, string keyValue, int? Aud)
        {
            if (Aud != null)
            {
                if (app.GetFormNoTracking(keyValue).F_ExamDTM < DateTime.Now)
                {
                    throw new Exception("考试时间已过，请重新报考！");
                }
                entity.F_AuditDTM_Fst = DateTime.Now;
                entity.F_Auditor_Fst = OperatorProvider.Provider.GetCurrent().UserId;
            }
            if (string.IsNullOrEmpty(keyValue))
            {
                entity.F_ExamNum = seedapp.getStuExamNum(Convert.ToInt32(entity.F_Year), entity.F_Divis_ID);
            }
            entity.F_DepartmentId = OperatorProvider.Provider.GetCurrent().DepartmentId;
            string F_Id = app.SubmitForm(entity, keyValue);
            //if (F_Id=="true")
            //{
            //    throw new Exception("不在报名时间内，请重新选择考试！");
            //}
            return Success("操作成功。", F_Id);
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult KsPay(string keyValue)
        {
            try
            {
                ExamSignUp entity = app.GetFormNoTracking(keyValue);
                entity.F_Statu = "2"; //待审核
                entity.F_PayTime = DateTime.Now; //缴费时间
                entity.Modify(entity.F_Id);
                if (DateTime.Now > entity.F_ExamDTM)
                {
                    throw new Exception("考试时间已过，请重新报考！");
                }
                //List<School_ExamSignUp_Entity> stList = app.GetByName(entity.F_Name, entity.F_ZK_Id,entity.F_CredNum);
                //if (stList.Count > 0)
                //    for (int i = 0; i < stList.Count(); i++)
                //    {
                //        if (stList[i].F_Statu == "2" || stList[i].F_Statu == "3")
                //        {
                //            if (stList[i].F_Year == entity.F_Year)
                //            {
                //                throw new Exception("已有考试在审核！");
                //            }
                //        }
                //    }
                /////支付流程

                //数据库操作
                //var expression = ExtLinq.True<School_Bill_Entity>();
                //expression = expression.And(t => t.F_Exam == keyValue);
                //School_Bill_Entity bill = new School_Bill_App().GetList(expression).First();
                //app.KsPay(bill, entity);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

            return Success("缴费成功");
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
        public FileResult export(string keyword, string F_Statu, string F_Divis_ID, string F_Year, string F_ZK_Id, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            //参数 字段名->string[]{"F_Id",value}
            IDictionary<string, string> parms = new Dictionary<string, string>();
            //过滤条件
            if (!Ext.IsEmpty(F_Statu))
                parms.Add("F_Statu", F_Statu);
            if (!Ext.IsEmpty(keyword))
                parms.Add("F_Name", keyword);
            if (!Ext.IsEmpty(F_Divis_ID))
                parms.Add("F_Divis_ID", F_Divis_ID);
            if (!Ext.IsEmpty(F_Year))
                parms.Add("F_Year", F_Year);
            if (!Ext.IsEmpty(F_ZK_Id))
                parms.Add("F_ZK_Id", F_ZK_Id);

            DbParameter[] dbParameter = CreateParms(parms);

            string exportSql = CreateExportSql("School_ExamSignUp", parms);
            if (F_Statu == "1")
            {
                exportSql += " and '" + DateTime.Now + "'< F_Time";
            }
            if (string.IsNullOrEmpty(F_Statu))
            {
                exportSql += " and F_Statu = '1' and '" + DateTime.Now + "'> F_Time";
            }
            exportSql += " and t.F_DeleteMark != 'true' ";
            if (!string.IsNullOrEmpty(F_CreatorTime_Start))
            {
                DateTime CreatorTime_Start = Convert.ToDateTime(F_CreatorTime_Start + " 00:00:00");
                exportSql += " and t.F_CreatorTime >= '" + CreatorTime_Start + "'";
            }

            if (!string.IsNullOrEmpty(F_CreatorTime_Stop))
            {
                DateTime CreatorTime_Stop = Convert.ToDateTime(F_CreatorTime_Stop + " 23:59:59");
                exportSql += " and t.F_CreatorTime <= '" + CreatorTime_Stop + "'";
            }
            //string exportSql = "";
            //Console.WriteLine("exportSql==>" + exportSql);
            DataTable users = app.getDataTable(new BaseApp().dataScopeFilter(exportSql), dbParameter);
            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(users, "考试报名列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "考试报名列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
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
            //rules.Add("F_Id", new string[] { "报名ID", "" });
            //rules.Add("F_Divis_ID", new string[] { "学部ID", "" });
            //rules.Add("F_Grade_ID", new string[] { "年级ID", "" });
            //rules.Add("F_Class_ID", new string[] { "班级ID", "" });
            //rules.Add("F_Year", new string[] { "年度", "" });
            //rules.Add("F_Statu", new string[] { "报名状态", "" });
            //rules.Add("F_Students_ID", new string[] { "学生ID", "" });
            //rules.Add("F_Users_ID", new string[] { "学生用户ID", "" });
            //rules.Add("F_ExamNum", new string[] { "报名序号", "" });
            //rules.Add("F_ZK_Id", new string[] { "招考类型ID", "" });
            //rules.Add("F_RegUsers_ID", new string[] { "注册用户ID", "" });
            //rules.Add("F_RegUserType", new string[] { "注册用户类型", "" });
            //rules.Add("F_RegisterName", new string[] { "注册用户姓名", "" });
            //rules.Add("F_RegisterNum", new string[] { "注册用户账号", "" });
            //rules.Add("F_Link_Teacher_Name", new string[] { "联系老师姓名", "" });
            //rules.Add("F_Link_Teacher_Phone", new string[] { "联系老师电话", "" });
            //rules.Add("F_Parent_Name", new string[] { "家长姓名", "" });
            //rules.Add("F_Parent_Phone", new string[] { "家长电话", "" });
            //rules.Add("F_Total", new string[] { "报名费", "" });
            //rules.Add("F_Name", new string[] { "姓名", "" });
            //rules.Add("F_Gender", new string[] { "性别", "" });
            //rules.Add("F_Birthday", new string[] { "出生日期", "" });
            //rules.Add("F_Honor", new string[] { "曾获荣誉", "" });
            //rules.Add("F_FamilyAddr_Pro", new string[] { "家庭地址省", "" });
            //rules.Add("F_FamilyAddr_City", new string[] { "家庭地址市", "" });
            //rules.Add("F_FamilyAddr_Cou", new string[] { "家庭地址县", "" });
            //rules.Add("F_FamilyAddr", new string[] { "家庭详细地址", "" });
            //rules.Add("F_ComeFrom", new string[] { "来源学校", "" });
            //rules.Add("F_Auditor_Fst", new string[] { "学部审核人", "" });
            //rules.Add("F_AuditDTM_Fst", new string[] { "学部审核时间", "" });
            //rules.Add("F_Auditor_Fst_Remark", new string[] { "学部审核意见", "" });
            //rules.Add("F_SortCode", new string[] { "序号", "" });
            //rules.Add("F_DeleteMark", new string[] { "删除标记", "" });
            //rules.Add("F_EnabledMark", new string[] { "启用标记", "" });
            //rules.Add("F_CreatorTime", new string[] { "创建时间", "" });
            //rules.Add("F_CreatorUserId", new string[] { "创建者", "" });
            //rules.Add("F_LastModifyTime", new string[] { "修改时间", "" });
            //rules.Add("F_LastModifyUserId", new string[] { "修改者", "" });
            //rules.Add("F_DeleteTime", new string[] { "删除时间", "" });
            //rules.Add("F_DeleteUserId", new string[] { "删除者", "" });

            //////////////////处理数据(机构 岗位 等字典替换，过滤不要的字段，修改表头)
            List<ExamSignUp> list = ExcelToList<ExamSignUp>(Server.MapPath(filePath), rules);

            ///////////////////入库
            if (list == null)
                return Error("导入失败");
            app.import(list);
            return Success("导入成功。");
        }

        /// <summary>
        /// 批量生成准考证 pdf
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult getzkzPdf(string F_Statu, string keyword, string F_Divis_ID, string F_Year, string F_ZK_Id, string F_CreatorTime_Start, string F_CreatorTime_Stop, string kszt)
        {
            var data = app.GetList(null, F_Statu, keyword, F_Divis_ID, F_Year, F_ZK_Id, F_CreatorTime_Start, F_CreatorTime_Stop, kszt);
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            for (int i = 0; i < data.Count(); i++)
            {
                Dictionary<string, string> para = new Dictionary<string, string>();
                para.Add("F_StuPhoto", Server.MapPath(data[i].F_StuPhoto));
                para.Add("F_Date", DateTime.Now.ToString());
                para.Add("F_Title", data[i].School_Exam_Entity.F_Title);
                para.Add("F_StuTestNum", data[i].F_StuTestNum);
                para.Add("F_Name", data[i].F_Name);
                para.Add("F_Gender", data[i].F_Gender == "0" ? "女" : "男");
                para.Add("F_ComeFrom", data[i].F_ComeFrom);
                para.Add("F_Exam_Content", data[i].School_Exam_Entity.F_Exam_Content);
                para.Add("F_Link_Teacher_Name", data[i].F_Link_Teacher_Name);
                para.Add("F_Parent_Phone", data[i].F_Parent_Phone);
                para.Add("F_ExamNum", data[i].F_ExamNum);
                list.Add(para);
            }
            string modelFilePath = Server.MapPath(Configs.GetValue("modelPath2"));
            string modelFile = modelFilePath + "zkzpl.pdf";
            string descFile = GetPdf(list, modelFile, "ExamSignFree" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
            return Success("生成成功。");
        }

        [HttpGet]
        //[HandlerAjaxOnly]
        public ActionResult GetFormByExamNum(string F_ExamNum)
        {
            var data = app.GetFormByF_ExamNum(F_ExamNum);
            return Json(data);
        }
    }
}