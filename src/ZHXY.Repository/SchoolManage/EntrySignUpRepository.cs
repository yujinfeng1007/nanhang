using System;
using System.Linq;
using ZHXY.Common;
using ZHXY.Domain;
using ZHXY.Domain.Entity;

namespace ZHXY.Repository
{
    public class EntrySignUpRepository : Data.Repository<EntrySignUp>, IEntrySignUpRepository
    {
        private string getStrNum(string num, int leng = 4)
        {
            var len = num.Length;
            for (var i = 0; i < leng - len; i++) num = "0" + num;
            return num;
        }

        public void AddLicence(EntrySignUp entity, SchoolBill bill, EntrySignUp entry)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                if (entry != null) db.Update(entry);
                db.Insert(entity);
                db.Insert(bill);

                //////////////////校易收接口
                var bm = "";
                var divis = db.FindEntity<SysOrganize>(t => t.F_Id == bill.F_Divis_ID);
                var grade = db.FindEntity<SysOrganize>(t => t.F_Id == bill.F_Grade_ID);
                var classOrg = db.FindEntity<SysOrganize>(t => t.F_Id == bill.F_Class_ID);
                if (!divis.IsEmpty()) bm += divis.F_FullName;
                if (!grade.IsEmpty()) bm += grade.F_FullName;
                if (!classOrg.IsEmpty()) bm += classOrg.F_FullName;

