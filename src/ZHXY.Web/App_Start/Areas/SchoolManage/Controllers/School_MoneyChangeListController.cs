/*******************************************************************************
 * Author: mario
 * Description: School_MoneyChangeList  Controller类
********************************************************************************/

using NFine.Application.SchoolManage;
using NFine.Code;
using NFine.Code.Excel;
using NFine.Domain.Entity.SchoolManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    //记录费用临时调整
    public class School_MoneyChangeListController : ControllerBase
    {
        public ActionResult Auditor()
        {
            return View();
        }

        public ActionResult OnlyForm()
        {
            return View();
        }

        private School_MoneyChangeList_App app = new School_MoneyChangeList_App();
        private School_EntrySignUp_App xyApp = new School_EntrySignUp_App();

        /// <summary>
        /// </summary>
        /// <param name="pagination">      </param>
        /// <param name="keyword">         </param>
        /// <param name="F_Auditor_Name">  </param>
        /// <param name="F_Year">          </param>
        /// <param name="F_Status">        </param>
        /// <param name="F_Sign_Id">       </param>
        /// <param name="fyzt">           费用状态 </param>
        /// <returns>  </returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_Auditor_Name, string F_Year, string F_Status, string F_Sign_Id, string fyzt)
        {
            var data = new
            {
                rows = app.GetList(pagination, keyword, F_Auditor_Name, F_Year, F_Status, F_Sign_Id, fyzt),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJsonOnlyComeBack(Pagination pagination, string keyword, string F_Auditor_Name, string F_Year, string F_Status, string F_Sign_Id, string fyzt)
        {
            var data = new
            {
                rows = app.GetList(pagination, keyword, F_Auditor_Name, F_Year, F_Status, F_Sign_Id, fyzt),
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
        //[ValidateAntiForgeryToken]
        public ActionResult SubmitForm(MoneyChangeList entity, string keyValue)
        {
            EntrySignUp sesu = xyApp.GetForm(entity.F_Sign_Id);
            entity.F_Student_Id = sesu.F_Students_ID;
            entity.F_Student_Num = sesu.F_StudentNum;
            entity.F_Student_Name = sesu.F_Name;
            entity.F_Sign_Id_Statu = sesu.F_Statu;
            var expression = ExtLinq.True<MoneyChangeList>();
            expression = expression.And(t => t.F_Sign_Id == entity.F_Sign_Id);
            expression = expression.And(t => t.F_Status == "4");
            var datas = app.GetList(expression);
            if (datas.Count() > 0)
            {
                throw new Exception("已有申请信息正在审核中！");
            }
            sesu.F_Statu = "7";
            app.SubmitForm(entity, keyValue, sesu);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        //[ValidateAntiForgeryToken]
        public ActionResult AuditorForm(MoneyChangeList entity, string keyValue)
        {
            EntrySignUp sesu = xyApp.GetForm(entity.F_Sign_Id);
            Student stu = null;
            entity.F_Id = keyValue;
            entity.F_Audit_Time = DateTime.Now;
            entity.F_Auditor = OperatorProvider.Provider.GetCurrent().UserId;
            entity.F_Auditor_Name = OperatorProvider.Provider.GetCurrent().UserName;
            var data = app.GetFormI(keyValue);
            sesu.F_Statu = data.F_Sign_Id_Statu;
            // 审核通过
            SchoolBill bill = null;
            ChangeBillLog changebill = null;
            if (entity.F_Status == "5")
            {
                bill = new School_Bill_App().GetList().Where(t => t.F_ChargeNo == entity.F_Bill_No).First();
                changebill = new ChangeBillLog();
                if (!string.IsNullOrEmpty(entity.F_Bill_No))
                {
                    switch (entity.F_Change_Item)
                    {
                        case "1":
                            if (entity.F_Change_Type == "1")
                                changebill.F_Bill_Change_Type = "2";
                            else
                                changebill.F_Bill_Change_Type = "3";
                            break;

                        case "2":
                            if (entity.F_Change_Type == "1")
                                changebill.F_Bill_Change_Type = "5";
                            else
                                changebill.F_Bill_Change_Type = "6";
                            break;

                        case "3":
                            if (entity.F_Burse_Type == "一次性")

                                changebill.F_Bill_Change_Type = "0";
                            else
                                changebill.F_Bill_Change_Type = "1";

                            break;

                        case "4":
                            changebill.F_Bill_Change_Type = "4";

                            break;
                    }
                    changebill.F_Fee = entity.F_Change;
                    changebill.F_Charge_Id = bill.F_Id;

                    if ("4".Equals(bill.F_Charge_Status))
                        throw new Exception("账单已撤销");
                    if ("5".Equals(bill.F_Charge_Status))
                        throw new Exception("账单已退款");
                    if ("2".Equals(bill.F_Charge_Status) && !"5".Equals(changebill.F_Bill_Change_Type))
                        throw new Exception("账单已缴清");
                    if (changebill.F_Fee < 0 && (-changebill.F_Fee) > bill.F_Not_Pay && !"5".Equals(changebill.F_Bill_Change_Type))
                        throw new Exception("减少费用大于欠缴费用");
                    //补充字段
                    changebill.F_Creator_Name = OperatorProvider.Provider.GetCurrent().UserName;
                    changebill.F_Students_ID = bill.F_Student_ID;
                    changebill.F_StudentNum = bill.F_StudentNum;
                    changebill.F_Divis_ID = bill.F_Divis_ID;
                    changebill.F_Grade_ID = bill.F_Grade_ID;
                    changebill.F_Class_ID = bill.F_Class_ID;

                    //变更类型
                    //奖学金、学杂费、接送费、保额金永久调整
                    if (changebill.F_Bill_Change_Type.Equals("1")
                        || changebill.F_Bill_Change_Type.Equals("3")
                        || changebill.F_Bill_Change_Type.Equals("4")
                        || changebill.F_Bill_Change_Type.Equals("6"))
                    {
                        //bill_tmp.F_Fees += (entity.F_Fee == null ? 0 : entity.F_Fee);
                        //bill_tmp.F_In_Pay += (entity.F_Fee == null ? 0 : entity.F_Fee);
                        //bill_tmp.F_Not_Pay += (entity.F_Fee == null ? 0 : entity.F_Fee);

                        bill.F_Fees += (changebill.F_Fee == null ? 0 : changebill.F_Fee);
                        bill.F_In_Pay += (changebill.F_Fee == null ? 0 : changebill.F_Fee);
                        bill.F_Not_Pay += (changebill.F_Fee == null ? 0 : changebill.F_Fee);
                    }
                    //奖学金、学杂费、接送费、保额金临时调整
                    if (changebill.F_Bill_Change_Type.Equals("0")
                        || changebill.F_Bill_Change_Type.Equals("2")
                        || changebill.F_Bill_Change_Type.Equals("5"))
                    {
                        //bill_tmp.F_Fees += (entity.F_Fee == null ? 0 : entity.F_Fee);
                        //bill_tmp.F_In_Pay += (entity.F_Fee == null ? 0 : entity.F_Fee);
                        //bill_tmp.F_Not_Pay += (entity.F_Fee == null ? 0 : entity.F_Fee);

                        bill.F_Fees += (changebill.F_Fee == null ? 0 : changebill.F_Fee);
                        bill.F_In_Pay += (changebill.F_Fee == null ? 0 : changebill.F_Fee);
                        bill.F_Not_Pay += (changebill.F_Fee == null ? 0 : changebill.F_Fee);
                    }

                    if (bill.F_Not_Pay == 0)
                    {
                        if (changebill.F_Bill_Change_Type.Equals("5"))
                            //bill_tmp.F_Charge_Status = "5";
                            bill.F_Charge_Status = "5";
                        else
                            bill.F_Charge_Status = "2";
                        //bill_tmp.F_Charge_Status = "2";
                    }
                    else if (bill.F_Not_Pay == bill.F_Fees)
                    {
                        //bill_tmp.F_Charge_Status = "1"\
                        if (bill.F_Charge_Status == "6")
                        {
                            bill.F_Charge_Status = "6";
                        }
                        else bill.F_Charge_Status = "1";
                    }
                    else if (bill.F_Not_Pay > 0)
                    {
                        //bill_tmp.F_Charge_Status = "3";
                        if (bill.F_Charge_Status == "6")
                        {
                            bill.F_Charge_Status = "6";
                        }
                        else bill.F_Charge_Status = "3";
                    }

                    changebill.F_Name = bill.F_Student_Name;
                    changebill.F_Creator_Name = OperatorProvider.Provider.GetCurrent().UserName;
                    changebill.Create();
                }
                // 状态为永久调整
                if (data.F_Change_Type == "2")
                {
                    stu = new School_Students_App().GetFormByF_InitNum(sesu.F_InitNum);
                    switch (data.F_Change_Item)
                    {
                        case "1":
                            sesu.F_ComeBack_Change_Ever = (sesu.F_ComeBack_Change_Ever == null ? 0 : sesu.F_ComeBack_Change_Ever)
                                + (data.F_Change == null ? 0 : data.F_Change);
                            sesu.F_ComeBack_Proof_Ever = data.F_Proof;
                            sesu.F_ComeBack_Desc_Ever = data.F_Proof_Desc;

                            sesu.F_ComeBackType = entity.F_ComeBackType;
                            sesu.F_ComeBackProv = entity.F_ComeBackProv;
                            sesu.F_ComeBackCity = entity.F_ComeBackCity;
                            sesu.F_ComeBackArea = entity.F_ComeBackArea;
                            sesu.F_ComeBackRoute_ID = entity.F_ComeBackRoute_ID;
                            sesu.F_ComeBackSite = entity.F_ComeBackSite;
                            if (stu != null)
                            {
                                stu.F_ComeBackType = entity.F_ComeBackType;
                                stu.F_ComeBackPro = entity.F_ComeBackProv;
                                stu.F_ComeBackCity = entity.F_ComeBackCity;
                                stu.F_ComeBackArea = entity.F_ComeBackArea;
                                stu.F_ComeBackRoute_ID = entity.F_ComeBackRoute_ID;
                                stu.F_ComeBackSite = entity.F_ComeBackSite;
                            }
                            break;

                        case "2":
                            sesu.F_SundryFees_Change_Ever = (sesu.F_SundryFees_Change_Ever == null ? 0 : sesu.F_SundryFees_Change_Ever)
                                + (data.F_Change == null ? 0 : data.F_Change);
                            sesu.F_SundryFees_Proof_Ever = data.F_Proof;
                            sesu.F_SundryFees_Desc_Ever = data.F_Proof_Desc;
                            break;
                            //case "3":
                            //    sesu.F_Burse_Change_Ever = (sesu.F_Burse_Change_Ever == null ? 0 : sesu.F_Burse_Change_Ever)
                            //        + (data.F_Change == null ? 0 : data.F_Change);
                            //    sesu.F_Burse_Proof_Ever = data.F_Proof;
                            //    sesu.F_Burse_Proof_Desc_Ever = data.F_Proof_Desc;
                            //    break;
                            //case "4":
                            //    sesu.F_Prepay_Fees_Change = data.F_Change;
                            //    sesu.F_Prepay_Proof = data.F_Proof;
                            //    sesu.F_Prepay_Proof_Desc = data.F_Proof_Desc;
                            //    sesu.F_Prepay_Fees_Must += (data.F_Change == null ? 0 : data.F_Change);
                            //    break;
                    }
                }

                // 项目为保额金
                if (data.F_Change_Item == "4")
                {
                    sesu.F_Prepay_Fees_Change = (sesu.F_Prepay_Fees_Change == null ? 0 : sesu.F_Prepay_Fees_Change)
                        + (data.F_Change == null ? 0 : data.F_Change);
                    sesu.F_Prepay_Proof = data.F_Proof;
                    sesu.F_Prepay_Proof_Desc = data.F_Proof_Desc;
                    sesu.F_Prepay_Fees_Must = (sesu.F_Prepay_Fees_Must == null ? 0 : sesu.F_Prepay_Fees_Must)
                        + (data.F_Change == null ? 0 : data.F_Change);
                }
                // 项目为奖学金
                if (data.F_Change_Item == "3")
                {
                    sesu.F_Burse_Type = data.F_Burse_Type;
                    sesu.F_Burse_Fees = (sesu.F_Burse_Fees == null ? 0 : sesu.F_Burse_Fees)
                        + (data.F_Change == null ? 0 : data.F_Change);
                    sesu.F_Burse_Proof_Ever = data.F_Proof;
                    sesu.F_Burse_Proof_Desc_Ever = data.F_Proof_Desc;
                }
                //new School_EntrySignUp_App().UpdateForm(sesu);
            }

            app.AuditorForm(entity, sesu, stu, changebill, bill);
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

            string exportSql = CreateExportSql("School_MoneyChangeList", parms);
            //string exportSql = "";
            //Console.WriteLine("exportSql==>" + exportSql);
            DataTable dt = app.getDataTable(exportSql, dbParameter);
            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(dt, "School_MoneyChangeList列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "School_MoneyChangeList列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
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
            //rules.Add("F_Year", new string[] { "年度", "" });
            //rules.Add("F_Student_Num", new string[] { "学号", "" });
            //rules.Add("F_Student_Id", new string[] { "学生ID", "" });
            //rules.Add("F_Change_Type", new string[] { "调整类型", "" });
            //rules.Add("F_Change_Item", new string[] { "调整项目", "" });
            //rules.Add("F_ComeBack_Change", new string[] { "调整金额", "" });
            //rules.Add("F_ComeBack_Proof", new string[] { "调整凭证", "" });
            //rules.Add("F_ComeBack_Proof_Desc", new string[] { "调整原因", "" });
            //rules.Add("F_Status", new string[] { "审核状态", "" });
            //rules.Add("F_Audit_Memo", new string[] { "审核意见", "" });
            //rules.Add("F_Auditor", new string[] { "审核人", "" });
            //rules.Add("F_Audit_Time", new string[] { "审核时间", "" });
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
            //rules.Add("F_Year_Period", new string[] { "缴费学期", "" });

            //////////////////处理数据(机构 岗位 等字典替换，过滤不要的字段，修改表头)
            List<MoneyChangeList> list = ExcelToList<MoneyChangeList>(Server.MapPath(filePath), "School_MoneyChangeList");

            ///////////////////入库
            if (list == null)
                return Error("导入失败");
            app.import(list);
            return Success("导入成功。");
        }

        /// <summary>
        /// 收费标准 根据协议收费标准 算上永久得调整
        /// </summary>
        /// <param name="entity">    </param>
        /// <param name="keyValue">  </param>
        /// <returns>  </returns>
        [HttpGet]
        [HandlerAjaxOnly]
        //[ValidateAntiForgeryToken]
        public ActionResult GetChargeStandard(string xyid)
        {
            EntrySignUp xy = xyApp.GetForm(xyid);
            //调整
            var expression = ExtLinq.True<MoneyChangeList>();
            expression = expression.And(t => t.F_Sign_Id == xyid);
            List<MoneyChangeList> list = app.GetList(expression);
            List<MoneyChangeList> tmp = new List<MoneyChangeList>();
            StringBuilder sb = new StringBuilder(string.Empty);
            //缴费方式
            string chargeType = xy.F_Charge_mode;
            //保额金
            sb.Append("<p>保额金(一次性):" + xy.F_Prepay_Fees);
            if (!Ext.IsEmpty(xy.F_Prepay_Fees_Change))
            {
                sb.Append("｜调整金额:" + xy.F_Prepay_Fees_Change);
            }
            sb.Append("</p>");
            //学杂费
            sb.Append("<p>学杂费(" + chargeType + "):" + xy.F_SundryFees);
            if (!Ext.IsEmpty(xy.F_SundryFees_Change_Ever))
            {
                sb.Append("｜永久调整金额:" + xy.F_SundryFees_Change_Ever);
            }

            tmp = list.Where(t => t.F_Change_Item == "2" && t.F_Change_Type == "1" && t.F_Status == "5" && t.F_Year == DateTime.Now.Year.ToString()).OrderByDescending(t => t.F_CreatorTime).ToList();
            if (tmp.Count >= 1)
            {
                foreach (MoneyChangeList smce in tmp)
                {
                    sb.Append("｜当期调整金额:" + smce.F_Change);
                }
            }
            sb.Append("</p>");
            //接送费
            if (!Ext.IsEmpty(xy.F_ComeBack_Fees))
            {
                sb.Append("<p>接送费(" + chargeType + "):" + xy.F_ComeBack_Fees);
                if (!Ext.IsEmpty(xy.F_ComeBack_Change_Ever))
                {
                    sb.Append("｜永久调整金额:" + xy.F_ComeBack_Change_Ever);
                }
                tmp = list.Where(t => t.F_Change_Item == "1" && t.F_Change_Type == "1" && t.F_Status == "5" && t.F_Year == DateTime.Now.Year.ToString()).OrderByDescending(t => t.F_CreatorTime).ToList();
                if (tmp.Count >= 1)
                {
                    foreach (MoneyChangeList smce in tmp)
                    {
                        sb.Append("｜当期调整金额:" + smce.F_Change);
                    }
                }
                sb.Append("</p>");
            }

            //奖学金
            if ("按学年".Equals(xy.F_Burse_Type))
            {
                sb.Append("<p>奖学金(" + xy.F_Burse_Type + "):" + xy.F_Burse_Fees);
                sb.Append("</p>");
            }

            tmp = list.Where(t => t.F_Change_Item == "3" && t.F_Burse_Type == "一次性" && t.F_Status == "5" && t.F_Year == DateTime.Now.Year.ToString()).OrderByDescending(t => t.F_CreatorTime).ToList();
            if (tmp.Count >= 1)
            {
                sb.Append("<p>奖学金(一次性):");
                foreach (MoneyChangeList smce in tmp)
                {
                    sb.Append(smce.F_Change + "｜");
                }
                sb.Append("</p>");
            }

            return Content(sb.ToString());
        }
    }
}