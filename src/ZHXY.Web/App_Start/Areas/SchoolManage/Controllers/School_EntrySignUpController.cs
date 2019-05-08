/*******************************************************************************
 * Author: mario
 * Description: School_EntrySignUp  Controller类
********************************************************************************/

using NFine.Application;
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
    //入学报名表
    public class School_EntrySignUpController : ControllerBase
    {
        private School_EntrySignUp_App app = new School_EntrySignUp_App();

        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Fast_Form()
        {
            return View();
        }

        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Fast_Price()
        {
            return View();
        }

        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Fast_Only_ComeBack()
        {
            return View();
        }

        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Fast_Dj_Form()
        {
            return View();
        }

        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Fast_Js_Form()
        {
            return View();
        }

        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Fast_Jx_Form()
        {
            return View();
        }

        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Fast_Xz_Form()
        {
            return View();
        }

        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Fast_Qz_Form()
        {
            return View();
        }

        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Fast_Index()
        {
            return View();
        }

        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Fast_Audio()
        {
            return View();
        }

        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Info_Audio()
        {
            return View();
        }

        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult Fast_Details()
        {
            return View();
        }

        /// <summary>
        /// </summary>
        /// <param name="pagination">               </param>
        /// <param name="keyword">                  </param>
        /// <param name="F_DepartmentId">           </param>
        /// <param name="F_CreatorTime_Start">      </param>
        /// <param name="F_CreatorTime_Stop">       </param>
        /// <param name="F_Statu">                 报名状态 </param>
        /// <param name="F_Date_Status">           信息完善状态 </param>
        /// <param name="F_Signed_License_Status"> 报名协议签字状态 </param>
        /// <param name="xyzt">                    报名状态 </param>
        /// <param name="xxzt">                    信息完善状态 </param>
        /// <returns>  </returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_DepartmentId, string F_CreatorTime_Start, string F_CreatorTime_Stop, string F_Statu, string F_Date_Status, string F_Signed_License_Status, string Statu, string xyzt, string xxzt)
        {
            var data = new
            {
                rows = app.GetList(pagination, keyword, F_DepartmentId, F_CreatorTime_Start, F_CreatorTime_Stop, F_Statu, F_Date_Status, F_Signed_License_Status, Statu, xyzt, xxzt),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetSelectList(string DutyId, string F_StudentName)
        {
            var F_DutyId = new DutyApp().GetForm(DutyId);
            string UserId = OperatorProvider.Provider.GetCurrent().UserId;
            string MobilePhone = OperatorProvider.Provider.GetCurrent().MobilePhone;
            var aa = new List<EntrySignUp>();
            List<object> list = new List<object>();
            string UserCode = OperatorProvider.Provider.GetCurrent().UserCode;
            if (UserCode == "admin")
            {
                aa = new School_EntrySignUp_App().GetList(string.Empty, string.Empty, F_StudentName);
                foreach (EntrySignUp item in aa)
                {
                    if (!string.IsNullOrEmpty(item.F_StudentNum))
                    {
                        list.Add(new { id = item.F_StudentNum, text = item.F_Name });
                    }
                }
            }
            else if (F_DutyId != null)
            {
                if (F_DutyId.F_FullName.Equals("家长"))
                {
                    aa = new School_EntrySignUp_App().GetList(string.Empty, UserId, F_StudentName);
                    foreach (EntrySignUp item in aa)
                    {
                        if (!string.IsNullOrEmpty(item.F_StudentNum))
                        {
                            list.Add(new { id = item.F_StudentNum, text = item.F_Name });
                        }
                    }
                }
                else if (F_DutyId.F_FullName.Equals("老师"))
                {
                    aa = new School_EntrySignUp_App().GetList(UserId, string.Empty, F_StudentName);
                    foreach (EntrySignUp item in aa)
                    {
                        if (!string.IsNullOrEmpty(item.F_StudentNum))
                        {
                            list.Add(new { id = item.F_StudentNum, text = item.F_Name });
                        }
                    }
                }
            }
            return Content(list.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJsonMobile()
        {
            string[] _order = { "F_CreatorTime" };
            var data = new
            {
                rows = app.GetList(OperatorProvider.Provider.GetCurrent().UserId, false, _order)
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = app.GetForm(keyValue);
            //将用户id替换成姓名
            //var creator = new UserEntity();
            //var modifier = new UserEntity();
            //Dictionary<string, UserEntity>  dic = CacheFactory.Cache().GetCache<Dictionary<string, UserEntity>>(Cons.USERS);
            //if (data.F_CreatorUserId != null && dic.TryGetValue(data.F_CreatorUserId, out creator))
            //{
            //    data.F_CreatorUserId = creator.F_RealName;
            //}
            //if (data.F_LastModifyUserId != null && dic.TryGetValue(data.F_LastModifyUserId, out modifier))
            //{
            //    data.F_LastModifyUserId = modifier.F_RealName;
            //}
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetForm(string F_InitNum)
        {
            var data = app.GetFormByF_InitNumI(F_InitNum);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        //[ValidateAntiForgeryToken]
        public ActionResult SubmitForm(EntrySignUp entity, string keyValue)
        {
            try
            {
                var entity1 = new EntrySignUpClone();
                entity1 = ExtObj.clonePropValue(entity, entity1);
                app.SubmitForm(entity, keyValue, entity1);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error("报名失败，" + ex.Message);
            }
        }

        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult UpdateCityAndArea(EntrySignUp entity, string keyValue)
        {
            app.UpdateCityAndArea(entity, keyValue);
            return Success("操作成功。");
        }

        //信息完善保存
        [HttpPost]
        [HandlerAjaxOnly]
        //[ValidateAntiForgeryToken]
        public ActionResult InfoSubmitForm(EntrySignUp entity, bool ifCommit)
        {
            try
            {
                if (ifCommit)
                    entity.F_Date_Status = "4";
                app.UpdateInfoForm(entity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        //信息完善保存
        [HttpPost]
        [HandlerAjaxOnly]
        //[ValidateAntiForgeryToken]
        public ActionResult AdminInfoSubmitForm(EntrySignUp entity, string keyValue)
        {
            try
            {
                entity.F_Id = keyValue;
                app.UpdateInfoForm(entity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        [HttpPost]
        [HandlerAjaxOnly]
        //调整金额
        public ActionResult ChangeForm(EntrySignUp entity, string keyValue, string F_Charge_Type)
        {
            try
            {
                entity.F_Id = keyValue;
                app.ChangeMoneyForm(entity, F_Charge_Type);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        [HttpPost]
        [HandlerAjaxOnly]
        //审核
        public ActionResult AudioForm(EntrySignUp entity, string keyValue)
        {
            if ("7".Equals(entity.F_Statu) || "8".Equals(entity.F_Statu))
            {
                throw new Exception("该报名协议调整审核中,暂停审核");
            }
            OperatorModel user = OperatorProvider.Provider.GetCurrent();
            entity.F_Id = keyValue;
            entity.F_Auditor_Sec = user.UserId;
            entity.F_Auditor_Sec_Name = user.UserName;
            entity.F_AuditDTM_Sec = DateTime.Now;
            app.UpdateForm(entity);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        //签字审核
        public ActionResult SignAudioForm(EntrySignUp entity, string keyValue)
        {
            OperatorModel user = OperatorProvider.Provider.GetCurrent();
            entity.F_Id = keyValue;
            entity.F_Signed_License_Auditor = user.UserId;
            entity.F_Signed_License_Auditor_Time = DateTime.Now;
            entity.F_Signed_License_Auditor_Name = user.UserName;
            app.UpdateForm(entity);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        //信息完善
        public ActionResult InfoAudioForm(EntrySignUp entity, string keyValue)
        {
            OperatorModel user = OperatorProvider.Provider.GetCurrent();
            entity.F_Id = keyValue;
            entity.F_Data_Memo_Auditor = user.UserId;
            entity.F_Data_Memo_Auditor_Name = user.UserName;
            entity.F_Data_Memo_Time = DateTime.Now;
            app.UpdateForm(entity);
            return Success("操作成功。");
        }

        [HttpGet]
        [HandlerAjaxOnly]
        //审核
        public ActionResult DjPay(string keyValue)
        {
            try
            {
                /////支付流程

                //数据库操作
                app.DjPay(keyValue);
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

        /// <summary>
        /// 下载协议
        /// </summary>
        /// <param name="keyValue">  </param>
        /// <returns>  </returns>
        [HttpGet]
        //[HandlerAuthorize]
        [HandlerAjaxOnly]
        public ActionResult downXy(string keyValue)
        {
            try
            {
                EntrySignUp e = app.GetForm(keyValue);
                Subject sce = new School_Subjects_App().GetForm(e.F_Subjects_ID);
                if (Ext.IsEmpty(sce))
                {
                    throw new Exception("该班级报名已停止或失效，请联系管理员！");
                }
                string F_Subjects_ID = sce.F_Class_Type;
                //Dictionary<string, object> areas = CacheFactory.Cache().GetCache<Dictionary<string, object>>(Cons.AREA);
                object F_ComeBackProv = string.Empty, F_ComeBackCity = string.Empty, F_ComeBackArea = string.Empty;
                Dictionary<string, object> schoolareas = CacheFactory.Cache().GetCache<Dictionary<string, object>>(Cons.SCHOOLAREA);
                if (schoolareas == null)
                    schoolareas = (Dictionary<string, object>)CacheConfig.GetSchoolAreaList();
                //object F_SchoolComeBackCity = "", F_SchoolComeBackArea = "";

                if (e.F_ComeBackCity != null && schoolareas.TryGetValue(e.F_ComeBackCity, out F_ComeBackCity))
                {
                    F_ComeBackCity = F_ComeBackCity.GetType().GetProperty("fullname").GetValue(F_ComeBackCity, null).ToString();
                }
                if (e.F_ComeBackArea != null && schoolareas.TryGetValue(e.F_ComeBackArea, out F_ComeBackArea))
                {
                    F_ComeBackArea = F_ComeBackArea.GetType().GetProperty("fullname").GetValue(F_ComeBackArea, null).ToString();
                }

                decimal? hj1 = e.F_SundryFees
                    + (e.F_SundryFees_Change == null ? 0 : e.F_SundryFees_Change)
                    + (e.F_SundryFees_Change_Ever == null ? 0 : e.F_SundryFees_Change_Ever)
                    + (e.F_ComeBack_Fees == null ? 0 : e.F_ComeBack_Fees)
                    + (e.F_ComeBack_Change == null ? 0 : e.F_ComeBack_Change)
                    + (e.F_ComeBack_Change_Ever == null ? 0 : e.F_ComeBack_Change_Ever);
                hj1 = Ext.ToDecimal(hj1, 2);
                decimal? hj2 = e.F_SundryFees
                    + (e.F_SundryFees_Change == null ? 0 : e.F_SundryFees_Change)
                    + (e.F_SundryFees_Change_Ever == null ? 0 : e.F_SundryFees_Change_Ever)
                    + (e.F_ComeBack_Fees == null ? 0 : e.F_ComeBack_Fees)
                    + (e.F_ComeBack_Change == null ? 0 : e.F_ComeBack_Change)
                    + (e.F_ComeBack_Change_Ever == null ? 0 : e.F_ComeBack_Change_Ever)
                    - (e.F_Burse_Fees == null ? 0 : e.F_Burse_Fees);
                hj2 = Ext.ToDecimal(hj2, 2);
                string F_Burse_Fees = string.Empty;
                if ("按学年".Equals(e.F_Burse_Type))
                {
                    F_Burse_Fees = "4.成绩特别优秀的学生给予魏珍奖学金，经双方约定学校提供 奖学金 " + Ext.ToDecimal(e.F_Burse_Fees, 2) + " 元/年，合计每年实交费用" + hj2 + " 元，但乙方须保证在我校就读初中，否则需归还每年享受的奖学金。";
                }
                else if ("一次性".Equals(e.F_Burse_Type))
                {
                    F_Burse_Fees = "4. 成绩特别优秀的学生给予魏珍奖学金，经双方约定一次性奖金额度为 奖学金" + Ext.ToDecimal(e.F_Burse_Fees, 2) + " 元（¥ 奖学金 " + Ext.CmycurD(Ext.ToDecimal(e.F_Burse_Fees, 2)) + "大写），但乙方须保证在我校就读初中，否则需归还享受的奖学金。";
                }
                else
                {
                    F_Burse_Fees = "                                                                                                                                                                                                                                             "
                        + "                                                                                                                                                                                                                                             ";
                }
                //模版 区分自费生 公费生
                string F_Model = Configs.GetValue("modelPath");
                Organize org = new OrganizeApp().GetForm(e.F_Divis_ID);
                string modelType = string.Empty;
                if ("自费生".Equals(e.F_Stu_Type) && (("03").Equals(e.F_Divis_ID) || ("06").Equals(e.F_Divis_ID)))
                {
                    modelType = "-z";
                }
                if ("公费生".Equals(e.F_Stu_Type) && (("03").Equals(e.F_Divis_ID) || ("06").Equals(e.F_Divis_ID)))
                {
                    modelType = "-g";
                }
                F_Model += org.F_EnCode + modelType + ".docx";
                Dictionary<string, object> dataitems = CacheFactory.Cache().GetCache<Dictionary<string, object>>(Cons.DATAITEMS);
                string F_YearText = string.Empty;
                object dic = new Dictionary<string, string>();
                if (e.F_Year != null && dataitems.TryGetValue("F_Year", out dic))
                {
                    ((Dictionary<string, string>)dic).TryGetValue(e.F_Year.ToString(), out F_YearText);
                }
                string F_Sign = "诸暨荣怀招生部";
                string F_Tel = "0575-87325668";
                if (e.F_RegUserType != "parentDuty")
                {
                    F_Sign = e.F_RegisterName + " " + new UserApp().GetForm(e.F_RegUsers_ID).F_Account;
                    F_Tel = e.F_RegisterPhone;
                }
                string F_Password = "000000";
                if (!Ext.IsEmpty(e.F_Guarder_CredNum) && e.F_Guarder_CredNum.Length >= 6)
                    F_Password = e.F_Guarder_CredNum.Substring(e.F_Guarder_CredNum.Length - 6, 6);
                F_Password = F_Password.ToLower().Replace("x", "0");//密码x变为0
                var data = new
                {
                    F_Guarder = e.F_Guarder,
                    F_Name = e.F_Name,
                    F_Gender = "1".Equals(e.F_Gender) ? "男" : "女",
                    F_CredType = e.F_CredType,
                    F_CredNum = e.F_CredNum,
                    F_Year = e.F_Year,
                    F_YearText = F_YearText,
                    F_INYear = e.F_INYear,
                    F_Subjects_ID = F_Subjects_ID,
                    F_Prepay_Fees_Must = e.F_Prepay_Fees_Must,
                    F_ComeBackType = e.F_ComeBackType,
                    //F_ComeBackProv = (Ext.IsEmpty(F_ComeBackProv) ? "无" : F_ComeBackProv),
                    F_ComeBackCity = (Ext.IsEmpty(F_ComeBackCity) ? string.Empty : F_ComeBackCity),
                    F_ComeBackArea = (Ext.IsEmpty(F_ComeBackArea) ? "无" : F_ComeBackArea),
                    //F_Hj1 = hj1+"(元)",
                    F_Hj1 = hj1,
                    //F_Hj2 = hj2,

                    //F_Description = "(备注："+ e.F_Description + ")",
                    F_Description = (string.IsNullOrEmpty(e.F_Description)) ? string.Empty : "(备注：" + e.F_Description + ")",
                    F_StudentNum = e.F_StudentNum,
                    F_Burse_Fees = F_Burse_Fees,
                    F_SundryFees = e.F_SundryFees
                    + (e.F_SundryFees_Change == null ? 0 : e.F_SundryFees_Change)
                    + (e.F_SundryFees_Change_Ever == null ? 0 : e.F_SundryFees_Change_Ever),
                    F_ComeBack = (e.F_ComeBack_Fees == null ? 0 : e.F_ComeBack_Fees)
                    + (e.F_ComeBack_Change == null ? 0 : e.F_ComeBack_Change)
                    + (e.F_ComeBack_Change_Ever == null ? 0 : e.F_ComeBack_Change_Ever),
                    F_Model = F_Model,
                    F_SchoolType = e.F_SchoolType,
                    F_zsrx = "0575-87325668",
                    F_cwrx = "0575-87321382",
                    F_szcode = string.Empty,
                    F_jfzn = string.Empty,
                    F_GenDate = DateTime.Now.ToString("yyyy年MM月dd日"),
                    F_Sign = F_Sign,
                    F_Tel = F_Tel,
                    F_Password = F_Password
                };

                return Content(data.ToJson());
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 上传协议
        /// </summary>
        /// <param name="keyValue">  </param>
        /// <param name="path">     协议图片地址 </param>
        /// <returns>  </returns>
        [HttpGet]
        //[HandlerAuthorize]
        [HandlerAjaxOnly]
        public ActionResult upXy(string keyValue, string path)
        {
            try
            {
                EntrySignUp e = app.GetForm(keyValue);
                e.F_Signed_License = path;
                e.F_Signed_License_Status = "2";
                app.UpdateForm(e);
                return Success("上传成功");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 手机接口完善信息固定字段
        /// </summary>
        /// <param name="keyValue">  </param>
        /// <returns>  </returns>
        [HttpGet]
        //[HandlerAuthorize]
        [HandlerAjaxOnly]
        public ActionResult StaticInfo(string keyValue)
        {
            try
            {
                EntrySignUp e = app.GetForm(keyValue);
                Dictionary<string, string[]> content = new Dictionary<string, string[]>();
                //班级类型
                string F_Subjects_ID = "";
                Dictionary<string, object> dataitems = CacheFactory.Cache().GetCache<Dictionary<string, object>>(Cons.DATAITEMS);
                string F_YearText = "";
                object dic = new Dictionary<string, string>();
                if (dataitems.TryGetValue("Class_Type", out dic))
                {
                    ((Dictionary<string, string>)dic).TryGetValue(new School_Subjects_App().GetForm(e.F_Subjects_ID).F_Class_Type, out F_YearText);
                    F_Subjects_ID = F_YearText;
                }

                //string F_Subjects_ID = new School_Subjects_App().GetForm(e.F_Subjects_ID).F_Title;

                //地区
                //Dictionary<string, object> areas = CacheFactory.Cache().GetCache<Dictionary<string, object>>(Cons.AREA);
                //if (areas == null)
                //    areas = (Dictionary<string, object>)MvcApplication.GetAreaList();
                object F_ComeBackProv = string.Empty, F_ComeBackCity = string.Empty, F_ComeBackArea = string.Empty;
                //if (e.F_ComeBackProv != null && areas.TryGetValue(e.F_ComeBackProv, out F_ComeBackProv))
                //{
                //    F_ComeBackProv = F_ComeBackProv.GetType().GetProperty("fullname").GetValue(F_ComeBackProv, null).ToString();
                //}
                //if (e.F_ComeBackCity != null && areas.TryGetValue(e.F_ComeBackCity, out F_ComeBackCity))
                //{
                //    F_ComeBackCity = F_ComeBackCity.GetType().GetProperty("fullname").GetValue(F_ComeBackCity, null).ToString();
                //}
                //if (e.F_ComeBackArea != null && areas.TryGetValue(e.F_ComeBackArea, out F_ComeBackArea))
                //{
                //    F_ComeBackArea = F_ComeBackArea.GetType().GetProperty("fullname").GetValue(F_ComeBackArea, null).ToString();
                //}

                //接送地区
                Dictionary<string, object> schoolareas = CacheFactory.Cache().GetCache<Dictionary<string, object>>(Cons.SCHOOLAREA);
                if (schoolareas == null)
                    schoolareas = (Dictionary<string, object>)CacheConfig.GetSchoolAreaList();
                //object F_SchoolComeBackCity = "", F_SchoolComeBackArea = "";

                if (e.F_ComeBackCity != null && schoolareas.TryGetValue(e.F_ComeBackCity, out F_ComeBackCity))
                {
                    F_ComeBackCity = F_ComeBackCity.GetType().GetProperty("fullname").GetValue(F_ComeBackCity, null).ToString();
                }
                if (e.F_ComeBackArea != null && schoolareas.TryGetValue(e.F_ComeBackArea, out F_ComeBackArea))
                {
                    F_ComeBackArea = F_ComeBackArea.GetType().GetProperty("fullname").GetValue(F_ComeBackArea, null).ToString();
                }

                //机构
                Dictionary<string, object> orgs = CacheFactory.Cache().GetCache<Dictionary<string, object>>(Cons.ORGANIZE);
                object F_Divis_ID = string.Empty, F_Grade_ID = string.Empty;
                if (e.F_Divis_ID != null && orgs.TryGetValue(e.F_Divis_ID, out F_Divis_ID))
                {
                    F_Divis_ID = F_Divis_ID.GetType().GetProperty("fullname").GetValue(F_Divis_ID, null).ToString();
                }
                if (e.F_Grade_ID != null && orgs.TryGetValue(e.F_Grade_ID, out F_Grade_ID))
                {
                    F_Grade_ID = F_Grade_ID.GetType().GetProperty("fullname").GetValue(F_Grade_ID, null).ToString();
                }
                //字典
                //Dictionary<string, object> dataitems = CacheFactory.Cache().GetCache<Dictionary<string, object>>(Cons.DATAITEMS);
                //string F_SchoolType = "", F_ComeBackType = "", F_Charge_mode = "";
                //object dic = new Dictionary<string, string>();
                //if (e.F_SchoolType != null && orgs.TryGetValue("F_SchoolType", out dic))
                //{
                //    ((Dictionary<string, string>)dic).TryGetValue(e.F_SchoolType, out F_SchoolType);
                //}
                //if (e.F_ComeBackType != null && orgs.TryGetValue("F_ComeBackType", out dic))
                //{
                //    ((Dictionary<string, string>)dic).TryGetValue(e.F_ComeBackType, out F_ComeBackType);
                //}
                //if (e.F_SchoolType != null && orgs.TryGetValue("F_Charge_mode", out dic))
                //{
                //    ((Dictionary<string, string>)dic).TryGetValue(e.F_Charge_mode, out F_Charge_mode);
                //}
                //接送路线
                if (!Ext.IsEmpty(e.F_ComeBackRoute_ID))
                {
                    string F_ComeBackRoute_ID = string.Empty;
                    ComeBackRoute scre = new School_ComeBackRoute_App().GetForm(e.F_ComeBackRoute_ID);
                    if (!Ext.IsEmpty(scre))
                    {
                        F_ComeBackRoute_ID = scre.F_Title;
                    }
                    string F_ComeBackSite = string.Empty;
                    ComeBackSite scbe = new School_ComeBackSite_App().GetForm(e.F_ComeBackSite);
                    if (!Ext.IsEmpty(scbe))
                    {
                        F_ComeBackSite = scbe.F_Title;
                    }
                    content.Add("F_ComeBackProv", new string[2] { F_ComeBackProv.ToString(), e.F_ComeBackProv });
                    content.Add("F_ComeBackCity", new string[2] { F_ComeBackCity.ToString(), e.F_ComeBackCity });
                    content.Add("F_ComeBackArea", new string[2] { F_ComeBackArea.ToString(), e.F_ComeBackArea });
                    content.Add("F_ComeBackRoute_ID", new string[2] { F_ComeBackRoute_ID, e.F_ComeBackRoute_ID });
                    content.Add("F_ComeBackSite", new string[2] { F_ComeBackSite, e.F_ComeBackSite });
                }

                content.Add("F_Year", new string[2] { e.F_Year + "年", Convert.ToString(e.F_Year) });
                //content.Add("F_INYear", new string[2] { e.F_INYear, e.F_INYear });

                content.Add("F_Subjects_ID", new string[2] { F_Subjects_ID, e.F_Subjects_ID });
                content.Add("F_Divis_ID", new string[2] { F_Divis_ID.ToString(), e.F_Divis_ID });
                content.Add("F_Grade_ID", new string[2] { F_Grade_ID.ToString(), e.F_Grade_ID });

                content.Add("F_SchoolType", new string[2] { e.F_SchoolType, e.F_SchoolType });
                content.Add("F_ComeBackType", new string[2] { e.F_ComeBackType, e.F_ComeBackType });
                content.Add("F_Charge_mode", new string[2] { e.F_Charge_mode, e.F_Charge_mode });

                return Content(content.ToJson());
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 取到学部对应
        /// </summary>
        /// <param name="keyValue">  </param>
        /// <returns>  </returns>
        [HttpGet]
        //[HandlerAuthorize]
        [HandlerAjaxOnly]
        public ActionResult GetOrgDics()
        {
            // 获取所有学部
            List<Organize> orgList = new OrganizeApp().GetList().Where(t => t.F_CategoryId == "Division").ToList();
            Dictionary<string, Dictionary<string, string[]>> orgs = new Dictionary<string, Dictionary<string, string[]>>();
            //入学年段
            Dictionary<string, string[]> tmp = new Dictionary<string, string[]>();
            ////就读方式
            //Dictionary <string, string[]> F_SchoolType = new Dictionary<string, string[]>();
            ////来源类型
            //Dictionary<string, string[]> F_ComeFrom_Type = new Dictionary<string, string[]>();
            foreach (Organize org in orgList)
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

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DisabledAccount(string keyValue)
        {
            EntrySignUp entity = app.GetList().Where(t => t.F_Id.Equals(keyValue)).First();
            entity.F_Id = keyValue;
            entity.F_Statu = "9";
            entity.Modify(entity.F_Id);
            app.UpdateAndDel(entity);

            return Success("禁用成功。");
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

            string exportSql = CreateExportSql("School_EntrySignUp", parms);
            //string exportSql = "";
            //Console.WriteLine("exportSql==>" + exportSql);
            DataTable dt = app.getDataTable(new BaseApp().dataScopeFilter(exportSql), dbParameter);
            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(dt, "School_EntrySignUp列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "School_EntrySignUp列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
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
            //rules.Add("F_ID", new string[] { "报名ID", "" });
            //rules.Add("F_RegUsers_ID", new string[] { "注册用户ID", "" });
            //rules.Add("F_RegUserType", new string[] { "注册用户类型", "" });
            //rules.Add("F_RegisterName", new string[] { "注册用户姓名（招生老师）", "" });
            //rules.Add("F_RegisterNum", new string[] { "注册用户编号（招生老师）", "" });
            //rules.Add("F_Statu", new string[] { "报名状态", "" });
            //rules.Add("F_Year", new string[] { "年度", "" });
            //rules.Add("F_INYear", new string[] { "入学年度", "" });
            //rules.Add("F_InClass", new string[] { "插班标注", "" });
            //rules.Add("F_Stu_Type", new string[] { "生源类别", "" });
            //rules.Add("F_Sco_Line", new string[] { "分数线", "" });
            //rules.Add("F_Sco_Pir", new string[] { "分数差", "" });
            //rules.Add("F_Stu_No", new string[] { "全国学籍号", "" });
            //rules.Add("F_Sqzn", new string[] { "随迁子女", "" });
            //rules.Add("F_Dzn", new string[] { "多子女", "" });
            //rules.Add("F_Dzn_File", new string[] { "多子女凭证", "" });
            //rules.Add("F_Dzn_Memo", new string[] { "多子女关联的学生姓名及身份证号", "" });
            //rules.Add("F_Teacherzn", new string[] { "教师子女", "" });
            //rules.Add("F_Teacherzn_File", new string[] { "教师子女凭证", "" });
            //rules.Add("F_Teacherzn_Memo", new string[] { "教师子女关联的学生姓名及身份证号", "" });
            //rules.Add("F_Old", new string[] { "复读生", "" });
            //rules.Add("F_In_Memo", new string[] { "入学信息备注", "" });
            //rules.Add("F_Divis_ID", new string[] { "学部ID", "" });
            //rules.Add("F_Grade_ID", new string[] { "年级ID", "" });
            //rules.Add("F_Class_ID", new string[] { "班级ID", "" });
            //rules.Add("F_Subjects_ID", new string[] { "班级类型ID", "" });
            //rules.Add("F_StudentNum", new string[] { "学号", "" });
            //rules.Add("F_Name", new string[] { "姓名", "" });
            //rules.Add("F_Name_Old", new string[] { "曾用名", "" });
            //rules.Add("F_Name_En", new string[] { "英文名", "" });
            //rules.Add("F_Sex", new string[] { "性别", "" });
            //rules.Add("F_Birthday", new string[] { "出生日期", "" });
            //rules.Add("F_Only_One", new string[] { "独生子女", "" });
            //rules.Add("F_FacePic_File", new string[] { "照片", "" });
            //rules.Add("F_Nation", new string[] { "国籍", "" });
            //rules.Add("F_ZJXY", new string[] { "宗教信仰", "" });
            //rules.Add("F_CredType", new string[] { "证件类型", "" });
            //rules.Add("F_CredNum", new string[] { "证件号码", "" });
            //rules.Add("F_CredPhoto_Obve", new string[] { "证件正面照片", "" });
            //rules.Add("F_CredPhoto_Rever", new string[] { "证件反面照片", "" });
            //rules.Add("F_Native", new string[] { "籍贯", "" });
            //rules.Add("F_Native_Old", new string[] { "祖籍国", "" });
            //rules.Add("F_Cn_Level", new string[] { "中文程度", "" });
            //rules.Add("F_HSK_TEST", new string[] { "HSK-TEST", "" });
            //rules.Add("F_Stu_Time", new string[] { "申请学习时间", "" });
            //rules.Add("F_Volk", new string[] { "民族", "" });
            //rules.Add("F_PolitStatu", new string[] { "政治面貌", "" });
            //rules.Add("F_Home_Addr", new string[] { "家庭住址", "" });
            //rules.Add("F_Height", new string[] { "身高", "" });
            //rules.Add("F_Weight", new string[] { "体重", "" });
            //rules.Add("F_Blood_Type", new string[] { "血型", "" });
            //rules.Add("F_Allergy", new string[] { "过敏药物", "" });
            //rules.Add("F_Food", new string[] { "过敏食物", "" });
            //rules.Add("F_MedicalHis", new string[] { "病史", "" });
            //rules.Add("F_MedicalHis_Memo", new string[] { "病史补充", "" });
            //rules.Add("F_Reg_Status", new string[] { "户口情况", "" });
            //rules.Add("F_RegAddr", new string[] { "户口所在地", "" });
            //rules.Add("F_RegRelat", new string[] { "与户主关系", "" });
            //rules.Add("F_RegPhoto_Obve", new string[] { "户口本主页照片", "" });
            //rules.Add("F_RegPhoto_Rever", new string[] { "户口本学生页照片", "" });
            //rules.Add("F_RegMainName", new string[] { "户主姓名", "" });
            //rules.Add("F_RegMain_CredNum", new string[] { "户主身份证号", "" });
            //rules.Add("F_RegMain_CredPhoto_Obve", new string[] { "户主身份证正面", "" });
            //rules.Add("F_RegMain_CredPhoto_Rever", new string[] { "户主身份证反面", "" });
            //rules.Add("F_FamilyAddr", new string[] { "家庭详细地址", "" });
            //rules.Add("F_Tel", new string[] { "联系电话", "" });
            //rules.Add("F_ICType", new string[] { "IC卡类别", "" });
            //rules.Add("F_Type", new string[] { "身份类别", "" });
            //rules.Add("F_ComeFrom", new string[] { "来源学校", "" });
            //rules.Add("F_ComeFromAddress", new string[] { "源校地址", "" });
            //rules.Add("F_Score", new string[] { "选拔成绩", "" });
            //rules.Add("F_Score_File", new string[] { "选拔成绩单", "" });
            //rules.Add("F_InitNum", new string[] { "入学序号", "" });
            //rules.Add("F_Students_ID", new string[] { "学生ID", "" });
            //rules.Add("F_Users_ID", new string[] { "学生用户ID", "" });
            //rules.Add("F_InitPWD", new string[] { "学生系统密码", "" });
            //rules.Add("F_InitDTM", new string[] { "入学时间", "" });
            //rules.Add("F_SchoolType", new string[] { "就读方式", "" });
            //rules.Add("F_ComeBackType", new string[] { "接送方式", "" });
            //rules.Add("F_ComeBackRoute_ID", new string[] { "接送路线ID", "" });
            //rules.Add("F_ComeBackStation", new string[] { "接送站点", "" });
            //rules.Add("F_ComeBackCity", new string[] { "接送市", "" });
            //rules.Add("F_ComeBackArea", new string[] { "接送区县", "" });
            //rules.Add("F_Relative1_Name", new string[] { "第一亲属姓名", "" });
            //rules.Add("F_Relative1_Tel", new string[] { "第一亲属电话", "" });
            //rules.Add("F_Relative1_Guarder", new string[] { "第一亲属文化程度", "" });
            //rules.Add("F_Relative1_Comp", new string[] { "第一亲属工作单位", "" });
            //rules.Add("F_Relative1_Guarder_Relation", new string[] { "第一亲属亲子关系", "" });
            //rules.Add("F_Relative2_Name", new string[] { "第二亲属姓名", "" });
            //rules.Add("F_Relative2_Tel", new string[] { "第二亲属电话", "" });
            //rules.Add("F_Relative2_Guarder", new string[] { "第二亲属文化程度", "" });
            //rules.Add("F_Relative2_Comp", new string[] { "第二亲属工作单位", "" });
            //rules.Add("F_Relative2_Guarder_Relation", new string[] { "第二亲属亲子关系", "" });
            //rules.Add("F_Relative3_Guarder_Relation", new string[] { "接送亲属亲子关系", "" });
            //rules.Add("F_Relative3_Name", new string[] { "接送亲属姓名", "" });
            //rules.Add("F_Relative3_Tel", new string[] { "接送亲属电话", "" });
            //rules.Add("F_Relative3_Guarder3", new string[] { "接送亲属文化程度", "" });
            //rules.Add("F_Relative3_Comp3", new string[] { "接送亲属工作单位", "" });
            //rules.Add("F_Guarder_Dw", new string[] { "监护人工作单位", "" });
            //rules.Add("F_Guarder_Wh", new string[] { "监护人文化程度", "" });
            //rules.Add("F_Guarder", new string[] { "监护人姓名", "" });
            //rules.Add("F_Guarder_Relation", new string[] { "监护关系", "" });
            //rules.Add("F_Guarder_Tel", new string[] { "监护人联系电话", "" });
            //rules.Add("F_Guarder_Nation", new string[] { "监护人国籍", "" });
            //rules.Add("F_Guarder_CredNum", new string[] { "监护人证件号", "" });
            //rules.Add("F_Guarder_CredType", new string[] { "监护人证件类型", "" });
            //rules.Add("F_Guarder_CredPhoto_Obve", new string[] { "监护人证件正面", "" });
            //rules.Add("F_Guarder_CredPhoto_Rever", new string[] { "监护人证件反面", "" });
            //rules.Add("F_Guarder_LinkType", new string[] { "监护人其他联系方式", "" });
            //rules.Add("F_Eat", new string[] { "吃饭", "" });
            //rules.Add("F_Relish", new string[] { "食欲", "" });
            //rules.Add("F_Incontinence", new string[] { "大小便", "" });
            //rules.Add("F_Dress", new string[] { "着装习惯", "" });
            //rules.Add("F_Sleep", new string[] { "午睡", "" });
            //rules.Add("F_Stripped", new string[] { "穿脱衣", "" });
            //rules.Add("F_Physique", new string[] { "体质", "" });
            //rules.Add("F_Life_Memo", new string[] { "生活其他", "" });
            //rules.Add("F_Prepay_Status", new string[] { "保额金", "" });
            //rules.Add("F_SundryFees_Status", new string[] { "学杂费ID", "" });
            //rules.Add("F_Auditor_Sec", new string[] { "总部审核人", "" });
            //rules.Add("F_AuditDTM_Sec", new string[] { "总部审核时间", "" });
            //rules.Add("F_Auditor_Sec_Remark", new string[] { "总部审核意见", "" });
            //rules.Add("F_History", new string[] { "报名操作流水", "" });
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
            List<EntrySignUp> list = ExcelToList<EntrySignUp>(Server.MapPath(filePath), rules);

            ///////////////////入库
            if (list == null)
                return Error("导入失败");
            app.import(list);
            return Success("导入成功。");
        }
    }
}