                db.Commit();
            }
        }

        /// <summary>
        ///     支付保额金动作
        /// </summary>
        /// <param name="keyValue">入学协议id</param>
        /// <returns></returns>
        public void DjPay(SchoolBill bill)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                //当前用户
                var user = OperatorProvider.Current;
                //入学报名表
                var e = db.FindEntity<EntrySignUp>(t => t.F_Id == bill.F_In);
                //协议调整中
                if ("7".Equals(e.F_Statu) || "8".Equals(e.F_Statu)) throw new Exception("该报名协议调整审核中,暂停缴费");
                e.F_Statu = "4"; //待审核

                //bill.F_Fees = e.F_Prepay_Fees;

                e.F_Prepay_Status = "2"; //保额金缴费状态
                e.F_PayTime = DateTime.Now; //保额金缴费时间
                //定金账单
                //School_Bill_Entity bill = db.FindEntity<School_Bill_Entity>(t => t.F_In == keyValue);
                e.F_Prepay_Fees_Fact = bill.F_Fees; //保额金应缴纳
                //用户表 监护人、学生
                //学生
                var u = db.FindEntity<SysUser>(t => t.F_Account == e.F_StudentNum);
                if (u == null)
                {
                    u = new SysUser();
                    u.F_Account = e.F_StudentNum;
                    u.F_DutyId = "studentDuty";
                    u.F_Gender = "1".Equals(e.F_Gender) ? true : false;
                    u.F_RoleId = "student";
                    u.F_MobilePhone = e.F_Phone;
                    u.F_RealName = e.F_Name;
                    u.F_OrganizeId = "1";
                    //分班的时候赋值
                    u.F_DepartmentId = e.F_Divis_ID;
                    u.F_NickName = e.F_Name;
                    u.F_IsAdministrator = false;
                    u.F_EnabledMark = true;
                    u.F_HeadIcon = e.F_FacePic_File;
                    u.Create();
                    var userLogOnEntity = new SysUserLogin();
                    userLogOnEntity.F_Id = u.F_Id;
                    userLogOnEntity.F_UserId = u.F_Id;
                    userLogOnEntity.F_UserSecretkey = Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                    //生成密码 证件号后6位
                    var pwd = "000000";
                    if (!e.F_CredNum.IsEmpty() && e.F_CredNum.Length >= 6)
                        pwd = e.F_CredNum.Substring(e.F_CredNum.Length - 6, 6);
                    pwd = pwd.ToLower().Replace("x", "0"); //密码x变为0
                    userLogOnEntity.F_UserPassword = Md5EncryptHelper
                        .Encrypt(
                            DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(pwd, 32), userLogOnEntity.F_UserSecretkey)
                                .ToLower(), 32).ToLower();
                    e.F_Students_ID = u.F_Id; //学生用户ID
                    db.Insert(u);
                    db.Insert(userLogOnEntity);
                }
                //UserEntity u = new UserEntity();

                //监护人
                //查询是否有该账号 有则不创建
                if (e.F_FamilyID.IsEmpty())
                {
                    var j = db.FindEntity<SysUser>(t => t.F_Account == e.F_Guarder_Tel);

                    if (j == null)
                    {
                        j = new SysUser();
                        j.F_Account = e.F_Guarder_Tel;
                        j.F_DutyId = "parentDuty";
                        j.F_Gender = "1".Equals(e.F_Gender) ? true : false;
                        j.F_RoleId = "parent";
                        j.F_MobilePhone = e.F_Guarder_Tel;
                        j.F_RealName = e.F_Guarder;
                        j.F_OrganizeId = "1";
                        //分班的时候赋值
                        j.F_DepartmentId = "parent";
                        j.F_NickName = e.F_Guarder;
                        j.F_IsAdministrator = false;
                        j.F_EnabledMark = true;
                        //j.F_HeadIcon = e.F_FacePic_File;
                        j.Create();

                        var userLogOnEntity2 = new SysUserLogin();
                        userLogOnEntity2.F_Id = j.F_Id;
                        userLogOnEntity2.F_UserId = j.F_Id;
                        userLogOnEntity2.F_UserSecretkey = Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                        //生成密码 监护人证件号后6位
                        var pwd2 = "000000";
                        if (!e.F_Guarder_CredNum.IsEmpty() && e.F_Guarder_CredNum.Length >= 6)
                            pwd2 = e.F_Guarder_CredNum.Substring(e.F_Guarder_CredNum.Length - 6, 6);
                        pwd2 = pwd2.ToLower().Replace("x", "0"); //密码x变为0
                        userLogOnEntity2.F_UserPassword = Md5EncryptHelper
                            .Encrypt(
                                DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(pwd2, 32),
                                    userLogOnEntity2.F_UserSecretkey).ToLower(), 32).ToLower();
                        db.Insert(j);
                        db.Insert(userLogOnEntity2);
                    }

                    e.F_FamilyID = j.F_Id; //家长ID
                }

                //入学协议表-算钱
                //保额金
                //e.F_Prepay_Fees_Must = e.F_Prepay_Fees + (e.F_Prepay_Fees_Change == null ? 0 : e.F_Prepay_Fees_Change);

                // //总计
                // e.F_Fee_Must = (e.F_SundryFees == null ? 0 : e.F_SundryFees)
                //     - (e.F_Prepay_Fees_Must == null ? 0 : e.F_Prepay_Fees_Must)
                // //+ (e.F_SundryFees_Change == null ? 0 : e.F_SundryFees_Change)
                // //+(e.F_SundryFees_Change_Ever == null ? 0 : e.F_SundryFees_Change_Ever)
                // + (e.F_ComeBack_Fees == null ? 0 : e.F_ComeBack_Fees);
                // //+ (e.F_ComeBack_Change == null ? 0 : e.F_ComeBack_Change)
                //// + (e.F_ComeBack_Change_Ever == null ? 0 : e.F_ComeBack_Change_Ever);

                //奖学金
                //if ("按学年".Equals(e.F_Burse_Type))
                //{
                //    e.F_Fee_Must = e.F_Fee_Must - e.F_Burse_Fees;
                //}

                db.Update(e);

                ////学生表

                var stu = db.FindEntity<Student>(t => t.F_StudentNum.Equals(e.F_StudentNum));
                var s = new Student();
                if (stu != null) s = ExtObj.ClonePropValue(stu, s);
                s.F_InitNum = e.F_InitNum;
                s.F_StudentNum = e.F_StudentNum;
                s.F_Users_ID = u.F_Id;
                s.F_Year = Convert.ToString(e.F_Year);
                s.F_Divis_ID = e.F_Divis_ID;
                s.F_Grade_ID = e.F_Grade_ID;
                s.F_INYear = e.F_INYear;
                s.F_Name = e.F_Name;
                s.F_Gender = e.F_Gender;
                s.F_CredType = e.F_CredType;
                s.F_CredNum = e.F_CredNum;
                s.F_Guarder = e.F_Guarder;
                s.F_Guarder_Tel = e.F_Guarder_Tel;
                s.F_Stu_Type = e.F_Stu_Type;
                s.F_ComeFrom_Type = e.F_ComeFrom_Type;
                s.F_ComeFrom = e.F_ComeFrom;
                s.F_ComeFrom_Area = e.F_ComeFrom_Area;
                s.F_ComeFrom_City = e.F_ComeFrom_City;
                s.F_ComeFrom_Province = e.F_ComeFrom_Province;
                s.F_ComeBackSite = e.F_ComeBackSite;
                s.F_Health = e.F_Health;
                s.F_SchoolType = e.F_SchoolType;
                s.F_InClass = e.F_InClass;
                s.F_ComeBackType = e.F_ComeBackType;
                s.F_ComeBackPro = e.F_ComeBackProv;
                s.F_ComeBackCity = e.F_ComeBackCity;
                s.F_ComeBackArea = e.F_ComeBackArea;
                s.F_ComeBackRoute_ID = e.F_ComeBackRoute_ID;
                s.F_Subjects_ID = e.F_Subjects_ID;
                s.F_SchoolType = e.F_SchoolType;
                s.F_DepartmentId = e.F_Divis_ID;
                s.F_CurStatu = "1"; //在校状态
                //分班的时候赋值
                //s.F_Class_ID = bill.F_Class_ID;
                if (stu != null)
                {
                    s.F_Id = stu.F_Id;
                    s.Modify(s.F_Id);
                    db.Update(s);
                }
                else
                {
                    s.Create();
                    db.Insert(s);
                }
                //账单表

                //流水表
                //School_TollFlow_Entity ste = new School_TollFlow_Entity();
                //ste.F_Divis_ID = bill.F_Divis_ID;
                //ste.F_Grade_ID = bill.F_Grade_ID;
                //ste.F_Class_ID = bill.F_Class_ID;
                //ste.F_Students_ID = u.F_Id;
                //ste.F_StudentNum = s.F_StudentNum;
                //ste.F_Fee = bill.F_Fees;
                //ste.F_Users_ID = bill.F_Users_ID;
                //ste.F_Users_Name = bill.F_Users_Name;
                //ste.F_PayType = bill.F_PayType;
                //ste.F_BankCode = bill.F_BankCode;
                //ste.F_BankName = bill.F_BankName;
                //ste.F_PayNum = bill.F_PayNum;
                //ste.F_Toll_Account = bill.F_Toll_Account;
                //ste.F_Toll_DTM = bill.F_Toll_DTM;
                //ste.F_Toll_Statu = bill.F_Toll_Statu;
                //ste.Create();

                var bill_tmp = new SchoolBill();
                //bill_tmp.F_TollFlow_Id = ste.F_Id;
                bill_tmp.F_Id = bill.F_Id;
                //缴费状态
                bill_tmp.F_Charge_Status = "2";
                //到账状态
                bill_tmp.F_Toll_Statu = "2";
                //确认状态
                bill_tmp.F_Confirm_Statu = "2";
                //实付款
                bill_tmp.F_Pay = bill.F_In_Pay;
                //欠付款
                bill_tmp.F_Not_Pay = 0;

                bill.F_Users_ID = OperatorProvider.Current.UserId;
                bill.F_Users_Name = OperatorProvider.Current.UserName;
                //db.Insert(ste);
                db.Update(bill_tmp);
                ////生成学杂费账单
                //School_Bill_Entity xzBill = new School_Bill_Entity();
                //xzBill.F_ChargeNo = "b" + RandomHelper.CreateNo();
                //xzBill.F_Student_ID = u.F_Id;
                //xzBill.F_StudentNum = s.F_StudentNum;
                //xzBill.F_Student_Name = s.F_Name;
                //xzBill.F_Divis_ID = e.F_Divis_ID;
                //xzBill.F_Grade_ID = e.F_Grade_ID;
                //xzBill.F_In_No = e.F_InitNum;
                //xzBill.F_In = e.F_Id;
                //xzBill.F_Subjects_ID = e.F_Subjects_ID;
                //xzBill.F_Year = Convert.ToString(e.F_Year);
                //xzBill.F_Charge_Status = "1";
                //xzBill.F_Fees = e.F_Fee_Must;
                //xzBill.F_Fees_Name = "学杂费";
                //xzBill.F_Charge_Type = "学杂费";
                //xzBill.F_Fees_Desc = e.F_Year + "学杂费";

                //xzBill.F_Toll_Statu = "2";
                //xzBill.F_Confirm_Statu = "2";
                //xzBill.F_In_Pay = e.F_Fee_Must;
                //xzBill.F_Pay = 0;
                //xzBill.F_Not_Pay = e.F_Fee_Must;
                //xzBill.F_Tax_Statu = "false";
                //if(e.F_Charge_mode.Equals("分学年"))
                //    xzBill.F_Year_Period = "全年";
                //else
                //    xzBill.F_Year_Period = "上学期";
                ////已抵扣保额金
                //xzBill.F_If_PrePay = "true";
                //xzBill.Create();
                //db.Insert(xzBill);

                //Sys_MsgModel model = new Sys_MsgModel();
                //model.F_StuName = s.F_Name;
                //model.F_Divis_ID = s.F_Divis_ID;
                //model.F_Deposit = bill_tmp.F_Pay;
                ////model.F_InitNum = s.F_InitNum;
                //model.F_StudentNum = s.F_StudentNum;
                //model.MsgModelId = "82c7837e-3711-4c8b-a729-79a7b16061e4";
                //string F_Module= new Sys_ShortMsg_Stay_Repository().GetMsg(model);
                ////短信表
                //Sys_ShortMsg_Stay_Entity stayentity = new Sys_ShortMsg_Stay_Entity();
                //if (e.F_RegUserType== "teacherDuty")
                //{
                //    stayentity.F_Mobile = e.F_RegisterPhone;
                //    stayentity.F_ShortMsg = F_Module;
                //    stayentity.F_Send_Statu = "3";
                //    stayentity.Create();
                //    db.Insert(stayentity);
                //}
                //stayentity.F_Mobile = e.F_Guarder_Tel;
                //stayentity.F_ShortMsg = F_Module;
                //stayentity.F_Send_Statu = "3";
                //stayentity.Create();
                //db.Insert(stayentity);

                db.Commit();
            }
        }

        public void ChangeMoney(EntrySignUp entity, string F_Charge_Type)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                if ("dj".Equals(F_Charge_Type))
                    F_Charge_Type = "保额金";
                else
                    F_Charge_Type = "学杂费";
                var bill = db.FindEntity<SchoolBill>(
                    t => t.F_In == entity.F_Id && F_Charge_Type.Equals(t.F_Charge_Type));
                //School_EntrySignUp_Entity old = db.FindEntity<School_EntrySignUp_Entity>(t => t.F_Id == entity.F_Id);
                //还未缴纳保额金，生成订单
                if (bill == null && "保额金".Equals(F_Charge_Type))
                {
                    db.Update(entity);
                    db.Commit();
                    return;
                }

                if (!bill.IsEmpty())
                {
                    if ("2".Equals(bill.F_Charge_Status))
                        throw new Exception("该账单已缴清，不能修改");
                    //修改学费账单
                    if ("保额金".Equals(bill.F_Charge_Type))
                    {
                        bill.F_In_Pay = entity.F_Prepay_Fees_Must;
                        bill.F_Fees = entity.F_Prepay_Fees_Must;
                    }
                    else
                    {
                        //entity.F_Fee_Must = ((entity.F_SundryFees == null ? 0 : entity.F_SundryFees)
                        //    + (entity.F_SundryFees_Change == null ? 0 : entity.F_SundryFees_Change)
                        //    + (entity.F_SundryFees_Change_Ever == null ? 0 : entity.F_SundryFees_Change_Ever))
                        //    + ((entity.F_ComeBack_Fees == null ? 0 : entity.F_ComeBack_Fees)
                        //    + (entity.F_ComeBack_Change == null ? 0 : entity.F_ComeBack_Change)
                        //    + (entity.F_ComeBack_Change_Ever == null ? 0 : entity.F_ComeBack_Change_Ever))
                        //    - (old.F_Prepay_Fees_Must == null ? 0 : old.F_Prepay_Fees_Must);
                        //奖学金
                        //if ("按学年".Equals(entity.F_Burse_Type))
                        //{
                        //    entity.F_Fee_Must = entity.F_Fee_Must - entity.F_Burse_Fees;
                        //}
                        bill.F_In_Pay = entity.F_Fee_Must;
                        bill.F_Fees = entity.F_Fee_Must;
                    }
                }
                else
                {
                    throw new Exception("没有该账单，请联系管理员");
                }

                db.Update(bill);
                db.Update(entity);
                db.Commit();
            }
        }

        public void UpdateInfoForm(EntrySignUp entity, string F_Students_ID)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                //School_EntrySignUp_Entity e = db.FindEntity<School_EntrySignUp_Entity>(entity.F_Id);
                var s = db.FindEntity<Student>(t => t.F_Users_ID == F_Students_ID);
                if (s != null)
                {
                    s = ExtObj.ClonePropValue(entity, s);
                    s.F_Year = Convert.ToString(entity.F_Year);
                    s.Modify(s.F_Id);
                    db.Update(s);
                }

                entity.Modify(entity.F_Id);
                db.Update(entity);
                db.Commit();
            }
        }

        public decimal costCount(string keyValue)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                //School_EntrySignUp_Entity e = db.FindEntity<School_EntrySignUp_Entity>(t => t.F_Id == keyValue);
                //if (e != null)
                //{
                //}

                //db.Update(entity);
                //db.Commit();
                return 0;
            }
        }

        public void updateandmsg(EntrySignUp entity)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                var e = QueryAsNoTracking().Where(t => t.F_Id.Equals(entity.F_Id)).First();
                //完善审核
                if (!string.IsNullOrEmpty(entity.F_Date_Status))
                {
                    if (entity.F_Date_Status == "3")
                    {
                        var model = new SysMsgModel();
                        model.F_StuName = e.F_Name;
                        //model.F_Divis_ID = e.F_Divis_ID;
                        model.F_WebsitePhone = "http://zjrhxx.ronghuai.cn:1312";
                        model.MsgModelId = "24da12f7-e50f-46ed-852a-17ec1c9dbe9a";
                        var F_Module = new ShortMsgStayRepository().GetMsg(model);

                        var stayentity = new ShortMsgStay();
                        stayentity.F_Mobile = e.F_RegisterPhone;
                        stayentity.F_ShortMsg = F_Module;
                        stayentity.F_Send_Statu = "3";
                        stayentity.Create();
                        db.Insert(stayentity);
                    }
                    else if (entity.F_Date_Status == "2")
                    {
                        var model = new SysMsgModel();
                        model.F_StuName = e.F_Name;
                        model.F_StudentNum = e.F_StudentNum;
                        model.F_WebsitePhone = "http://zjrhxx.ronghuai.cn:1312";
                        model.MsgModelId = "93621603-dc49-4aed-81db-08f72dea9bf6";
                        var F_Module = new ShortMsgStayRepository().GetMsg(model);

                        var stayentity = new ShortMsgStay();
                        stayentity.F_Mobile = e.F_RegisterPhone;
                        stayentity.F_ShortMsg = F_Module;
                        stayentity.F_Send_Statu = "3";
                        stayentity.Create();
                        db.Insert(stayentity);
                    }
                }
                //报名审核
                else if (!string.IsNullOrEmpty(entity.F_Statu))
                {
                    //Sys_MsgModel model = new Sys_MsgModel();
                    //model.F_Divis_ID = e.F_Divis_ID;
                    //model.F_StuName = e.F_Name;
                    //model.F_WebsitePhone = "http://zjrhxx.ronghuai.cn:1312";
                    if (entity.F_Statu == "6")
                    {
                        var model = new SysMsgModel();
                        //model.F_Divis_ID = e.F_Divis_ID;
                        model.F_StuName = e.F_Name;
                        model.F_WebsitePhone = "http://zjrhxx.ronghuai.cn:1312";
                        model.MsgModelId = "fd3a9218-65c1-4483-999a-4f4251c07265";
                        var F_Module = new ShortMsgStayRepository().GetMsg(model);

                        var stayentity = new ShortMsgStay();
                        stayentity.F_Mobile = e.F_RegisterPhone;
                        stayentity.F_ShortMsg = F_Module;
                        stayentity.F_Send_Statu = "3";
                        stayentity.Create();
                        db.Insert(stayentity);
                    }

                    if (entity.F_Statu == "5")
                    {
                        var bill = db.FindEntity<SchoolBill>(t =>
                            t.F_Divis_ID == e.F_Divis_ID && t.F_StudentNum == e.F_StudentNum &&
                            t.F_In_No == e.F_InitNum && t.F_In == e.F_Id && t.F_Charge_Type == "学杂费");
                        if (bill == null)
                        {
                            //总计
                            entity.F_Fee_Must = (e.F_SundryFees == null ? 0 : e.F_SundryFees)
                                                - (e.F_Prepay_Fees_Must == null ? 0 : e.F_Prepay_Fees_Must)
                                                //+ (e.F_SundryFees_Change == null ? 0 : e.F_SundryFees_Change)
                                                //+(e.F_SundryFees_Change_Ever == null ? 0 : e.F_SundryFees_Change_Ever)
                                                + (e.F_ComeBack_Fees == null ? 0 : e.F_ComeBack_Fees);
                            //+ (e.F_ComeBack_Change == null ? 0 : e.F_ComeBack_Change)
                            // + (e.F_ComeBack_Change_Ever == null ? 0 : e.F_ComeBack_Change_Ever);
                            //生成学杂费账单
                            var xzBill = new SchoolBill();
                            var stu = db.FindEntity<Student>(t => t.F_StudentNum.Equals(e.F_StudentNum));
                            var u = db.FindEntity<SysUser>(t => t.F_Account == e.F_StudentNum);
                            xzBill.F_ChargeNo = "b" + NumberBuilder.Build_18bit();
                            xzBill.F_Student_ID = u.F_Id;
                            xzBill.F_StudentNum = stu.F_StudentNum;
                            xzBill.F_Student_Name = stu.F_Name;
                            xzBill.F_Divis_ID = e.F_Divis_ID;
                            xzBill.F_Grade_ID = e.F_Grade_ID;
                            xzBill.F_In_No = e.F_InitNum;
                            xzBill.F_In = e.F_Id;
                            xzBill.F_Subjects_ID = e.F_Subjects_ID;
                            xzBill.F_Year = Convert.ToString(e.F_Year);
                            xzBill.F_Charge_Status = "6";
                            xzBill.F_Fees = entity.F_Fee_Must;
                            xzBill.F_Fees_Name = "学杂费";
                            xzBill.F_Charge_Type = "学杂费";
                            xzBill.F_Fees_Desc = e.F_Year + "学杂费";

                            xzBill.F_Toll_Statu = "2";
                            xzBill.F_Confirm_Statu = "2";
                            xzBill.F_In_Pay = entity.F_Fee_Must;
                            xzBill.F_Pay = 0;
                            xzBill.F_Not_Pay = entity.F_Fee_Must;
                            xzBill.F_Tax_Statu = "false";
                            if (e.F_Charge_mode.Equals("分学年"))
                                xzBill.F_Year_Period = "全年";
                            else
                                xzBill.F_Year_Period = "上学期";
                            //已抵扣保额金
                            xzBill.F_If_PrePay = "true";
                            xzBill.Create();
                            db.Insert(xzBill);
                        }

                        var model = new SysMsgModel();
                        //model.F_Divis_ID = e.F_Divis_ID;
                        model.F_StuName = e.F_Name;
                        model.F_WebsitePhone = "http://zjrhxx.ronghuai.cn:1312";
                        //model.F_InitNum = e.F_InitNum;
                        model.F_StudentNum = e.F_StudentNum;
                        model.MsgModelId = "deb1e2c1-8a1f-40a9-a873-e83516dd81ed";
                        var F_Module = new ShortMsgStayRepository().GetMsg(model);

                        var stayentity = new ShortMsgStay();
                        stayentity.F_Mobile = e.F_RegisterPhone;
                        stayentity.F_ShortMsg = F_Module;
                        stayentity.F_Send_Statu = "3";
                        stayentity.Create();
                        db.Insert(stayentity);

                        ////////////////////校易收接口
                        //string bm = "";
                        //OrganizeEntity divis = db.FindEntity<OrganizeEntity>(t => t.F_Id == xzBill.F_Divis_ID);
                        //OrganizeEntity grade = db.FindEntity<OrganizeEntity>(t => t.F_Id == xzBill.F_Grade_ID);
                        //OrganizeEntity classOrg = db.FindEntity<OrganizeEntity>(t => t.F_Id == xzBill.F_Class_ID);
                        //if (!Ext.IsEmpty(divis))
                        //{
                        //    bm += divis.F_FullName;
                        //}
                        //if (!Ext.IsEmpty(grade))
                        //{
                        //    bm += grade.F_FullName;
                        //}
                        //if (!Ext.IsEmpty(classOrg))
                        //{
                        //    bm += classOrg.F_FullName;
                        //}

                        //new School_Bill_Repository().makeK12OrderJson(xzBill, e, bm);
                    }
                    //string F_Module = new Sys_ShortMsg_Stay_Repository().GetMsg(model);

                    //Sys_ShortMsg_Stay_Entity stayentity = new Sys_ShortMsg_Stay_Entity();
                    //stayentity.F_Mobile = e.F_Phone;
                    //stayentity.F_ShortMsg = F_Module;
                    //stayentity.F_Send_Statu = "3";
                    //stayentity.Create();
                    //db.Insert(stayentity);
                }
                //签字审核
                else if (!string.IsNullOrEmpty(entity.F_Signed_License_Status))
                {
                    if (entity.F_Signed_License_Status == "4")
                    {
                        var model = new SysMsgModel();
                        model.F_StuName = e.F_Name;
                        model.F_WebsitePhone = "http://zjrhxx.ronghuai.cn:1312";
                        model.MsgModelId = "0fcf2690-0930-4270-a91f-6590aa6b6354";
                        var F_Module = new ShortMsgStayRepository().GetMsg(model);

                        var stayentity = new ShortMsgStay();
                        stayentity.F_Mobile = e.F_RegisterPhone;
                        stayentity.F_ShortMsg = F_Module;
                        stayentity.F_Send_Statu = "3";
                        stayentity.Create();
                        db.Insert(stayentity);
                    }
                    else if (entity.F_Signed_License_Status == "3")
                    {
                        var model = new SysMsgModel();
                        model.F_StuName = e.F_Name;
                        //model.F_StudentNum= e.F_StudentNum;
                        //model.F_WebsitePhone = "http://zjrhxx.ronghuai.cn:1312";
                        model.MsgModelId = "9742467c-23dc-4158-8358-aabe32670cb0";
                        var F_Module = new ShortMsgStayRepository().GetMsg(model);

                        var stayentity = new ShortMsgStay();
                        stayentity.F_Mobile = e.F_RegisterPhone;
                        stayentity.F_ShortMsg = F_Module;
                        stayentity.F_Send_Statu = "3";
                        stayentity.Create();
                        db.Insert(stayentity);
                    }
                }

                db.Update(entity);
                db.Commit();
            }
        }

        public void updateanddel(EntrySignUp entity)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                db.Update(entity);
                db.Delete<SysUser>(t => t.F_Account.Equals(entity.F_StudentNum));
                db.Delete<SysUser>(t => t.F_Account.Equals(entity.F_Guarder_Tel));
                db.Delete<Student>(t => t.F_StudentNum.Equals(entity.F_StudentNum));
                db.Commit();
            }
        }

        public void UpdateLicence(EntrySignUp entity)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                var stu = db.FindEntity<Student>(t => t.F_StudentNum.Equals(entity.F_StudentNum));
                if (stu != null)
                {
                    stu.F_ComeBackArea = entity.F_ComeBackArea;
                    stu.F_ComeBackCity = entity.F_ComeBackCity;
                    db.Update(stu);
                }

                var xzBill = db.FindEntity<SchoolBill>(t =>
                    t.F_StudentNum == entity.F_StudentNum && t.F_Charge_Type == "学杂费");
                if (xzBill != null)
                {
                    xzBill.F_Fees = entity.F_Fee_Must;
                    xzBill.F_In_Pay = entity.F_Fee_Must;
                    xzBill.F_Not_Pay = entity.F_Fee_Must;
                    db.Update(xzBill);
                }

                db.Update(entity);
                db.Commit();
            }
        }

        public void UpdateLic(EntrySignUp entity)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                var e = db.FindEntity<EntrySignUp>(t => t.F_Id.Equals(entity.F_Id));
                var stu = db.FindEntity<Student>(t => t.F_StudentNum.Equals(e.F_StudentNum));
                if (stu != null)
                {
                    stu.F_ComeBackArea = entity.F_ComeBackArea;
                    stu.F_ComeBackCity = entity.F_ComeBackCity;
                    db.Update(stu);
                }

                db.Update(entity);
                db.Commit();
            }
        }
    }
}