/*******************************************************************************
 * Author: mario
 * Description: School_Bill  Controller类
********************************************************************************/
using NFine.Application.SchoolManage;
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Code.Excel;
using NFine.Domain.Entity.SchoolManage;
using NFine.Domain.Entity.SystemManage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    //学生缴费账单
    public class School_BillController : ControllerBase
    {
        public ActionResult NewForm()
        {
            return View();
        }
        public ActionResult ChangeForm()
        {
            return View();
        }

        public ActionResult TKIndex()
        {
            return View();
        }
        public ActionResult IndexCastration()
        {
            return View();
        }

        private School_Bill_App app = new School_Bill_App();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_Tax_Statu, string F_Charge_Status, string F_Tax_Num, string F_CreatorTime_Start, string F_CreatorTime_Stop, string F_StudentNum)
        {
            var data = new
            {
                rows = app.GetList(pagination, keyword, F_Tax_Statu, F_Charge_Status, F_Tax_Num, F_CreatorTime_Start, F_CreatorTime_Stop, F_StudentNum),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }


        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetListWhere(string F_StudentNum, string F_Fees_Name)
        {
            var data = app.GetList(F_StudentNum, F_Fees_Name);
            return Content(data.ToJson());
        }

        /// <summary>
        /// 手机端接口
        /// </summary>
        /// <param name="keyword">学号</param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetMobileGridJson(string keyValue)
        {
            return Content(app.GetMobileList(keyValue).ToJson());
        }

        /// <summary>
        /// 手机端接口-详情页
        /// </summary>
        /// <param name="keyword">账单ID</param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetMobileFormJson(string keyValue)
        {
            var data = app.GetMobileForm(keyValue);
            return Content(data.ToJson());
        }

        /// <summary>
        /// 手机端接口-获取所有欠款总额
        /// </summary>
        /// <param name="keyValue">学生学号</param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult sumBill(string keyValue)
        {
            var data = app.sumBill(keyValue);
            return Content(data.ToJson());
        }

        /// <summary>
        /// 农行调用接预支付,创建对账流水
        /// </summary>
        /// <param name="keyValue">学生学号</param>
        /// <param name="payType">"1" 保额金 "2" 学杂费 "3" 考试费</param>
        /// <param name="billId">xyid</param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult LhBillSearch(string keyValue)
        {

            return Content("");
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


        /// <summary>
        /// 单个账单支付
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        //[ValidateAntiForgeryToken]
        public ActionResult Pay(string keyValue)
        {
            try
            {
                //网银缴费
                SchoolBill bill = app.GetForm(keyValue);
                app.Pay(bill);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }

        /// <summary>
        /// 所有账单支付账单支付
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        //[HttpGet]
        //[HandlerAjaxOnly]
        //[ValidateAntiForgeryToken]
        //public ActionResult PayAll()
        //{
        //    try
        //    {
        //        //网银缴费
        //        School_Bill_Entity bill = app.GetForm(keyValue);
        //        app.Pay(bill);
        //        return Success("操作成功。");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Error(ex.Message);
        //    }

        //}


        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SchoolBill entity, string keyValue)
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

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult ChargeForm(string keyValue)
        {
            app.UpdateForm(keyValue);
            return Success("删除成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult CancelForm(string keyValue)
        {
            app.CancelForm(keyValue);
            return Success("撤销成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        public ActionResult cancelXYSBill(string F_ChargeNo)
        {
            app.cancelXYSBill(F_ChargeNo);
            return Success("撤销成功。");
        }

        //导出excel
        [HttpGet]
        [HandlerAuthorize]
        public FileResult export(string keyword, string F_Tax_Statu, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            //参数 字段名->string[]{"F_Id",value}
            IDictionary<string, string> parms = new Dictionary<string, string>();
            //过滤条件
            if (!Ext.IsEmpty(F_Tax_Statu))
                parms.Add("F_Tax_Statu", F_Tax_Statu);

            DbParameter[] dbParameter = CreateParms(parms);

            string exportSql = CreateExportSql("School_Bill", parms);
            if (!Ext.IsEmpty(keyword))
            {
                exportSql += " and t.F_StudentNum like '%" + keyword + "%' or t.F_ChargeNo like '%" + keyword + "%' ";
            }
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
            DataTable dt = app.getDataTable(exportSql, dbParameter);
            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(dt, "School_Bill列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "School_Bill列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }


        //导入excel
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult import(string filePath)
        {
            ////////////////////定义规则：字段名，表头名称，字典
            ////字段名->string[]{表头,字典}，若是一般字段 字典为空字符串
            //IDictionary<string, string[]> rules = new Dictionary<string, string[]>();
            ////rules.Add("F_Id", new string[] { "编号", "" });
            ////rules.Add("F_RealName", new string[] { "姓名", "" });
            ////rules.Add("F_Gender", new string[] { "性别", "104" });
            ////rules.Add("F_OrganizeId", new string[] { "公司", "F_OrganizeId" });
            ////rules.Add("F_DepartmentId", new string[] { "部门", "F_DepartmentId" });
            ////rules.Add("F_AreaId", new string[] { "地区", "F_AreaId" });
            ////rules.Add("F_RoleId", new string[] { "角色", "F_RoleId" });
            ////rules.Add("F_DutyId", new string[] { "岗位", "F_DutyId" });
            ////rules.Add("F_CreatorTime", new string[] { "创建时间", "" });
            ////rules.Add("F_HeadIcon", new string[] { "头像", "" });

            ////所有字段代码
            //            //rules.Add("F_Id", new string[] { "ID", "" });
            //            //rules.Add("F_TollFlow_Id", new string[] { "流水ID", "" });
            //            //rules.Add("F_ChargeNo", new string[] { "账单序号", "" });
            //            //rules.Add("F_Charge_Type", new string[] { "收费类别", "" });
            //            //rules.Add("F_Student_ID", new string[] { "学生ID", "" });
            //            //rules.Add("F_StudentNum", new string[] { "学号", "" });
            //            //rules.Add("F_Divis_ID", new string[] { "学部ID", "" });
            //            //rules.Add("F_Grade_ID", new string[] { "年级ID", "" });
            //            //rules.Add("F_Class_ID", new string[] { "班级ID", "" });
            //            //rules.Add("F_Subjects_ID", new string[] { "班级类型ID", "" });
            //            //rules.Add("F_Exam_ID", new string[] { "招考类型ID", "" });
            //            //rules.Add("F_Year", new string[] { "缴费年度", "" });
            //            //rules.Add("F_Year_Period", new string[] { "缴费学期", "" });
            //            //rules.Add("F_Charge_Status", new string[] { "缴费状态", "" });
            //            //rules.Add("F_Fees", new string[] { "收费项目", "" });
            //            //rules.Add("F_Fees_Name", new string[] { "收费项目名称", "" });
            //            //rules.Add("F_Fees_Desc", new string[] { "收费内容", "" });
            //            //rules.Add("F_Users_ID", new string[] { "付款人ID", "" });
            //            //rules.Add("F_Users_Name", new string[] { "付款人姓名", "" });
            //            //rules.Add("F_BankCode", new string[] { "付款银行编码", "" });
            //            //rules.Add("F_BankName", new string[] { "付款银行名称", "" });
            //            //rules.Add("F_PayType", new string[] { "支付方式", "" });
            //            //rules.Add("F_PayNum", new string[] { "支付凭证", "" });
            //            //rules.Add("F_Toll_Account", new string[] { "收款账户", "" });
            //            //rules.Add("F_Toll_DTM", new string[] { "收款时间", "" });
            //            //rules.Add("F_Toll_Statu", new string[] { "到帐状态", "" });
            //            //rules.Add("F_Confirm_Statu", new string[] { "确认状态", "" });
            //            //rules.Add("F_Auditor", new string[] { "审核人", "" });
            //            //rules.Add("F_AuditDTM", new string[] { "审核时间", "" });
            //            //rules.Add("F_ARInvoice_ID", new string[] { "发票ID", "" });
            //            //rules.Add("F_In_Pay", new string[] { "应缴", "" });
            //            //rules.Add("F_Pay", new string[] { "实缴", "" });
            //            //rules.Add("F_Not_Pay", new string[] { "欠缴", "" });
            //            //rules.Add("F_SortCode", new string[] { "序号", "" });
            //            //rules.Add("F_DepartmentId", new string[] { "所属部门", "" });
            //            //rules.Add("F_DeleteMark", new string[] { "删除标记", "" });
            //            //rules.Add("F_EnabledMark", new string[] { "启用标记", "" });
            //            //rules.Add("F_CreatorTime", new string[] { "创建时间", "" });
            //            //rules.Add("F_CreatorUserId", new string[] { "创建者", "" });
            //            //rules.Add("F_LastModifyTime", new string[] { "修改时间", "" });
            //            //rules.Add("F_LastModifyUserId", new string[] { "修改者", "" });
            //            //rules.Add("F_DeleteTime", new string[] { "删除时间", "" });
            //            //rules.Add("F_DeleteUserId", new string[] { "删除者", "" });

            ////////////////////处理数据(机构 岗位 等字典替换，过滤不要的字段，修改表头)
            //List<School_Bill_Entity> list = ExcelToList<School_Bill_Entity>(Server.MapPath(filePath), rules);

            var list = ExcelToList<SchoolBill>(Server.MapPath(filePath), "School_Bill");
            ///////////////////入库
            if (list == null)
                return Error("导入失败");
            app.import(list);
            return Success("导入成功。");
        }

        /// <summary>
        /// 生成缴费凭证 pdf
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        public virtual JsonResult getjfpzPdfs(string F_Id)
        {
            SchoolBill bill = app.GetForm(F_Id);
            var data = new School_Bill_Voucher_App().GetFormByF_Charge_ID(F_Id, "", "");
            if (data == null)
            {
                BillVoucher vouentity = new BillVoucher();
                vouentity.F_Bill_Num = bill.F_ChargeNo;
                vouentity.F_Charge_ID = bill.F_Id;
                vouentity.F_Class_ID = bill.F_Class_ID;
                vouentity.F_CredNum = bill.F_Charge_Type == "考试报名费" ? bill.School_ExamSignUp_Entity.F_CredNum : bill.School_EntrySignUp_Entity.F_CredNum;
                vouentity.F_Divis_ID = bill.F_Divis_ID;
                vouentity.F_Exam = bill.F_Exam;
                vouentity.F_Exam_No = bill.F_Exam_No;
                vouentity.F_Grade_ID = bill.F_Grade_ID;
                vouentity.F_In = bill.F_In;
                vouentity.F_InvoiceDesc = bill.F_Year + "学年" + new OrganizeApp().GetForm(bill.F_Divis_ID).F_FullName + "学部" + " " + bill.F_Charge_Type + "" + bill.F_Pay + "元";
                vouentity.F_In_No = bill.F_In_No;
                vouentity.F_Money = CmycurD(bill.F_Pay.ToString()) + "  ￥" + bill.F_Pay.ToString() + "元";
                vouentity.F_Name = bill.F_Charge_Type == "考试报名费" ? bill.School_ExamSignUp_Entity.F_Name : bill.School_EntrySignUp_Entity.F_Name;

                //Dictionary<string, object> dataitems = CacheFactory.Cache().GetCache<Dictionary<string, object>>(Cons.DATAITEMS);
                //string F_PayType = "";
                //object dic = new Dictionary<string, string>();
                //if (bill.F_PayType != null && dataitems.TryGetValue("pay_type", out dic))
                //{
                //    ((Dictionary<string, string>)dic).TryGetValue(bill.F_PayType, out F_PayType);
                //}

                vouentity.F_PayType = bill.F_PayType;
                vouentity.F_StudentNum = bill.F_Charge_Type == "考试报名费" ? bill.School_ExamSignUp_Entity.F_StuTestNum : bill.School_EntrySignUp_Entity.F_StudentNum;
                vouentity.F_Students_ID = bill.F_Charge_Type != "考试报名费" ? bill.School_EntrySignUp_Entity.F_Students_ID : "";
                vouentity.F_Voucher_Num = new School_No_Seed_App().getVoucherNum();
                vouentity.F_Toll_DTM = bill.F_Toll_DTM;
                new School_Bill_Voucher_App().SubmitForm(vouentity, "");
            }
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            Dictionary<string, string> para = new Dictionary<string, string>();
            if (bill.F_Charge_Status != "2")
            {
                throw new Exception("当前费用还未缴清！");
            }
            string files = "学杂费专用收款收据(修改)（表单）.pdf";
            string newname = "xzjf" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
            var datas = new School_Bill_Voucher_App().GetFormByF_Charge_ID(F_Id, "", "");
            if (!string.IsNullOrEmpty(datas.F_Exam))
            {
                files = "考试费专用收款收据(修改)（表单）.pdf";
                newname = "ksjf" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
            }
            para.Add("F_StudentNum", datas.F_StudentNum);
            para.Add("F_Voucher_Num", datas.F_Voucher_Num);
            para.Add("F_Name", datas.F_Name);
            para.Add("F_CredNum", datas.F_CredNum);
            para.Add("F_PayType", datas.F_PayType);
            para.Add("F_Date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            para.Add("F_Toll_DTM", datas.F_Toll_DTM.ToDateTimeString());
            para.Add("F_InvoiceDesc", datas.F_InvoiceDesc);
            para.Add("F_DPay", datas.F_Money);
            list.Add(para);
            string modelFilePath = Server.MapPath(Configs.GetValue("modelPath2"));
            string modelFile = modelFilePath + files;
            string descFile = GetPdf(list, modelFile, newname);
            return Json(new { message = "生成成功。", filpath = Configs.GetValue("modelPath2") + newname, filename = newname }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        public virtual JsonResult getjfpzPdf(string F_Id)
        {
            SchoolBill bill = app.GetForm(F_Id);
            var data = new School_Bill_Voucher_App().GetFormByF_Charge_ID(F_Id, "", "");
            if (data == null)
            {
                BillVoucher vouentity = new BillVoucher();
                vouentity.F_Bill_Num = bill.F_ChargeNo;
                vouentity.F_Charge_ID = bill.F_Id;
                vouentity.F_Class_ID = bill.F_Class_ID;
                vouentity.F_CredNum = bill.F_Charge_Type == "考试报名费" ? bill.School_ExamSignUp_Entity.F_CredNum : bill.School_EntrySignUp_Entity.F_CredNum;
                vouentity.F_Divis_ID = bill.F_Divis_ID;
                vouentity.F_Exam = bill.F_Exam;
                vouentity.F_Exam_No = bill.F_Exam_No;
                vouentity.F_Grade_ID = bill.F_Grade_ID;
                vouentity.F_In = bill.F_In;
                vouentity.F_InvoiceDesc = bill.F_Year + "学年" + new OrganizeApp().GetForm(bill.F_Divis_ID).F_FullName + "学部" + " " + bill.F_Charge_Type + "" + bill.F_Pay + "元";
                vouentity.F_In_No = bill.F_In_No;
                vouentity.F_Money = CmycurD(bill.F_Pay.ToString()) + "  ￥" + bill.F_Pay.ToString() + "元";
                vouentity.F_Name = bill.F_Charge_Type == "考试报名费" ? bill.School_ExamSignUp_Entity.F_Name : bill.School_EntrySignUp_Entity.F_Name;

                //Dictionary<string, object> dataitems = CacheFactory.Cache().GetCache<Dictionary<string, object>>(Cons.DATAITEMS);
                //string F_PayType = "";
                //object dic = new Dictionary<string, string>();
                //if (bill.F_PayType != null && dataitems.TryGetValue("pay_type", out dic))
                //{
                //    ((Dictionary<string, string>)dic).TryGetValue(bill.F_PayType, out F_PayType);
                //}

                vouentity.F_PayType = bill.F_PayType;
                vouentity.F_StudentNum = bill.F_Charge_Type == "考试报名费" ? bill.School_ExamSignUp_Entity.F_StuTestNum : bill.School_EntrySignUp_Entity.F_StudentNum;
                vouentity.F_Students_ID = bill.F_Charge_Type != "考试报名费" ? bill.School_EntrySignUp_Entity.F_Students_ID : "";
                vouentity.F_Voucher_Num = new School_No_Seed_App().getVoucherNum();
                vouentity.F_Toll_DTM = bill.F_Toll_DTM;
                new School_Bill_Voucher_App().SubmitForm(vouentity, "");
            }
            return Json("生成成功。", JsonRequestBehavior.AllowGet);
        }

        public ActionResult FileDelete(string filepa, string filePath, string fileName)
        {
            filePath = Server.MapPath(filePath);
            FileStream fs = new FileStream(filePath, FileMode.Open);
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(fileName));
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
            System.IO.File.Delete(Server.MapPath(filepa));
            return new EmptyResult();
        }


        /// <summary>
        /// 批量生成账单
        /// 规则：
        /// 1、根据缴费类型 上学期、下学期、全年
        /// 2、根据协议，永久调整、奖学金按学年
        /// </summary>
        /// <param name="F_DepartmentId">最后一级机构Id</param>
        /// <param name="F_StudentNum">多个学号，逗号分割</param>
        /// <param name="F_Period">全年、上学期、下学期</param>
        /// <returns>生成日志</returns>
        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]

        public ActionResult makeBills(string F_DepartmentId, string F_StudentNum, string F_Year)
        {
            string restult = app.makeBills(F_DepartmentId, F_StudentNum, F_Year);
            string state = ResultType.error.ToString();
            if (restult == "<p></p>")
            {
                state = ResultType.success.ToString();
                restult = "生成成功";
            }
            return Content(new AjaxResult { state = state, message = restult }.ToJson());
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]

        public ActionResult ChangeBills(string F_DepartmentId, string F_StudentNum, string F_Year)
        {
            app.ChangeBills(F_DepartmentId, F_StudentNum, F_Year);
            return Success("生成成功");
        }
    }
}