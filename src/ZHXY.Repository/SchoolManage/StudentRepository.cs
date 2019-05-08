using System;
using System.Collections.Generic;
using System.Linq;
using ZHXY.Common;
using ZHXY.Domain;
using ZHXY.Domain.Entity;

namespace ZHXY.Repository
{
    public class StudentRepository : Data.Repository<Student>, IStudentRepository
    {
        private string getStuNum(int F_Year, string F_Divis_ID, IUnitWork db)
        {
            //using (var db = new RepositoryBase().BeginTrans())
            //{
            //var db = new RepositoryBase();
            var org = db.FindEntity<SysOrganize>(t => t.F_Id == F_Divis_ID);
            var seed = db.FindEntity<NoSeed>(
                t => t.F_Divis == F_Divis_ID && t.F_Year == F_Year && t.F_Type == "student");

            var no = org.F_EnCode + F_Year;
            if (seed == null)
            {
                seed = new NoSeed();
                seed.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                seed.F_Divis = F_Divis_ID;
                seed.F_Year = F_Year;
                seed.F_Type = "student";
                seed.F_No = 1;
                db.Insert(seed);
            }
            else
            {
                seed.F_No = seed.F_No + 1;
                db.Update(seed);
            }

            no = no + getStrNum(Convert.ToString(seed.F_No));
            //db.Commit();
            return no;
            //}
        }

        private string getStrNum(string num, int leng = 4)
        {
            var len = num.Length;
            for (var i = 0; i < leng - len; i++) num = "0" + num;
            return num;
        }

        private readonly string appid = Configs.GetValue("appid");
        private readonly string CallUrlAdd = Configs.GetValue("CallUrlAdd");
        private readonly string CallUrlUpt = Configs.GetValue("CallUrlUpt");
        private readonly WebHelper webhelp = new WebHelper();

        public new void Delete(string ids)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                string[] F_Id = ids.Split('|');
                for (int i = 0; i < F_Id.Length; i++)
                {
                    var model = db.Find<Student>(F_Id[i]);
                    if (model != null)
                    {
                        db.Delete<SysUser>(t => t.F_Id == model.F_Users_ID);
                        db.Delete(model);

                        bool isPost = true;
                        string parameters = "sysid=" + F_Id[i] + "&amp;appid=" + Configs.GetValue("appid") + "";
                        var Result = WebHelper.SendRequest(Configs.GetValue("CallUrlDel"), parameters, isPost, "application/json");
                    }
                }
                db.Commit();
            }
        }

        public void AddDatas(List<Student> datas)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                try
                {
                    //var list = new List<string>();
                    //var mod = "<p>导入错误如下：</p>";
                    //var k = 1;
                    foreach (var data in datas)
                        if (data.F_Id.IsEmpty())
                            try
                            {
                                var entity = new EntrySignUp();
                                //注册人信息
                                var Oper = OperatorProvider.Current;
                                entity.F_RegUsers_ID = Oper.UserId;
                                entity.F_RegUserType = Oper.Duty;
                                entity.F_RegisterName = Oper.UserName;
                                entity.F_RegisterNum = Oper.UserCode;
                                entity.F_RegisterPhone = Oper.MobilePhone;

                                entity = ExtObj.ClonePropValue(data, entity);

                                //entity.F_Subjects_ID = data.F_Subjects_ID;
                                //entity.F_ComeFrom_Province = data.F_ComeFrom_Province;
                                //entity.F_ComeFrom_City = data.F_ComeFrom_City;
                                //entity.F_ComeFrom_Area = data.F_ComeFrom_Area;
                                //entity.F_Divis_ID = data.F_Divis_ID;
                                entity.F_Year = Convert.ToInt32(data.F_Year);
                                ////学杂费
                                //entity.F_SundryFees = data.F_SundryFees;
                                ////接送费
                                //entity.F_ComeBack_Fees = data.F_ComeBack_Fees;
                                ////应缴
                                //entity.F_Fee_Must = data.F_Fee_Must;
                                ////实缴
                                //entity.F_Fee_Fact = data.F_Fee_Fact;
                                //签字协议状态
                                entity.F_Signed_License_Status = "1";
                                //学费缴纳状态 F_Sundry_Status
                                entity.F_Sundry_Status = "1";
                                //保额金缴纳状态 F_Prepay_Status
                                entity.F_Prepay_Status = "1";
                                //信息完善状态 F_Date_Status
                                entity.F_Date_Status = "1";
                                //报名协议状态
                                entity.F_Statu = "4";
                                //报名序号
                                entity.F_InitNum = "s" + NumberBuilder.Build_18bit();
                                entity.F_DepartmentId = entity.F_Divis_ID;
                                if (string.IsNullOrEmpty(data.F_StudentNum))
                                {
                                    var org = db.FindEntity<SysOrganize>(t => t.F_Id == entity.F_Divis_ID);
                                    var seed = db.FindEntity<NoSeed>(t =>
                                        t.F_Divis == entity.F_Divis_ID && t.F_Year == entity.F_Year &&
                                        t.F_Type == "student");

                                    var no = org.F_EnCode + entity.F_Year;
                                    if (seed == null)
                                    {
                                        seed = new NoSeed();
                                        seed.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                                        seed.F_Divis = entity.F_Divis_ID;
                                        seed.F_Year = entity.F_Year;
                                        seed.F_Type = "student";
                                        seed.F_No = 1;
                                        db.Insert(seed);
                                    }
                                    else
                                    {
                                        seed.F_No = seed.F_No + 1;
                                        db.Update(seed);
                                    }

                                    var stuNo = no + getStrNum(Convert.ToString(seed.F_No));
                                    //string stuNo = getStuNum(Convert.ToInt32(entity.F_Year), entity.F_Divis_ID,db);
                                    entity.F_StudentNum = stuNo;
                                }
                                else
                                {
                                    var entry = new EntrySignUpRepository().QueryAsNoTracking()
                                        .Where(t => t.F_StudentNum.Equals(data.F_StudentNum));
                                    //是否被使用
                                    if (!entry.Any())
                                    {
                                        var org = db.FindEntity<SysOrganize>(t => t.F_Id == entity.F_Divis_ID);
                                        var no = org.F_EnCode + entity.F_Year;
                                        var len = org.F_EnCode.Length + 4;
                                        if (data.F_StudentNum.Substring(0, len) == no)
                                        {
                                            var seed = db.FindEntity<NoSeed>(t =>
                                                t.F_Divis == entity.F_Divis_ID && t.F_Year == entity.F_Year &&
                                                t.F_Type == "student");
                                            seed.F_No = Convert.ToInt32(
                                                            data.F_StudentNum.Substring(data.F_StudentNum.Length - 4,
                                                                4)) + 1;
                                            db.Update(seed);
                                        }
                                        else
                                        {
                                            throw new Exception("学生姓名为'" + data.F_Name + "'新增失败（学号规则错误）");
                                            //list.Add("<p>" + k + ". 学生姓名为'" + data.F_Name + "'新增失败（学号规则错误）</p> ");
                                            //k++;
                                            //continue;
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("学生姓名为'" + data.F_Name + "'新增失败（学号已被使用）");
                                        //list.Add("<p>" + k + ". 学生姓名为'" + data.F_Name + "'新增失败（学号已被使用）</p> ");
                                        //k++;
                                        //continue;
                                    }
                                }

                                entity.Create();

                                //data.Create();
                                var user = new SysUser
                                {
                                    F_Id = Guid.NewGuid().ToString(),
                                    F_Account = data.F_StudentNum,
                                    F_DutyId = "studentDuty",
                                    F_Gender = Convert.ToBoolean(Convert.ToInt32(data.F_Gender)),
                                    F_RoleId = "student",
                                    F_MobilePhone = data.F_Tel,
                                    F_RealName = data.F_Name,
                                    F_OrganizeId = "1",
                                    //分班的时候赋值
                                    //u.F_DepartmentId
                                    F_NickName = data.F_Name,
                                    F_IsAdministrator = false,
                                    F_EnabledMark = true,
                                    F_HeadIcon = data.F_FacePic_File
                                };
                                var userLogOnEntity = new SysUserLogin
                                {
                                    F_Id = user.F_Id,
                                    F_UserId = user.F_Id,
                                    F_UserSecretkey = Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower()
                                };
                                userLogOnEntity.F_UserPassword = Md5EncryptHelper
                                    .Encrypt(
                                        DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt("123456", 32).ToLower(),
                                            userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                                db.Insert(user);
                                db.Insert(userLogOnEntity);

                                var isPost = true;
                                var F_Birthday = "";
                                if (user.F_Birthday != null) F_Birthday = user.F_Birthday.ToDateTimeString();
                                var parameters = "sysid=" + user.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                                 user.F_RealName + "&amp;username=" + user.F_Account +
                                                 "&amp;password=" + userLogOnEntity.F_UserPassword +
                                                 "&amp;sysgroupid=" + user.F_DepartmentId + "&amp;headicon=" +
                                                 user.F_HeadIcon + "&amp;birthday=" + F_Birthday + "";
                                WebHelper.SendRequest(CallUrlAdd, parameters, isPost, "application/json");
                                data.F_Users_ID = user.F_Id;

                                var j = db.FindEntity<SysUser>(t => t.F_Account == data.F_Guarder_Tel);

                                if (j == null)
                                {
                                    j = new SysUser();
                                    j.F_Account = data.F_Guarder_Tel;
                                    j.F_DutyId = "parentDuty";
                                    j.F_Gender = "1".Equals(data.F_Gender) ? true : false;
                                    j.F_RoleId = "parent";
                                    j.F_MobilePhone = data.F_Guarder_Tel;
                                    j.F_RealName = data.F_Guarder;
                                    j.F_OrganizeId = "1";
                                    //分班的时候赋值
                                    j.F_DepartmentId = "parent";
                                    j.F_NickName = data.F_Guarder;
                                    j.F_IsAdministrator = false;
                                    j.F_EnabledMark = true;
                                    //j.F_HeadIcon = e.F_FacePic_File;
                                    j.Create();

                                    var userLogOnEntity2 = new SysUserLogin();
                                    userLogOnEntity2.F_Id = j.F_Id;
                                    userLogOnEntity2.F_UserId = j.F_Id;
                                    userLogOnEntity2.F_UserSecretkey =
                                        Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                                    //生成密码 监护人证件号后6位
                                    var pwd2 = "000000";
                                    if (!data.F_Guarder_CredNum.IsEmpty() && data.F_Guarder_CredNum.Length >= 6)
                                        pwd2 = data.F_Guarder_CredNum.Substring(data.F_Guarder_CredNum.Length - 6, 6);
                                    pwd2 = pwd2.ToLower().Replace("x", "0"); //密码x变为0
                                    userLogOnEntity2.F_UserPassword = Md5EncryptHelper
                                        .Encrypt(
                                            DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(pwd2, 32),
                                                userLogOnEntity2.F_UserSecretkey), 32).ToLower();
                                    db.Insert(j);
                                    db.Insert(userLogOnEntity2);

                                    var jus = new SysUserRole();
                                    jus.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                                    jus.F_User = j.F_Id;
                                    jus.F_Role = j.F_RoleId;
                                    db.Insert(jus);

                                    if (j.F_Birthday != null)
                                        F_Birthday = j.F_Birthday.ToDateTimeString();
                                    else F_Birthday = "";
                                    parameters = "sysid=" + j.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                                 j.F_RealName + "&amp;username=" + j.F_Account + "&amp;password=" +
                                                 userLogOnEntity2.F_UserPassword + "&amp;sysgroupid=" +
                                                 j.F_DepartmentId + "&amp;headicon=" + j.F_HeadIcon + "&amp;birthday=" +
                                                 F_Birthday + "";
                                    WebHelper.SendRequest(CallUrlAdd, parameters, isPost, "application/json");

                                    data.F_Users_ID = user.F_Id;
                                }

                                entity.F_FamilyID = j.F_Id; //家长ID
                                db.Insert(entity);

                                //School_Students_Entity s = new School_Students_Entity();
                                //s = ExtObj.clonePropValue(data, s);
                                data.Create();
                                data.F_InitNum = entity.F_InitNum;
                                data.F_StudentNum = entity.F_StudentNum;
                                db.Insert(data);
                            }
                            catch
                            {
                                throw new Exception("学生姓名为'" + data.F_Name + "'新增失败");
                                //list.Add("<p>" + k + ". 学生姓名为'" + data.F_Name + "'新增失败</p> ");
                                //k++;
                                //continue;
                            }
                        else
                            try
                            {
                                //School_Students_Entity stuentity = db.FindEntity<School_Students_Entity>(t => t.F_Id == data.F_Id); //GetForm(data.F_Id);

                                var entity =
                                    db.FindEntity<EntrySignUp>(t =>
                                        t.F_InitNum ==
                                        data.F_InitNum); //new School_EntrySignUp_App().GetFormByF_InitNum(stuentity.F_InitNum);

                                entity = ExtObj.ClonePropValue(data, entity);

                                //stuentity = ExtObj.clonePropValue(data, stuentity);
                                //stuentity.F_Id = data.F_Id;
                                var entrystu = new StudentRepository().QueryAsNoTracking()
                                    .Where(t => t.F_StudentNum.Equals(entity.F_StudentNum));
                                if (entrystu.Any())
                                {
                                    if (entrystu.First().F_Id != data.F_Id)
                                    {
                                        throw new Exception("学生姓名为'" + entity.F_Name + "'更新失败（学号已被使用）");
                                        //list.Add("<p>" + k + ". 学生姓名为'" + entity.F_Name + "'更新失败（学号已被使用）</p> ");
                                        //k++;
                                        //continue;
                                    }
                                }
                                else
                                {
                                    var org = db.FindEntity<SysOrganize>(t => t.F_Id == entity.F_Divis_ID);
                                    var no = org.F_EnCode + entity.F_Year;
                                    var len = org.F_EnCode.Length + 4;
                                    if (entity.F_StudentNum.Substring(0, len) == no)
                                    {
                                        var F_Year = Convert.ToInt32(entity.F_Year);
                                        var seed = db.FindEntity<NoSeed>(t =>
                                            t.F_Divis == entity.F_Divis_ID && t.F_Year == F_Year &&
                                            t.F_Type == "student");
                                        seed.F_No = Convert.ToInt32(
                                                        entity.F_StudentNum.Substring(entity.F_StudentNum.Length - 4,
                                                            4)) + 1;
                                        db.Update(seed);

                                        //getStuNum(Convert.ToInt32(data.F_Year), data.F_Divis_ID);
                                    }
                                    else
                                    {
                                        throw new Exception("学生姓名为'" + entity.F_Name + "'更新失败（学号规则错误）");
                                        //list.Add("<p>" + k + ". 学生姓名为'" + entity.F_Name + "'更新失败（学号规则错误）</p> ");
                                        //k++;
                                        //continue;
                                    }
                                }

                                var u = db.FindEntity<SysUser>(t =>
                                    t.F_Id == data.F_Users_ID); //new UserApp().GetForm(stuentity.F_Users_ID);
                                u.F_Account = entity.F_StudentNum;
                                u.F_Gender = Convert.ToBoolean(Convert.ToInt32(entity.F_Gender));
                                u.F_MobilePhone = entity.F_Tel;
                                u.F_RealName = entity.F_Name;
                                u.F_NickName = entity.F_Name;
                                u.F_HeadIcon = entity.F_FacePic_File;
                                var userLogOnEntity = new SysUserLogin();
                                userLogOnEntity.F_Id = u.F_Id;
                                userLogOnEntity.F_UserId = u.F_Id;
                                userLogOnEntity.F_UserSecretkey =
                                    Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                                var pwd1 = "000000";
                                if (!entity.F_Guarder_CredNum.IsEmpty() && entity.F_Guarder_CredNum.Length >= 6)
                                    pwd1 = entity.F_Guarder_CredNum.Substring(entity.F_Guarder_CredNum.Length - 6, 6);
                                pwd1 = pwd1.ToLower().Replace("x", "0"); //密码x变为0
                                userLogOnEntity.F_UserPassword = Md5EncryptHelper
                                    .Encrypt(
                                        DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(pwd1, 32).ToLower(),
                                            userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                                db.Update(u);
                                db.Update(userLogOnEntity);

                                var isPost = true;
                                var F_Birthday = "";
                                if (u.F_Birthday != null) F_Birthday = u.F_Birthday.ToDateTimeString();
                                var parameters = "sysid=" + u.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                                 u.F_RealName + "&amp;username=" + u.F_Account + "&amp;password=" +
                                                 userLogOnEntity.F_UserPassword + "&amp;sysgroupid=" +
                                                 u.F_DepartmentId + "&amp;headicon=" + u.F_HeadIcon + "&amp;birthday=" +
                                                 F_Birthday + "";
                                WebHelper.SendRequest(CallUrlUpt, parameters, isPost, "application/json");

                                data.F_Users_ID = u.F_Id;

                                var j = db.FindEntity<SysUser>(t => t.F_Account == entity.F_Guarder_Tel);
                                var userLogOnEntity2 = new SysUserLogin();
                                //bool guarder = false;
                                if (j == null)
                                {
                                    //guarder = true;
                                    j = new SysUser();
                                    j.F_Account = entity.F_Guarder_Tel;
                                    j.F_DutyId = "parentDuty";
                                    j.F_Gender = "1".Equals(entity.F_Gender) ? true : false;
                                    j.F_RoleId = "parent";
                                    j.F_MobilePhone = entity.F_Guarder_Tel;
                                    j.F_RealName = entity.F_Guarder;
                                    j.F_OrganizeId = "1";
                                    //分班的时候赋值
                                    j.F_DepartmentId = "parent";
                                    j.F_NickName = entity.F_Guarder;
                                    j.F_IsAdministrator = false;
                                    j.F_EnabledMark = true;
                                    //j.F_HeadIcon = e.F_FacePic_File;
                                    j.Create();

                                    userLogOnEntity2.F_Id = j.F_Id;
                                    userLogOnEntity2.F_UserId = j.F_Id;
                                    userLogOnEntity2.F_UserSecretkey =
                                        Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                                    //生成密码 监护人证件号后6位
                                    var pwd2 = "000000";
                                    if (!entity.F_Guarder_CredNum.IsEmpty() && entity.F_Guarder_CredNum.Length >= 6)
                                        pwd2 = entity.F_Guarder_CredNum.Substring(entity.F_Guarder_CredNum.Length - 6,
                                            6);
                                    pwd2 = pwd2.ToLower().Replace("x", "0"); //密码x变为0
                                    userLogOnEntity2.F_UserPassword = Md5EncryptHelper
                                        .Encrypt(
                                            DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(pwd2, 32),
                                                userLogOnEntity2.F_UserSecretkey), 32).ToLower();
                                    db.Insert(j);
                                    db.Insert(userLogOnEntity2);

                                    var jus = new SysUserRole();
                                    jus.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                                    jus.F_User = j.F_Id;
                                    jus.F_Role = j.F_RoleId;
                                    db.Insert(jus);

                                    if (j.F_Birthday != null)
                                        F_Birthday = j.F_Birthday.ToDateTimeString();
                                    else F_Birthday = "";
                                    parameters = "sysid=" + j.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                                 j.F_RealName + "&amp;username=" + j.F_Account + "&amp;password=" +
                                                 userLogOnEntity2.F_UserPassword + "&amp;sysgroupid=" +
                                                 j.F_DepartmentId + "&amp;headicon=" + j.F_HeadIcon + "&amp;birthday=" +
                                                 F_Birthday + "";
                                    WebHelper.SendRequest(CallUrlAdd, parameters, isPost, "application/json");
                                }
                                else
                                {
                                    //guarder = false;
                                    j.F_Account = entity.F_Guarder_Tel;
                                    j.F_Gender = "1".Equals(entity.F_Gender) ? true : false;
                                    j.F_MobilePhone = entity.F_Guarder_Tel;
                                    j.F_RealName = entity.F_Guarder;
                                    j.F_NickName = entity.F_Guarder;

                                    userLogOnEntity2.F_Id = j.F_Id;
                                    userLogOnEntity2.F_UserId = j.F_Id;
                                    userLogOnEntity2.F_UserSecretkey =
                                        Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                                    //生成密码 监护人证件号后6位
                                    var pwd2 = "000000";
                                    if (!entity.F_Guarder_CredNum.IsEmpty() && entity.F_Guarder_CredNum.Length >= 6)
                                        pwd2 = entity.F_Guarder_CredNum.Substring(entity.F_Guarder_CredNum.Length - 6,
                                            6);
                                    pwd2 = pwd2.ToLower().Replace("x", "0"); //密码x变为0
                                    userLogOnEntity2.F_UserPassword = Md5EncryptHelper
                                        .Encrypt(
                                            DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(pwd2, 32),
                                                userLogOnEntity2.F_UserSecretkey), 32).ToLower();
                                    db.Update(j);
                                    db.Update(userLogOnEntity2);

                                    if (j.F_Birthday != null)
                                        F_Birthday = j.F_Birthday.ToDateTimeString();
                                    else F_Birthday = "";
                                    parameters = "sysid=" + j.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                                 j.F_RealName + "&amp;username=" + j.F_Account + "&amp;password=" +
                                                 userLogOnEntity2.F_UserPassword + "&amp;sysgroupid=" +
                                                 j.F_DepartmentId + "&amp;headicon=" + j.F_HeadIcon + "&amp;birthday=" +
                                                 F_Birthday + "";
                                    WebHelper.SendRequest(CallUrlUpt, parameters, isPost, "application/json");
                                }

                                entity.F_FamilyID = j.F_Id; //家长ID
                                db.Update(entity);

                                //stuentity = ExtObj.clonePropValue(data, stuentity);
                                //stuentity.Modify(stuentity.F_Id);
                                //stuentity.F_InitNum = entity.F_InitNum;
                                //stuentity.F_StudentNum = entity.F_StudentNum;
                                db.Update(data);
                            }
                            catch //(Exception ex)
                            {
                                throw new Exception("学生姓名为'" + data.F_Name + "'更新失败");
                                //list.Add("<p>" + k + ". 学生姓名为'" + data.F_Name + "'更新失败</p> ");
                                //k++;
                                //continue;
                            }

                    //if (list.Any())
                    //{
                    //    for (var i = 0; i < list.Count(); i++) mod += list[i];

                    //    throw new Exception(mod);
                    //}

                    db.Commit();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public void UpdClass(List<Student> datas)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                var list = new List<string>();
                var mod = "<p>导入错误如下：</p>";
                var k = 1;
                foreach (var data in datas)
                    try
                    {
                        var ent = db.FindEntity<Student>(t => t.F_Id.Equals(data.F_Id));
                        var user = db.FindEntity<SysUser>(t => t.F_Id.Equals(ent.F_Users_ID));
                        user.F_DepartmentId = data.F_Class_ID;
                        db.Update(user);
                        db.Update(data);

                        var isPost = true;
                        var parameters = "sysid=" + user.F_Id + "&amp;appid=" + appid + "&amp;sysgroupid=" +
                                         user.F_DepartmentId + "";
                        var Result = WebHelper.SendRequest(CallUrlUpt, parameters, isPost, "application/json");
                    }
                    catch (Exception ex)
                    {
                        list.Add("<p>" + ex + "'更新失败 </p>");
                        k++;
                    }

                if (list.Any())
                {
                    for (var i = 0; i < list.Count(); i++)
                        //if (list.Count() - i == 1)
                        //{
                        mod += list[i];
                    //}
                    //else
                    //    mod += list[i].ToString() + " and ";

                    throw new Exception(mod);
                }

                db.Commit();
            }
        }

        public void UpdCurStatu(string F_id, string F_CurStatu)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                var model = db.Find<Student>(F_id);
                model.F_CurStatu = F_CurStatu;
                var logEntity = new StuStatus();
                logEntity.Create();
                logEntity.F_CurStatu = F_CurStatu;
                logEntity.F_Class_ID = model.F_Class_ID;
                logEntity.F_DepartmentId = model.F_DepartmentId;
                logEntity.F_Divis_ID = model.F_Divis_ID;
                logEntity.F_Grade_ID = model.F_Grade_ID;
                logEntity.F_Name = model.F_Name;
                logEntity.F_StudentId = model.F_Id;
                logEntity.F_StudentNum = model.F_StudentNum;
                db.Insert(logEntity);
                db.Update(model);
                db.Commit();
            }
        }

        public void AddDatas(EntrySignUp entry, SysUser userentity, SysUserLogin userlogentity, SysUser juserentity,
            SysUserLogin juserlogentity, Student entity, bool guarder)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                if (string.IsNullOrEmpty(entity.F_Id))
                {
                    db.Insert(entry);
                    db.Insert(userentity);
                    db.Insert(userlogentity);

                    var F_Birthday = "";
                    if (userentity.F_Birthday != null) F_Birthday = userentity.F_Birthday.ToDateTimeString();
                    var parameters = "sysid=" + userentity.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                     userentity.F_RealName + "&amp;username=" + userentity.F_Account +
                                     "&amp;password=" + userlogentity.F_UserPassword + "&amp;sysgroupid=" +
                                     userentity.F_DepartmentId + "&amp;headicon=" + userentity.F_HeadIcon +
                                     "&amp;birthday=" + F_Birthday + "";
                    WebHelper.SendRequest(CallUrlAdd, parameters, true, "application/json");

                    if (guarder)
                    {
                        db.Insert(juserentity);
                        db.Insert(juserlogentity);

                        if (juserentity.F_Birthday != null)
                            F_Birthday = juserentity.F_Birthday.ToDateTimeString();
                        else F_Birthday = "";
                        parameters = "sysid=" + juserentity.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                     juserentity.F_RealName + "&amp;username=" + juserentity.F_Account +
                                     "&amp;password=" + juserlogentity.F_UserPassword + "&amp;sysgroupid=" +
                                     juserentity.F_DepartmentId + "&amp;headicon=" + juserentity.F_HeadIcon +
                                     "&amp;birthday=" + F_Birthday + "";
                        WebHelper.SendRequest(CallUrlAdd, parameters, true, "application/json");
                    }

                    entity.Create();
                    db.Insert(entity);
                }
                else
                {
                    db.Update(entry);
                    db.Update(userentity);
                    db.Update(userlogentity);

                    var F_Birthday = "";
                    if (userentity.F_Birthday != null) F_Birthday = userentity.F_Birthday.ToDateTimeString();
                    var parameters = "sysid=" + userentity.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                     userentity.F_RealName + "&amp;username=" + userentity.F_Account +
                                     "&amp;password=" + userlogentity.F_UserPassword + "&amp;sysgroupid=" +
                                     userentity.F_DepartmentId + "&amp;headicon=" + userentity.F_HeadIcon +
                                     "&amp;birthday=" + F_Birthday + "";
                    WebHelper.SendRequest(CallUrlUpt, parameters, true, "application/json");

                    if (guarder)
                    {
                        db.Insert(juserentity);
                        db.Insert(juserlogentity);

                        if (juserentity.F_Birthday != null)
                            F_Birthday = juserentity.F_Birthday.ToDateTimeString();
                        else F_Birthday = "";
                        parameters = "sysid=" + juserentity.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                     juserentity.F_RealName + "&amp;username=" + juserentity.F_Account +
                                     "&amp;password=" + juserlogentity.F_UserPassword + "&amp;sysgroupid=" +
                                     juserentity.F_DepartmentId + "&amp;headicon=" + juserentity.F_HeadIcon +
                                     "&amp;birthday=" + F_Birthday + "";
                        WebHelper.SendRequest(CallUrlAdd, parameters, true, "application/json");
                    }
                    else
                    {
                        db.Update(juserentity);
                        db.Update(juserlogentity);

                        if (juserentity.F_Birthday != null)
                            F_Birthday = juserentity.F_Birthday.ToDateTimeString();
                        else F_Birthday = "";
                        parameters = "sysid=" + juserentity.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                     juserentity.F_RealName + "&amp;username=" + juserentity.F_Account +
                                     "&amp;password=" + juserlogentity.F_UserPassword + "&amp;sysgroupid=" +
                                     juserentity.F_DepartmentId + "&amp;headicon=" + juserentity.F_HeadIcon +
                                     "&amp;birthday=" + F_Birthday + "";
                        WebHelper.SendRequest(CallUrlUpt, parameters, true, "application/json");
                    }

                    entity.Modify(entity.F_Id);
                    db.Update(entity);
                }

                db.Commit();
            }
        }

        public void AddDatas(List<StuImport> datas)
        {
            try
            {
                var list = new List<string>();
                var mod = "<p>导入错误如下：</p>";
                var k = 1;
                foreach (var data in datas)
                    if (data.F_Id.IsEmpty())
                        try
                        {
                            using (var db = new Data.UnitWork().BeginTrans())
                            {
                                data.F_Gender = data.F_Gender == "男" ? "1" : "0";
                                var entity = new EntrySignUp();
                                //注册人信息
                                var Oper = OperatorProvider.Current;
                                entity.F_RegUsers_ID = Oper.UserId;
                                entity.F_RegUserType = Oper.Duty;
                                entity.F_RegisterName = Oper.UserName;
                                entity.F_RegisterNum = Oper.UserCode;
                                entity.F_RegisterPhone = Oper.MobilePhone;

                                entity = ExtObj.ClonePropValue(data, entity);
                                entity.F_Year = Convert.ToInt32(data.F_Year);
                                //签字协议状态
                                entity.F_Signed_License_Status = "1";
                                //学费缴纳状态 F_Sundry_Status
                                entity.F_Sundry_Status = "1";
                                //保额金缴纳状态 F_Prepay_Status
                                entity.F_Prepay_Status = "1";
                                //信息完善状态 F_Date_Status
                                entity.F_Date_Status = "1";
                                //报名协议状态
                                entity.F_Statu = "4";
                                //报名序号
                                entity.F_InitNum = "s" + NumberBuilder.Build_18bit();
                                entity.F_DepartmentId = entity.F_Divis_ID;
                                if (string.IsNullOrEmpty(entity.F_StudentNum))
                                {
                                    var org = db.FindEntity<SysOrganize>(t => t.F_Id == entity.F_Divis_ID);
                                    var F_Year = Convert.ToInt32(entity.F_Year);
                                    var seed = db.FindEntity<NoSeed>(t =>
                                        t.F_Divis == entity.F_Divis_ID && t.F_Year == F_Year && t.F_Type == "student");

                                    var no = org.F_EnCode + F_Year;
                                    if (seed == null)
                                    {
                                        seed = new NoSeed();
                                        seed.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                                        seed.F_Divis = entity.F_Divis_ID;
                                        seed.F_Year = F_Year;
                                        seed.F_Type = "student";
                                        seed.F_No = 1;
                                        db.Insert(seed);
                                    }
                                    else
                                    {
                                        seed.F_No = seed.F_No + 1;
                                        db.Update(seed);
                                    }

                                    no = no + getStrNum(Convert.ToString(seed.F_No));
                                    var stuNo = no;
                                    entity.F_StudentNum = stuNo;
                                }
                                else
                                {
                                    var entry = new EntrySignUpRepository().QueryAsNoTracking()
                                        .Where(t => t.F_StudentNum.Equals(entity.F_StudentNum));
                                    //是否被使用
                                    if (entry.Count() <= 0)
                                    {
                                        var org = db.FindEntity<SysOrganize>(t => t.F_Id == entity.F_Divis_ID);
                                        var no = org.F_EnCode + entity.F_Year;
                                        var len = org.F_EnCode.Length + 4;
                                        if (entity.F_StudentNum.Substring(0, len) == no)
                                        {
                                            var seed = db.FindEntity<NoSeed>(t =>
                                                t.F_Divis == entity.F_Divis_ID && t.F_Year == entity.F_Year &&
                                                t.F_Type == "student");
                                            if (seed == null)
                                            {
                                                seed = new NoSeed();
                                                seed.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                                                seed.F_Divis = entity.F_Divis_ID;
                                                seed.F_Year = entity.F_Year;
                                                seed.F_Type = "student";
                                                seed.F_No = 1;
                                                db.Insert(seed);
                                            }
                                            else
                                            {
                                                seed.F_No = Convert.ToInt32(
                                                                entity.F_StudentNum.Substring(
                                                                    entity.F_StudentNum.Length - 4, 4)) + 1;
                                                db.Update(seed);
                                            }

                                            //seed.F_No = Convert.ToInt32(entity.F_StudentNum.Substring(entity.F_StudentNum.Length - 4, 4)) + 1;
                                            //seedservice.Update(seed);
                                        }
                                        else
                                        {
                                            list.Add("<p>" + k + ". 学生姓名为'" + entity.F_Name + "'新增失败（学号规则错误）</p> ");
                                            k++;
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        list.Add("<p>" + k + ". 学生姓名为'" + entity.F_Name + "'新增失败（学号已被使用）</p> ");
                                        k++;
                                        continue;
                                    }
                                }

                                entity.Create();

                                //data.Create();
                                var user = new SysUser
                                {
                                    F_Id = Guid.NewGuid().ToString(),
                                    F_Account = entity.F_StudentNum,
                                    F_DutyId = "studentDuty",
                                    F_Gender = Convert.ToBoolean(Convert.ToInt32(entity.F_Gender)),
                                    F_RoleId = "student",
                                    F_MobilePhone = entity.F_Tel,
                                    F_RealName = entity.F_Name,
                                    F_OrganizeId = "1",
                                    //分班的时候赋值
                                    //u.F_DepartmentId
                                    F_NickName = entity.F_Name,
                                    F_IsAdministrator = false,
                                    F_EnabledMark = true,
                                    F_HeadIcon = entity.F_FacePic_File
                                };
                                var userLogOnEntity = new SysUserLogin();
                                userLogOnEntity.F_Id = user.F_Id;
                                userLogOnEntity.F_UserId = user.F_Id;
                                userLogOnEntity.F_UserSecretkey =
                                    Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                                userLogOnEntity.F_UserPassword = Md5EncryptHelper
                                    .Encrypt(
                                        DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt("123456", 32).ToLower(),
                                            userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                                //userservice.Insert(user);
                                //userlogservice.Insert(userLogOnEntity);
                                data.F_Users_ID = user.F_Id;

                                var us = new SysUserRole();
                                us.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                                us.F_User = user.F_Id;
                                us.F_Role = user.F_RoleId;
                                db.Insert(us);

                                var isPost = true;
                                var F_Birthday = "";
                                if (user.F_Birthday != null) F_Birthday = user.F_Birthday.ToDateTimeString();
                                var parameters = "sysid=" + user.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                                 user.F_RealName + "&amp;username=" + user.F_Account +
                                                 "&amp;password=" + userLogOnEntity.F_UserPassword +
                                                 "&amp;sysgroupid=" + user.F_DepartmentId + "&amp;headicon=" +
                                                 user.F_HeadIcon + "&amp;birthday=" + F_Birthday + "";
                                var Result = WebHelper.SendRequest(CallUrlAdd, parameters, isPost, "application/json");

                                var j = db.FindEntity<SysUser>(t => t.F_Account == entity.F_Guarder_Tel);
                                var userLogOnEntity2 = new SysUserLogin();
                                if (j == null)
                                {
                                    j = new SysUser();
                                    j.F_Account = entity.F_Guarder_Tel;
                                    j.F_DutyId = "parentDuty";
                                    j.F_Gender = "1".Equals(entity.F_Gender) ? true : false;
                                    j.F_RoleId = "parent";
                                    j.F_MobilePhone = entity.F_Guarder_Tel;
                                    j.F_RealName = entity.F_Guarder;
                                    j.F_OrganizeId = "1";
                                    //分班的时候赋值
                                    j.F_DepartmentId = "parent";
                                    j.F_NickName = entity.F_Guarder;
                                    j.F_IsAdministrator = false;
                                    j.F_EnabledMark = true;
                                    //j.F_HeadIcon = e.F_FacePic_File;
                                    j.Create();

                                    userLogOnEntity2.F_Id = j.F_Id;
                                    userLogOnEntity2.F_UserId = j.F_Id;
                                    userLogOnEntity2.F_UserSecretkey =
                                        Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                                    //生成密码 监护人证件号后6位
                                    var pwd2 = "000000";
                                    if (!entity.F_Guarder_CredNum.IsEmpty() && entity.F_Guarder_CredNum.Length >= 6)
                                        pwd2 = entity.F_Guarder_CredNum.Substring(entity.F_Guarder_CredNum.Length - 6,
                                            6);
                                    pwd2 = pwd2.ToLower().Replace("x", "0"); //密码x变为0
                                    userLogOnEntity2.F_UserPassword = Md5EncryptHelper
                                        .Encrypt(
                                            DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(pwd2, 32),
                                                userLogOnEntity2.F_UserSecretkey), 32).ToLower();
                                    db.Insert(j);
                                    db.Insert(userLogOnEntity2);

                                    var jus = new SysUserRole();
                                    jus.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                                    jus.F_User = j.F_Id;
                                    jus.F_Role = j.F_RoleId;
                                    db.Insert(jus);

                                    if (j.F_Birthday != null)
                                        F_Birthday = j.F_Birthday.ToDateTimeString();
                                    else F_Birthday = "";
                                    parameters = "sysid=" + j.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                                 j.F_RealName + "&amp;username=" + j.F_Account + "&amp;password=" +
                                                 userLogOnEntity2.F_UserPassword + "&amp;sysgroupid=" +
                                                 j.F_DepartmentId + "&amp;headicon=" + j.F_HeadIcon + "&amp;birthday=" +
                                                 F_Birthday + "";
                                    Result = WebHelper.SendRequest(CallUrlAdd, parameters, isPost, "application/json");
                                }

                                entity.F_FamilyID = j.F_Id; //家长ID
                                //entservice.Insert(entity);

                                var s = new Student();
                                s = ExtObj.ClonePropValue(data, s);
                                //s.Create();
                                s.F_DepartmentId = entity.F_DepartmentId;
                                s.F_InitNum = entity.F_InitNum;
                                s.F_StudentNum = entity.F_StudentNum;
                                //service.Insert(s);
                                db.Insert(entity);
                                db.Insert(user);
                                db.Insert(userLogOnEntity);

                                s.Create();
                                db.Insert(s);

                                //service.AddDatas(entity, user, userLogOnEntity, j, userLogOnEntity2, s, guarder);
                                db.Commit();
                            }
                        }
                        catch
                        {
                            if (!data.F_Name.IsEmpty())
                            {
                                list.Add("<p>" + k + ". 学生姓名为'" + data.F_Name + "'新增失败</p> ");
                                k++;
                                continue;
                            }
                        }
                    else
                        try
                        {
                            using (var db = new Data.UnitWork().BeginTrans())
                            {
                                data.F_Gender = data.F_Gender == "男" ? "1" : "0";
                                var stuentity = db.Find<Student>(data.F_Id);

                                var expression = ExtLinq.True<EntrySignUp>();
                                if (!string.IsNullOrEmpty(stuentity.F_InitNum))
                                    expression = expression.And(t => t.F_InitNum.Equals(stuentity.F_InitNum));
                                expression = expression.And(t => t.F_Statu != "3");
                                var entity = db.FindEntity(expression);

                                entity = ExtObj.ClonePropValue(data, entity);
                                entity.F_DepartmentId = entity.F_Divis_ID;
                                //stuentity = ExtObj.clonePropValue(data, stuentity);
                                //stuentity.F_Id = data.F_Id;
                                var entry = new EntrySignUpRepository().QueryAsNoTracking()
                                    .Where(t => t.F_StudentNum.Equals(entity.F_StudentNum));
                                if (entry.Count() > 0)
                                {
                                    if (entry.First().F_Id != entity.F_Id)
                                    {
                                        list.Add("<p>" + k + ". 学生姓名为'" + entity.F_Name + "'更新失败（学号已被使用）</p> ");
                                        k++;
                                        continue;
                                    }
                                }
                                else
                                {
                                    var org = db.FindEntity<SysOrganize>(t => t.F_Id == entity.F_Divis_ID);
                                    var no = org.F_EnCode + entity.F_Year;
                                    var len = org.F_EnCode.Length + 4;
                                    if (entity.F_StudentNum.Substring(0, len) == no)
                                    {
                                        var F_Year = Convert.ToInt32(entity.F_Year);
                                        var seed = db.FindEntity<NoSeed>(t =>
                                            t.F_Divis == entity.F_Divis_ID && t.F_Year == F_Year &&
                                            t.F_Type == "student");
                                        if (seed == null)
                                        {
                                            seed = new NoSeed();
                                            seed.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                                            seed.F_Divis = entity.F_Divis_ID;
                                            seed.F_Year = entity.F_Year;
                                            seed.F_Type = "student";
                                            seed.F_No = 1;
                                            db.Insert(seed);
                                        }
                                        else
                                        {
                                            seed.F_No = Convert.ToInt32(
                                                            entity.F_StudentNum.Substring(
                                                                entity.F_StudentNum.Length - 4, 4)) + 1;
                                            db.Update(seed);
                                        }
                                        //seed.F_No = Convert.ToInt32(entity.F_StudentNum.Substring(entity.F_StudentNum.Length - 4, 4)) + 1;
                                        //seedservice.Update(seed);

                                        //getStuNum(Convert.ToInt32(data.F_Year), data.F_Divis_ID);
                                    }
                                    else
                                    {
                                        list.Add("<p>" + k + ". 学生姓名为'" + entity.F_Name + "'更新失败（学号规则错误）</p> ");
                                        k++;
                                        continue;
                                    }
                                }

                                var u = db.Find<SysUser>(stuentity.F_Users_ID);
                                u.F_Account = entity.F_StudentNum;
                                u.F_Gender = Convert.ToBoolean(Convert.ToInt32(entity.F_Gender));
                                u.F_MobilePhone = entity.F_Tel;
                                u.F_RealName = entity.F_Name;
                                u.F_NickName = entity.F_Name;
                                u.F_HeadIcon = entity.F_FacePic_File;
                                var userLogOnEntity = new SysUserLogin();
                                userLogOnEntity.F_Id = u.F_Id;
                                userLogOnEntity.F_UserId = u.F_Id;
                                userLogOnEntity.F_UserSecretkey =
                                    Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                                var pwd1 = "000000";
                                if (!entity.F_Guarder_CredNum.IsEmpty() && entity.F_Guarder_CredNum.Length >= 6)
                                    pwd1 = entity.F_Guarder_CredNum.Substring(entity.F_Guarder_CredNum.Length - 6, 6);
                                pwd1 = pwd1.ToLower().Replace("x", "0"); //密码x变为0
                                userLogOnEntity.F_UserPassword = Md5EncryptHelper
                                    .Encrypt(
                                        DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(pwd1, 32).ToLower(),
                                            userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                                //userservice.Insert(user);
                                //userlogservice.Insert(userLogOnEntity);
                                data.F_Users_ID = u.F_Id;

                                var isPost = true;
                                var F_Birthday = "";
                                if (u.F_Birthday != null) F_Birthday = u.F_Birthday.ToDateTimeString();
                                var parameters = "sysid=" + u.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                                 u.F_RealName + "&amp;username=" + u.F_Account + "&amp;password=" +
                                                 userLogOnEntity.F_UserPassword + "&amp;sysgroupid=" +
                                                 u.F_DepartmentId + "&amp;headicon=" + u.F_HeadIcon + "&amp;birthday=" +
                                                 F_Birthday + "";
                                var Result = WebHelper.SendRequest(CallUrlUpt, parameters, isPost, "application/json");

                                var j = db.FindEntity<SysUser>(t => t.F_Account == entity.F_Guarder_Tel);
                                var userLogOnEntity2 = new SysUserLogin();
                                if (j == null)
                                {
                                    j = new SysUser();
                                    j.F_Account = entity.F_Guarder_Tel;
                                    j.F_DutyId = "parentDuty";
                                    j.F_Gender = "1".Equals(entity.F_Gender) ? true : false;
                                    j.F_RoleId = "parent";
                                    j.F_MobilePhone = entity.F_Guarder_Tel;
                                    j.F_RealName = entity.F_Guarder;
                                    j.F_OrganizeId = "1";
                                    //分班的时候赋值
                                    j.F_DepartmentId = "parent";
                                    j.F_NickName = entity.F_Guarder;
                                    j.F_IsAdministrator = false;
                                    j.F_EnabledMark = true;
                                    //j.F_HeadIcon = e.F_FacePic_File;
                                    j.Create();

                                    userLogOnEntity2.F_Id = j.F_Id;
                                    userLogOnEntity2.F_UserId = j.F_Id;
                                    userLogOnEntity2.F_UserSecretkey =
                                        Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                                    //生成密码 监护人证件号后6位
                                    var pwd2 = "000000";
                                    if (!entity.F_Guarder_CredNum.IsEmpty() && entity.F_Guarder_CredNum.Length >= 6)
                                        pwd2 = entity.F_Guarder_CredNum.Substring(entity.F_Guarder_CredNum.Length - 6,
                                            6);
                                    pwd2 = pwd2.ToLower().Replace("x", "0"); //密码x变为0
                                    userLogOnEntity2.F_UserPassword = Md5EncryptHelper
                                        .Encrypt(
                                            DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(pwd2, 32),
                                                userLogOnEntity2.F_UserSecretkey), 32).ToLower();
                                    db.Insert(j);
                                    db.Insert(userLogOnEntity2);

                                    var jus = new SysUserRole();
                                    jus.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                                    jus.F_User = j.F_Id;
                                    jus.F_Role = j.F_RoleId;
                                    db.Insert(jus);

                                    if (j.F_Birthday != null)
                                        F_Birthday = j.F_Birthday.ToDateTimeString();
                                    else F_Birthday = "";
                                    parameters = "sysid=" + j.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                                 j.F_RealName + "&amp;username=" + j.F_Account + "&amp;password=" +
                                                 userLogOnEntity2.F_UserPassword + "&amp;sysgroupid=" +
                                                 j.F_DepartmentId + "&amp;headicon=" + j.F_HeadIcon + "&amp;birthday=" +
                                                 F_Birthday + "";
                                    Result = WebHelper.SendRequest(CallUrlAdd, parameters, isPost, "application/json");
                                }
                                else
                                {
                                    j.F_Account = entity.F_Guarder_Tel;
                                    j.F_Gender = "1".Equals(entity.F_Gender) ? true : false;
                                    j.F_MobilePhone = entity.F_Guarder_Tel;
                                    j.F_RealName = entity.F_Guarder;
                                    j.F_NickName = entity.F_Guarder;

                                    userLogOnEntity2.F_Id = j.F_Id;
                                    userLogOnEntity2.F_UserId = j.F_Id;
                                    userLogOnEntity2.F_UserSecretkey =
                                        Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                                    //生成密码 监护人证件号后6位
                                    var pwd2 = "000000";
                                    if (!entity.F_Guarder_CredNum.IsEmpty() && entity.F_Guarder_CredNum.Length >= 6)
                                        pwd2 = entity.F_Guarder_CredNum.Substring(entity.F_Guarder_CredNum.Length - 6,
                                            6);
                                    pwd2 = pwd2.ToLower().Replace("x", "0"); //密码x变为0
                                    userLogOnEntity2.F_UserPassword = Md5EncryptHelper
                                        .Encrypt(
                                            DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(pwd2, 32),
                                                userLogOnEntity2.F_UserSecretkey), 32).ToLower();
                                    db.Update(j);
                                    db.Update(userLogOnEntity2);

                                    if (j.F_Birthday != null)
                                        F_Birthday = j.F_Birthday.ToDateTimeString();
                                    else F_Birthday = "";
                                    parameters = "sysid=" + j.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                                 j.F_RealName + "&amp;username=" + j.F_Account + "&amp;password=" +
                                                 userLogOnEntity2.F_UserPassword + "&amp;sysgroupid=" +
                                                 j.F_DepartmentId + "&amp;headicon=" + j.F_HeadIcon + "&amp;birthday=" +
                                                 F_Birthday + "";
                                    Result = WebHelper.SendRequest(CallUrlUpt, parameters, isPost, "application/json");
                                }

                                entity.F_FamilyID = j.F_Id; //家长ID
                                //entservice.Insert(entity);

                                stuentity = ExtObj.ClonePropValue(data, stuentity);
                                //s.Create();
                                stuentity.F_InitNum = entity.F_InitNum;
                                stuentity.F_StudentNum = entity.F_StudentNum;
                                stuentity.F_DepartmentId = entity.F_DepartmentId;
                                //service.Insert(s);
                                db.Update(entity);
                                db.Update(u);
                                db.Update(userLogOnEntity);
                                stuentity.Modify(stuentity.F_Id);
                                db.Update(stuentity);
                                //service.AddDatas(entity, u, userLogOnEntity, j, userLogOnEntity2, stuentity, guarder);
                                db.Commit();
                            }
                        }
                        catch // (Exception ex)
                        {
                            list.Add("<p>" + k + ". 学生姓名为'" + data.F_Name + "'更新失败</p> ");
                            k++;
                            continue;
                        }

                if (list.Count() > 0)
                {
                    for (var i = 0; i < list.Count(); i++)
                        //if (list.Count() - i == 1)
                        //{
                        mod += list[i];
                    //}
                    //else
                    //    mod += list[i].ToString() + " and ";

                    throw new Exception(mod);
                }

                //service.AddDatas(data);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        ///     导入
        /// </summary>
        /// <param name="datas"></param>
        public void AddDatasImport(List<StuImport> datas)
        {
            var list = new List<string>();
            var mod = "<p>导入错误如下：</p>";
            var k = 1;
            foreach (var data in datas)
                if (data.F_Id.IsEmpty())
                    try
                    {
                        using (var db = new Data.UnitWork().BeginTrans())
                        {
                            data.Create();
                            var entity = new EntrySignUp();
                            //注册人信息
                            var Oper = OperatorProvider.Current;
                            entity.F_RegUsers_ID = Oper.UserId;
                            entity.F_RegUserType = Oper.Duty;
                            entity.F_RegisterName = Oper.UserName;
                            entity.F_RegisterNum = Oper.UserCode;
                            entity.F_RegisterPhone = Oper.MobilePhone;

                            entity = ExtObj.ClonePropValue(data, entity);
                            entity.F_Year = Convert.ToInt32(data.F_Year);
                            //签字协议状态
                            entity.F_Signed_License_Status = "1";
                            //学费缴纳状态 F_Sundry_Status
                            entity.F_Sundry_Status = "1";
                            //保额金缴纳状态 F_Prepay_Status
                            entity.F_Prepay_Status = "1";
                            //信息完善状态 F_Date_Status
                            entity.F_Date_Status = "1";
                            //报名协议状态
                            entity.F_Statu = "4";
                            //报名序号
                            entity.F_InitNum = "s" + NumberBuilder.Build_18bit();
                            entity.F_DepartmentId = entity.F_Divis_ID;
                            if (string.IsNullOrEmpty(entity.F_StudentNum))
                            {
                                var stuNo = new NoSeedRepository().getStuNum(Convert.ToInt32(entity.F_Year),
                                    entity.F_Divis_ID);
                                entity.F_StudentNum = stuNo;
                            }
                            else
                            {
                                var entry = new EntrySignUpRepository().QueryAsNoTracking()
                                    .Where(t => t.F_StudentNum.Equals(entity.F_StudentNum));
                                //是否被使用
                                if (entry.Count() <= 0)
                                {
                                    var org = db.FindEntity<SysOrganize>(t => t.F_Id == entity.F_Divis_ID);
                                    var no = org.F_EnCode + entity.F_Year;
                                    var len = org.F_EnCode.Length + 4;
                                    if (entity.F_StudentNum.Substring(0, len) == no)
                                    {
                                        var seed = db.FindEntity<NoSeed>(t =>
                                            t.F_Divis == entity.F_Divis_ID && t.F_Year == entity.F_Year &&
                                            t.F_Type == "student");
                                        if (seed == null)
                                        {
                                            seed = new NoSeed();
                                            seed.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                                            seed.F_Divis = entity.F_Divis_ID;
                                            seed.F_Year = entity.F_Year;
                                            seed.F_Type = "student";
                                            seed.F_No = 1;
                                            db.Insert(seed);
                                        }
                                        else
                                        {
                                            if (Convert.ToInt32(
                                                    entity.F_StudentNum.Substring(entity.F_StudentNum.Length - 4, 4)) +
                                                1 > seed.F_No)
                                            {
                                                seed.F_No = Convert.ToInt32(
                                                                entity.F_StudentNum.Substring(
                                                                    entity.F_StudentNum.Length - 4, 4)) + 1;
                                                db.Update(seed);
                                            }
                                            else
                                            {
                                                seed.F_No = seed.F_No + 1;
                                                db.Update(seed);
                                            }
                                        }

                                        //seed.F_No = Convert.ToInt32(entity.F_StudentNum.Substring(entity.F_StudentNum.Length - 4, 4)) + 1;
                                        //seedservice.Update(seed);
                                    }
                                    else
                                    {
                                        list.Add("<p>" + k + ". 学生姓名为'" + entity.F_Name + "'新增失败（学号规则错误）</p> ");
                                        k++;
                                        continue;
                                    }
                                }
                                else
                                {
                                    list.Add("<p>" + k + ". 学生姓名为'" + entity.F_Name + "'新增失败（学号已被使用）</p> ");
                                    k++;
                                    continue;
                                }
                            }

                            entity.Create();

                            //data.Create();
                            var user = new SysUser
                            {
                                F_Id = Guid.NewGuid().ToString(),
                                F_Account = entity.F_StudentNum,
                                F_DutyId = "studentDuty",
                                F_Gender = Convert.ToBoolean(Convert.ToInt32(entity.F_Gender)),
                                F_RoleId = "student",
                                F_MobilePhone = entity.F_Tel,
                                F_RealName = entity.F_Name,
                                F_OrganizeId = "1",
                                //分班的时候赋值
                                //u.F_DepartmentId
                                F_NickName = entity.F_Name,
                                F_IsAdministrator = false,
                                F_EnabledMark = true,
                                F_HeadIcon = entity.F_FacePic_File
                            };
                            var userLogOnEntity = new SysUserLogin();
                            userLogOnEntity.F_Id = user.F_Id;
                            userLogOnEntity.F_UserId = user.F_Id;
                            userLogOnEntity.F_UserSecretkey = Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                            userLogOnEntity.F_UserPassword = Md5EncryptHelper
                                .Encrypt(
                                    DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt("123456", 32).ToLower(),
                                        userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                            //userservice.Insert(user);
                            //userlogservice.Insert(userLogOnEntity);
                            data.F_Users_ID = user.F_Id;
                            var us = new SysUserRole();
                            us.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                            us.F_User = user.F_Id;
                            us.F_Role = user.F_RoleId;
                            db.Insert(us);
                            var j = db.FindEntity<SysUser>(t => t.F_Account == entity.F_Guarder_Tel);
                            var userLogOnEntity2 = new SysUserLogin();
                            //bool guarder = false;
                            if (j == null)
                            {
                                //guarder = true;
                                j = new SysUser();
                                j.F_Account = entity.F_Guarder_Tel;
                                j.F_DutyId = "parentDuty";
                                j.F_Gender = "1".Equals(entity.F_Gender) ? true : false;
                                j.F_RoleId = "parent";
                                j.F_MobilePhone = entity.F_Guarder_Tel;
                                j.F_RealName = entity.F_Guarder;
                                j.F_OrganizeId = "1";
                                //分班的时候赋值
                                j.F_DepartmentId = "parent";
                                j.F_NickName = entity.F_Guarder;
                                j.F_IsAdministrator = false;
                                j.F_EnabledMark = true;
                                //j.F_HeadIcon = e.F_FacePic_File;
                                j.Create();

                                userLogOnEntity2.F_Id = j.F_Id;
                                userLogOnEntity2.F_UserId = j.F_Id;
                                userLogOnEntity2.F_UserSecretkey =
                                    Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                                //生成密码 监护人证件号后6位
                                var pwd2 = "000000";
                                if (!entity.F_Guarder_CredNum.IsEmpty() && entity.F_Guarder_CredNum.Length >= 6)
                                    pwd2 = entity.F_Guarder_CredNum.Substring(entity.F_Guarder_CredNum.Length - 6, 6);
                                pwd2 = pwd2.ToLower().Replace("x", "0"); //密码x变为0
                                userLogOnEntity2.F_UserPassword = Md5EncryptHelper
                                    .Encrypt(
                                        DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(pwd2, 32),
                                            userLogOnEntity2.F_UserSecretkey), 32).ToLower();
                                db.Insert(j);
                                db.Insert(userLogOnEntity2);

                                var jus = new SysUserRole();
                                jus.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                                jus.F_User = j.F_Id;
                                jus.F_Role = j.F_RoleId;
                                db.Insert(jus);
                            }

                            entity.F_FamilyID = j.F_Id; //家长ID
                            //entservice.Insert(entity);

                            var s = new Student();
                            s = ExtObj.ClonePropValue(data, s);
                            //s.Create();
                            s.F_InitNum = entity.F_InitNum;
                            s.F_StudentNum = entity.F_StudentNum;
                            //service.Insert(s);
                            db.Insert(entity);
                            db.Insert(user);
                            db.Insert(userLogOnEntity);

                            s.Create();
                            db.Insert(s);
                            //service.AddDatas(entity, user, userLogOnEntity, j, userLogOnEntity2, s, guarder);
                            db.Commit();
                        }
                    }
                    catch
                    {
                        list.Add("<p>" + k + ". 学生姓名为'" + data.F_Name + "'新增失败</p> ");
                        k++;
                        continue;
                    }
                else
                    try
                    {
                        using (var db = new Data.UnitWork().BeginTrans())
                        {
                            var stuentity = db.Find<Student>(data.F_Id);

                            var expression = ExtLinq.True<EntrySignUp>();
                            if (!string.IsNullOrEmpty(stuentity.F_InitNum))
                                expression = expression.And(t => t.F_InitNum.Equals(stuentity.F_InitNum));
                            expression = expression.And(t => t.F_Statu != "3");
                            var entity = db.FindEntity(expression);
                            //School_EntrySignUp_Entity entity = new School_EntrySignUp_App().GetFormByF_InitNum(stuentity.F_InitNum);

                            entity = ExtObj.ClonePropValue(data, entity);

                            //stuentity = ExtObj.clonePropValue(data, stuentity);
                            //stuentity.F_Id = data.F_Id;
                            var entry = new EntrySignUpRepository().QueryAsNoTracking()
                                .Where(t => t.F_StudentNum.Equals(entity.F_StudentNum));
                            if (entry.Count() > 0)
                            {
                                if (entry.First().F_Id != entity.F_Id)
                                {
                                    list.Add("<p>" + k + ". 学生姓名为'" + entity.F_Name + "'更新失败（学号已被使用）</p> ");
                                    k++;
                                    continue;
                                }
                            }
                            else
                            {
                                var org = db.FindEntity<SysOrganize>(t => t.F_Id == entity.F_Divis_ID);
                                var no = org.F_EnCode + entity.F_Year;
                                var len = org.F_EnCode.Length + 4;
                                if (entity.F_StudentNum.Substring(0, len) == no)
                                {
                                    var F_Year = Convert.ToInt32(entity.F_Year);
                                    var seed = db.FindEntity<NoSeed>(t =>
                                        t.F_Divis == entity.F_Divis_ID && t.F_Year == F_Year && t.F_Type == "student");
                                    if (seed == null)
                                    {
                                        seed = new NoSeed();
                                        seed.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                                        seed.F_Divis = entity.F_Divis_ID;
                                        seed.F_Year = entity.F_Year;
                                        seed.F_Type = "student";
                                        seed.F_No = 1;
                                        db.Insert(seed);
                                    }
                                    else
                                    {
                                        if (Convert.ToInt32(
                                                entity.F_StudentNum.Substring(entity.F_StudentNum.Length - 4, 4)) + 1 >
                                            seed.F_No)
                                        {
                                            seed.F_No = Convert.ToInt32(
                                                            entity.F_StudentNum.Substring(
                                                                entity.F_StudentNum.Length - 4, 4)) + 1;
                                            db.Update(seed);
                                        }
                                        else
                                        {
                                            seed.F_No = seed.F_No + 1;
                                            db.Update(seed);
                                        }
                                    }
                                    //seed.F_No = Convert.ToInt32(entity.F_StudentNum.Substring(entity.F_StudentNum.Length - 4, 4)) + 1;
                                    //seedservice.Update(seed);

                                    //getStuNum(Convert.ToInt32(data.F_Year), data.F_Divis_ID);
                                }
                                else
                                {
                                    list.Add("<p>" + k + ". 学生姓名为'" + entity.F_Name + "'更新失败（学号规则错误）</p> ");
                                    k++;
                                    continue;
                                }
                            }

                            var u = db.Find<SysUser>(stuentity.F_Users_ID);
                            u.F_Account = entity.F_StudentNum;
                            u.F_Gender = Convert.ToBoolean(Convert.ToInt32(entity.F_Gender));
                            u.F_MobilePhone = entity.F_Tel;
                            u.F_RealName = entity.F_Name;
                            u.F_NickName = entity.F_Name;
                            u.F_HeadIcon = entity.F_FacePic_File;
                            var userLogOnEntity = new SysUserLogin();
                            userLogOnEntity.F_Id = u.F_Id;
                            userLogOnEntity.F_UserId = u.F_Id;
                            userLogOnEntity.F_UserSecretkey = Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                            var pwd1 = "000000";
                            if (!entity.F_Guarder_CredNum.IsEmpty() && entity.F_Guarder_CredNum.Length >= 6)
                                pwd1 = entity.F_Guarder_CredNum.Substring(entity.F_Guarder_CredNum.Length - 6, 6);
                            pwd1 = pwd1.ToLower().Replace("x", "0"); //密码x变为0
                            userLogOnEntity.F_UserPassword = Md5EncryptHelper
                                .Encrypt(
                                    DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(pwd1, 32).ToLower(),
                                        userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                            //userservice.Insert(user);
                            //userlogservice.Insert(userLogOnEntity);
                            data.F_Users_ID = u.F_Id;

                            var j = db.FindEntity<SysUser>(t => t.F_Account == entity.F_Guarder_Tel);
                            var userLogOnEntity2 = new SysUserLogin();
                            //bool guarder = false;
                            if (j == null)
                            {
                                //guarder = true;
                                j = new SysUser();
                                j.F_Account = entity.F_Guarder_Tel;
                                j.F_DutyId = "parentDuty";
                                j.F_Gender = "1".Equals(entity.F_Gender) ? true : false;
                                j.F_RoleId = "parent";
                                j.F_MobilePhone = entity.F_Guarder_Tel;
                                j.F_RealName = entity.F_Guarder;
                                j.F_OrganizeId = "1";
                                //分班的时候赋值
                                j.F_DepartmentId = "parent";
                                j.F_NickName = entity.F_Guarder;
                                j.F_IsAdministrator = false;
                                j.F_EnabledMark = true;
                                //j.F_HeadIcon = e.F_FacePic_File;
                                j.Create();

                                userLogOnEntity2.F_Id = j.F_Id;
                                userLogOnEntity2.F_UserId = j.F_Id;
                                userLogOnEntity2.F_UserSecretkey =
                                    Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                                //生成密码 监护人证件号后6位
                                var pwd2 = "000000";
                                if (!entity.F_Guarder_CredNum.IsEmpty() && entity.F_Guarder_CredNum.Length >= 6)
                                    pwd2 = entity.F_Guarder_CredNum.Substring(entity.F_Guarder_CredNum.Length - 6, 6);
                                pwd2 = pwd2.ToLower().Replace("x", "0"); //密码x变为0
                                userLogOnEntity2.F_UserPassword = Md5EncryptHelper
                                    .Encrypt(
                                        DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(pwd2, 32),
                                            userLogOnEntity2.F_UserSecretkey), 32).ToLower();
                                db.Insert(j);
                                db.Insert(userLogOnEntity2);

                                var jus = new SysUserRole();
                                jus.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                                jus.F_User = j.F_Id;
                                jus.F_Role = j.F_RoleId;
                                db.Insert(jus);
                            }
                            else
                            {
                                //guarder = false;
                                j.F_Account = entity.F_Guarder_Tel;
                                j.F_Gender = "1".Equals(entity.F_Gender) ? true : false;
                                j.F_MobilePhone = entity.F_Guarder_Tel;
                                j.F_RealName = entity.F_Guarder;
                                j.F_NickName = entity.F_Guarder;

                                userLogOnEntity2.F_Id = j.F_Id;
                                userLogOnEntity2.F_UserId = j.F_Id;
                                userLogOnEntity2.F_UserSecretkey =
                                    Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                                //生成密码 监护人证件号后6位
                                var pwd2 = "000000";
                                if (!entity.F_Guarder_CredNum.IsEmpty() && entity.F_Guarder_CredNum.Length >= 6)
                                    pwd2 = entity.F_Guarder_CredNum.Substring(entity.F_Guarder_CredNum.Length - 6, 6);
                                pwd2 = pwd2.ToLower().Replace("x", "0"); //密码x变为0
                                userLogOnEntity2.F_UserPassword = Md5EncryptHelper
                                    .Encrypt(
                                        DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(pwd2, 32),
                                            userLogOnEntity2.F_UserSecretkey), 32).ToLower();
                                db.Update(j);
                                db.Update(userLogOnEntity2);
                            }

                            entity.F_FamilyID = j.F_Id; //家长ID
                            //entservice.Insert(entity);

                            stuentity = ExtObj.ClonePropValue(data, stuentity);
                            //s.Create();
                            stuentity.F_InitNum = entity.F_InitNum;
                            stuentity.F_StudentNum = entity.F_StudentNum;
                            //service.Insert(s);
                            db.Update(entity);
                            db.Update(u);
                            db.Update(userLogOnEntity);
                            stuentity.Modify(stuentity.F_Id);
                            db.Update(stuentity);
                            //service.AddDatas(entity, u, userLogOnEntity, j, userLogOnEntity2, stuentity, guarder);
                            db.Commit();
                        }
                    }
                    catch
                    {
                        list.Add("<p>" + k + ". 学生姓名为'" + data.F_Name + "'更新失败</p> ");
                        k++;
                        continue;
                    }

            if (list.Count() > 0)
            {
                for (var i = 0; i < list.Count(); i++)
                    //if (list.Count() - i == 1)
                    //{
                    mod += list[i];
                //}
                //else
                //    mod += list[i].ToString() + " and ";

                throw new Exception(mod);
            }
        }

        /// <summary>
        ///     导入改
        /// </summary>
        /// <param name="datas"></param>
        public void AddDatasImportTwo(List<StuImportTwo> datas)
        {
            var list = new List<string>();
            var mod = "<p>导入错误如下：</p>";
            var k = 1;
            foreach (var data in datas)
                if (data.F_Id.IsEmpty())
                    try
                    {
                        using (var db = new Data.UnitWork().BeginTrans())
                        {
                            //List<School_Teachers_Entity> tea = db.IQueryable<School_Teachers_Entity>(t => t.F_Name.Equals(data.F_RegisterName)).ToList();
                            //if (tea.Count > 1)
                            //{
                            //    list.Add("<p>" + k + ". 学生姓名为'" + data.F_Name + "'招生老师'" + data.F_RegisterName + "'不明确</p> ");
                            //    k++;
                            //    continue;
                            //}
                            //if (tea.Count <= 0)
                            //{
                            //    list.Add("<p>" + k + ". 学生姓名为'" + data.F_Name + "'招生老师'" + data.F_RegisterName + "'不存在</p> ");
                            //    k++;
                            //    continue;
                            //}
                            data.Create();

                            var entity = new EntrySignUp();

                            if (data.F_ComeFrom_Type.Contains("直升")) data.F_ComeFrom_Type = "校内直升";
                            else data.F_ComeFrom_Type = "校外转入";

                            if (data.F_CredNum.Equals("000000000000000000"))
                                data.F_CredNum = data.F_StudentNum.Substring(4);

                            data.F_Year = data.F_StudentNum.Substring(2, 4);

                            if (data.F_ComeBack_Fees == null || data.F_ComeBack_Fees.ToString() == "0")
                            {
                                data.F_ComeBackArea = "";
                            }
                            else
                            {
                                //var area = db.FindEntity<SchArea>(t => t.F_Id.Equals(data.F_ComeBackArea));
                                //if (area != null)
                                //    data.F_ComeBackCity =
                                //        db.FindEntity<SchArea>(t => t.F_Id.Equals(data.F_ComeBackArea)).F_ParentId;
                            }

                            if (!string.IsNullOrEmpty(data.F_ComeFrom_City)) data.F_ComeFrom_Province = "330000";

                            switch (data.F_INYear)
                            {
                                case "小一":
                                    data.F_INYear = "一年级";
                                    break;

                                case "小二":
                                    data.F_INYear = "二年级";
                                    break;

                                case "小三":
                                    data.F_INYear = "三年级";
                                    break;

                                case "小四":
                                    data.F_INYear = "四年级";
                                    break;

                                case "小五":
                                    data.F_INYear = "五年级";
                                    break;

                                case "小六":
                                    data.F_INYear = "六年级";
                                    break;
                            }

                            switch (data.F_Divis_ID)
                            {
                                case "01":

                                    #region F_Divis_ID 精品小学

                                    switch (data.F_Grade_ID)
                                    {
                                        case "2013级":

                                            #region F_Class_ID 2013级

                                            switch (data.F_Class_ID)
                                            {
                                                case "601":
                                                    data.F_Class_ID = "010101";
                                                    break;

                                                case "602":
                                                    data.F_Class_ID = "010102";
                                                    break;

                                                case "603":
                                                    data.F_Class_ID = "010103";
                                                    break;

                                                case "604":
                                                    data.F_Class_ID = "010104";
                                                    break;

                                                case "605":
                                                    data.F_Class_ID = "010105";
                                                    break;

                                                case "606":
                                                    data.F_Class_ID = "010106";
                                                    break;

                                                case "607":
                                                    data.F_Class_ID = "010107";
                                                    break;

                                                case "608":
                                                    data.F_Class_ID = "010108";
                                                    break;

                                                case "609":
                                                    data.F_Class_ID = "010109";
                                                    break;

                                                case "610":
                                                    data.F_Class_ID = "010110";
                                                    break;

                                                case "611":
                                                    data.F_Class_ID = "010111";
                                                    break;

                                                case "612":
                                                    data.F_Class_ID = "010112";
                                                    break;
                                            }

                                            #endregion F_Class_ID 2013级

                                            data.F_Grade_ID = "0101";
                                            break;

                                        case "2014级":

                                            #region F_Class_ID 2014级

                                            switch (data.F_Class_ID)
                                            {
                                                case "501":
                                                    data.F_Class_ID = "010201";
                                                    break;

                                                case "502":
                                                    data.F_Class_ID = "010202";
                                                    break;

                                                case "503":
                                                    data.F_Class_ID = "010203";
                                                    break;

                                                case "504":
                                                    data.F_Class_ID = "010204";
                                                    break;

                                                case "505":
                                                    data.F_Class_ID = "010205";
                                                    break;

                                                case "506":
                                                    data.F_Class_ID = "010206";
                                                    break;

                                                case "507":
                                                    data.F_Class_ID = "010207";
                                                    break;

                                                case "508":
                                                    data.F_Class_ID = "010208";
                                                    break;

                                                case "509":
                                                    data.F_Class_ID = "010209";
                                                    break;

                                                case "510":
                                                    data.F_Class_ID = "010210";
                                                    break;

                                                case "511":
                                                    data.F_Class_ID = "010211";
                                                    break;
                                            }

                                            #endregion F_Class_ID 2014级

                                            data.F_Grade_ID = "0102";
                                            break;

                                        case "2015级":

                                            #region F_Grade_ID 2015级

                                            switch (data.F_Class_ID)
                                            {
                                                case "401":
                                                    data.F_Class_ID = "010301";
                                                    break;

                                                case "402":
                                                    data.F_Class_ID = "010302";
                                                    break;

                                                case "403":
                                                    data.F_Class_ID = "010303";
                                                    break;

                                                case "404":
                                                    data.F_Class_ID = "010304";
                                                    break;

                                                case "405":
                                                    data.F_Class_ID = "010305";
                                                    break;

                                                case "406":
                                                    data.F_Class_ID = "010306";
                                                    break;

                                                case "407":
                                                    data.F_Class_ID = "010307";
                                                    break;

                                                case "408":
                                                    data.F_Class_ID = "010308";
                                                    break;

                                                case "409":
                                                    data.F_Class_ID = "010309";
                                                    break;
                                            }

                                            #endregion F_Grade_ID 2015级

                                            data.F_Grade_ID = "0103";
                                            break;

                                        case "2016级":

                                            #region F_Grade_ID 2016级

                                            switch (data.F_Class_ID)
                                            {
                                                case "301":
                                                    data.F_Class_ID = "010401";
                                                    break;

                                                case "302":
                                                    data.F_Class_ID = "010402";
                                                    break;

                                                case "303":
                                                    data.F_Class_ID = "010403";
                                                    break;

                                                case "304":
                                                    data.F_Class_ID = "010404";
                                                    break;

                                                case "305":
                                                    data.F_Class_ID = "010405";
                                                    break;

                                                case "306":
                                                    data.F_Class_ID = "010406";
                                                    break;

                                                case "307":
                                                    data.F_Class_ID = "010407";
                                                    break;

                                                case "308":
                                                    data.F_Class_ID = "010408";
                                                    break;
                                            }

                                            #endregion F_Grade_ID 2016级

                                            data.F_Grade_ID = "0104";
                                            break;

                                        case "2017级":

                                            #region F_Grade_ID 2017级

                                            switch (data.F_Class_ID)
                                            {
                                                case "201":
                                                    data.F_Class_ID = "010501";
                                                    break;

                                                case "202":
                                                    data.F_Class_ID = "010502";
                                                    break;

                                                case "203":
                                                    data.F_Class_ID = "010503";
                                                    break;

                                                case "204":
                                                    data.F_Class_ID = "010504";
                                                    break;

                                                case "205":
                                                    data.F_Class_ID = "010505";
                                                    break;

                                                case "206":
                                                    data.F_Class_ID = "010506";
                                                    break;

                                                case "207":
                                                    data.F_Class_ID = "010507";
                                                    break;

                                                case "208":
                                                    data.F_Class_ID = "010508";
                                                    break;
                                            }

                                            #endregion F_Grade_ID 2017级

                                            data.F_Grade_ID = "0105";
                                            break;

                                        case "2018级":

                                            #region F_Grade_ID 2018级

                                            switch (data.F_Class_ID)
                                            {
                                                case "101":
                                                    data.F_Class_ID = "010601";
                                                    break;

                                                case "102":
                                                    data.F_Class_ID = "010602";
                                                    break;

                                                case "103":
                                                    data.F_Class_ID = "010603";
                                                    break;

                                                case "104":
                                                    data.F_Class_ID = "010604";
                                                    break;

                                                case "105":
                                                    data.F_Class_ID = "010605";
                                                    break;

                                                case "106":
                                                    data.F_Class_ID = "010606";
                                                    break;

                                                case "107":
                                                    data.F_Class_ID = "010607";
                                                    break;

                                                case "108":
                                                    data.F_Class_ID = "010608";
                                                    break;

                                                case "109":
                                                    data.F_Class_ID = "010609";
                                                    break;
                                            }

                                            #endregion F_Grade_ID 2018级

                                            data.F_Grade_ID = "0106";
                                            break;
                                    }

                                    #endregion F_Divis_ID 精品小学

                                    break;

                                case "02":

                                    #region F_Divis_ID 精品初中

                                    switch (data.F_Grade_ID)
                                    {
                                        case "2016级":

                                            #region F_Grade_ID 2016级

                                            switch (data.F_Class_ID)
                                            {
                                                case "901":
                                                    data.F_Class_ID = "020101";
                                                    break;

                                                case "902":
                                                    data.F_Class_ID = "020102";
                                                    break;

                                                case "903":
                                                    data.F_Class_ID = "020103";
                                                    break;

                                                case "904":
                                                    data.F_Class_ID = "020104";
                                                    break;

                                                case "905":
                                                    data.F_Class_ID = "020105";
                                                    break;

                                                case "906":
                                                    data.F_Class_ID = "020106";
                                                    break;

                                                case "907":
                                                    data.F_Class_ID = "020107";
                                                    break;

                                                case "908":
                                                    data.F_Class_ID = "020108";
                                                    break;

                                                case "909":
                                                    data.F_Class_ID = "020109";
                                                    break;

                                                case "910":
                                                    data.F_Class_ID = "020110";
                                                    break;

                                                case "911":
                                                    data.F_Class_ID = "020111";
                                                    break;

                                                case "912":
                                                    data.F_Class_ID = "020112";
                                                    break;

                                                case "913":
                                                    data.F_Class_ID = "020113";
                                                    break;

                                                case "914":
                                                    data.F_Class_ID = "020114";
                                                    break;

                                                case "915":
                                                    data.F_Class_ID = "020115";
                                                    break;

                                                case "916":
                                                    data.F_Class_ID = "020116";
                                                    break;

                                                case "917":
                                                    data.F_Class_ID = "020117";
                                                    break;
                                            }

                                            #endregion F_Grade_ID 2016级

                                            data.F_Grade_ID = "0201";
                                            break;

                                        case "2017级":

                                            #region F_Grade_ID 2017级

                                            switch (data.F_Class_ID)
                                            {
                                                case "801":
                                                    data.F_Class_ID = "020201";
                                                    break;

                                                case "802":
                                                    data.F_Class_ID = "020202";
                                                    break;

                                                case "803":
                                                    data.F_Class_ID = "020203";
                                                    break;

                                                case "804":
                                                    data.F_Class_ID = "020204";
                                                    break;

                                                case "805":
                                                    data.F_Class_ID = "020205";
                                                    break;

                                                case "806":
                                                    data.F_Class_ID = "020206";
                                                    break;

                                                case "807":
                                                    data.F_Class_ID = "020207";
                                                    break;

                                                case "808":
                                                    data.F_Class_ID = "020208";
                                                    break;

                                                case "809":
                                                    data.F_Class_ID = "020209";
                                                    break;

                                                case "810":
                                                    data.F_Class_ID = "020210";
                                                    break;

                                                case "811":
                                                    data.F_Class_ID = "020211";
                                                    break;

                                                case "812":
                                                    data.F_Class_ID = "020212";
                                                    break;

                                                case "813":
                                                    data.F_Class_ID = "020213";
                                                    break;

                                                case "814":
                                                    data.F_Class_ID = "020214";
                                                    break;

                                                case "815":
                                                    data.F_Class_ID = "020215";
                                                    break;

                                                case "816":
                                                    data.F_Class_ID = "020216";
                                                    break;

                                                case "817":
                                                    data.F_Class_ID = "020217";
                                                    break;

                                                case "818":
                                                    data.F_Class_ID = "020218";
                                                    break;

                                                case "819":
                                                    data.F_Class_ID = "020219";
                                                    break;

                                                case "820":
                                                    data.F_Class_ID = "020220";
                                                    break;
                                            }

                                            #endregion F_Grade_ID 2017级

                                            data.F_Grade_ID = "0202";
                                            break;

                                        case "2018级":

                                            #region F_Grade_ID 2018级

                                            switch (data.F_Class_ID)
                                            {
                                                case "701":
                                                    data.F_Class_ID = "020301";
                                                    break;

                                                case "702":
                                                    data.F_Class_ID = "020302";
                                                    break;

                                                case "703":
                                                    data.F_Class_ID = "020303";
                                                    break;

                                                case "704":
                                                    data.F_Class_ID = "020304";
                                                    break;

                                                case "705":
                                                    data.F_Class_ID = "020305";
                                                    break;

                                                case "706":
                                                    data.F_Class_ID = "020306";
                                                    break;

                                                case "707":
                                                    data.F_Class_ID = "020307";
                                                    break;

                                                case "708":
                                                    data.F_Class_ID = "020308";
                                                    break;

                                                case "709":
                                                    data.F_Class_ID = "020309";
                                                    break;

                                                case "710":
                                                    data.F_Class_ID = "020310";
                                                    break;

                                                case "711":
                                                    data.F_Class_ID = "020311";
                                                    break;

                                                case "712":
                                                    data.F_Class_ID = "020312";
                                                    break;

                                                case "713":
                                                    data.F_Class_ID = "020313";
                                                    break;

                                                case "714":
                                                    data.F_Class_ID = "020314";
                                                    break;

                                                case "715":
                                                    data.F_Class_ID = "020315";
                                                    break;

                                                case "716":
                                                    data.F_Class_ID = "020316";
                                                    break;

                                                case "717":
                                                    data.F_Class_ID = "020317";
                                                    break;

                                                case "718":
                                                    data.F_Class_ID = "020318";
                                                    break;

                                                case "719":
                                                    data.F_Class_ID = "020319";
                                                    break;

                                                case "720":
                                                    data.F_Class_ID = "020320";
                                                    break;
                                            }

                                            #endregion F_Grade_ID 2018级

                                            data.F_Grade_ID = "0203";
                                            break;
                                    }

                                    #endregion F_Divis_ID 精品初中

                                    break;

                                case "03":

                                    #region F_Divis_ID 精品高中

                                    switch (data.F_Grade_ID)
                                    {
                                        case "2016级":

                                            #region F_Grade_ID 2016级

                                            switch (data.F_Class_ID)
                                            {
                                                case "301":
                                                    data.F_Class_ID = "030101";
                                                    break;

                                                case "302":
                                                    data.F_Class_ID = "030102";
                                                    break;

                                                case "303":
                                                    data.F_Class_ID = "030103";
                                                    break;

                                                case "304":
                                                    data.F_Class_ID = "030104";
                                                    break;

                                                case "305":
                                                    data.F_Class_ID = "030105";
                                                    break;

                                                case "306":
                                                    data.F_Class_ID = "030106";
                                                    break;

                                                case "307":
                                                    data.F_Class_ID = "030107";
                                                    break;

                                                case "308":
                                                    data.F_Class_ID = "030108";
                                                    break;

                                                case "309":
                                                    data.F_Class_ID = "030109";
                                                    break;

                                                case "310":
                                                    data.F_Class_ID = "030110";
                                                    break;

                                                case "311":
                                                    data.F_Class_ID = "030111";
                                                    break;

                                                case "312":
                                                    data.F_Class_ID = "030112";
                                                    break;

                                                case "313":
                                                    data.F_Class_ID = "030113";
                                                    break;

                                                case "314":
                                                    data.F_Class_ID = "030114";
                                                    break;

                                                case "315":
                                                    data.F_Class_ID = "030115";
                                                    break;

                                                case "316":
                                                    data.F_Class_ID = "030116";
                                                    break;

                                                case "317":
                                                    data.F_Class_ID = "030117";
                                                    break;

                                                case "318":
                                                    data.F_Class_ID = "030118";
                                                    break;

                                                case "319":
                                                    data.F_Class_ID = "030119";
                                                    break;

                                                case "320":
                                                    data.F_Class_ID = "030120";
                                                    break;

                                                case "321":
                                                    data.F_Class_ID = "030121";
                                                    break;

                                                case "322":
                                                    data.F_Class_ID = "030122";
                                                    break;

                                                case "323":
                                                    data.F_Class_ID = "030123";
                                                    break;

                                                case "324":
                                                    data.F_Class_ID = "030124";
                                                    break;

                                                case "325":
                                                    data.F_Class_ID = "030125";
                                                    break;

                                                case "326":
                                                    data.F_Class_ID = "030126";
                                                    break;

                                                case "327":
                                                    data.F_Class_ID = "030127";
                                                    break;

                                                case "328":
                                                    data.F_Class_ID = "030128";
                                                    break;

                                                case "329":
                                                    data.F_Class_ID = "030129";
                                                    break;

                                                case "330":
                                                    data.F_Class_ID = "030130";
                                                    break;

                                                case "331":
                                                    data.F_Class_ID = "030131";
                                                    break;

                                                case "332":
                                                    data.F_Class_ID = "030132";
                                                    break;

                                                case "333":
                                                    data.F_Class_ID = "030133";
                                                    break;

                                                case "334":
                                                    data.F_Class_ID = "030134";
                                                    break;

                                                case "335":
                                                    data.F_Class_ID = "030135";
                                                    break;
                                            }

                                            #endregion F_Grade_ID 2016级

                                            data.F_Grade_ID = "0301";
                                            break;

                                        case "2017级":

                                            #region F_Grade_ID 2017级

                                            switch (data.F_Class_ID)
                                            {
                                                case "201":
                                                    data.F_Class_ID = "030201";
                                                    break;

                                                case "202":
                                                    data.F_Class_ID = "030202";
                                                    break;

                                                case "203":
                                                    data.F_Class_ID = "030203";
                                                    break;

                                                case "204":
                                                    data.F_Class_ID = "030204";
                                                    break;

                                                case "205":
                                                    data.F_Class_ID = "030205";
                                                    break;

                                                case "206":
                                                    data.F_Class_ID = "030206";
                                                    break;

                                                case "207":
                                                    data.F_Class_ID = "030207";
                                                    break;

                                                case "208":
                                                    data.F_Class_ID = "030208";
                                                    break;

                                                case "209":
                                                    data.F_Class_ID = "030209";
                                                    break;

                                                case "210":
                                                    data.F_Class_ID = "030210";
                                                    break;

                                                case "211":
                                                    data.F_Class_ID = "030211";
                                                    break;

                                                case "212":
                                                    data.F_Class_ID = "030212";
                                                    break;

                                                case "213":
                                                    data.F_Class_ID = "030213";
                                                    break;

                                                case "214":
                                                    data.F_Class_ID = "030214";
                                                    break;

                                                case "215":
                                                    data.F_Class_ID = "030215";
                                                    break;

                                                case "216":
                                                    data.F_Class_ID = "030216";
                                                    break;

                                                case "217":
                                                    data.F_Class_ID = "030217";
                                                    break;

                                                case "218":
                                                    data.F_Class_ID = "030218";
                                                    break;

                                                case "219":
                                                    data.F_Class_ID = "030219";
                                                    break;

                                                case "220":
                                                    data.F_Class_ID = "030220";
                                                    break;

                                                case "221":
                                                    data.F_Class_ID = "030221";
                                                    break;

                                                case "222":
                                                    data.F_Class_ID = "030222";
                                                    break;

                                                case "223":
                                                    data.F_Class_ID = "030223";
                                                    break;

                                                case "224":
                                                    data.F_Class_ID = "030224";
                                                    break;

                                                case "225":
                                                    data.F_Class_ID = "030225";
                                                    break;

                                                case "226":
                                                    data.F_Class_ID = "030226";
                                                    break;

                                                case "227":
                                                    data.F_Class_ID = "030227";
                                                    break;

                                                case "228":
                                                    data.F_Class_ID = "030228";
                                                    break;

                                                case "229":
                                                    data.F_Class_ID = "030229";
                                                    break;

                                                case "230":
                                                    data.F_Class_ID = "030230";
                                                    break;

                                                case "231":
                                                    data.F_Class_ID = "030231";
                                                    break;

                                                case "232":
                                                    data.F_Class_ID = "030232";
                                                    break;

                                                case "233":
                                                    data.F_Class_ID = "030233";
                                                    break;

                                                case "234":
                                                    data.F_Class_ID = "030234";
                                                    break;

                                                case "235":
                                                    data.F_Class_ID = "030235";
                                                    break;

                                                case "236":
                                                    data.F_Class_ID = "030236";
                                                    break;

                                                case "237":
                                                    data.F_Class_ID = "030237";
                                                    break;

                                                case "238":
                                                    data.F_Class_ID = "030238";
                                                    break;

                                                case "239":
                                                    data.F_Class_ID = "030239";
                                                    break;
                                            }

                                            #endregion F_Grade_ID 2017级

                                            data.F_Grade_ID = "0302";
                                            break;

                                        case "2018级":

                                            #region F_Grade_ID 2018级

                                            switch (data.F_Class_ID)
                                            {
                                                case "101":
                                                    data.F_Class_ID = "030301";
                                                    break;

                                                case "102":
                                                    data.F_Class_ID = "030302";
                                                    break;

                                                case "103":
                                                    data.F_Class_ID = "030303";
                                                    break;

                                                case "104":
                                                    data.F_Class_ID = "030304";
                                                    break;

                                                case "105":
                                                    data.F_Class_ID = "030305";
                                                    break;

                                                case "106":
                                                    data.F_Class_ID = "030306";
                                                    break;

                                                case "107":
                                                    data.F_Class_ID = "030307";
                                                    break;

                                                case "108":
                                                    data.F_Class_ID = "030308";
                                                    break;

                                                case "109":
                                                    data.F_Class_ID = "030309";
                                                    break;

                                                case "110":
                                                    data.F_Class_ID = "030310";
                                                    break;

                                                case "111":
                                                    data.F_Class_ID = "030311";
                                                    break;

                                                case "112":
                                                    data.F_Class_ID = "030312";
                                                    break;

                                                case "113":
                                                    data.F_Class_ID = "030313";
                                                    break;

                                                case "114":
                                                    data.F_Class_ID = "030314";
                                                    break;

                                                case "115":
                                                    data.F_Class_ID = "030315";
                                                    break;

                                                case "116":
                                                    data.F_Class_ID = "030316";
                                                    break;

                                                case "117":
                                                    data.F_Class_ID = "030317";
                                                    break;

                                                case "118":
                                                    data.F_Class_ID = "030318";
                                                    break;

                                                case "119":
                                                    data.F_Class_ID = "030319";
                                                    break;

                                                case "120":
                                                    data.F_Class_ID = "030320";
                                                    break;

                                                case "121":
                                                    data.F_Class_ID = "030321";
                                                    break;

                                                case "122":
                                                    data.F_Class_ID = "030322";
                                                    break;

                                                case "123":
                                                    data.F_Class_ID = "030323";
                                                    break;

                                                case "124":
                                                    data.F_Class_ID = "030324";
                                                    break;

                                                case "125":
                                                    data.F_Class_ID = "030325";
                                                    break;

                                                case "126":
                                                    data.F_Class_ID = "030326";
                                                    break;

                                                case "127":
                                                    data.F_Class_ID = "030327";
                                                    break;

                                                case "128":
                                                    data.F_Class_ID = "030328";
                                                    break;

                                                case "129":
                                                    data.F_Class_ID = "030329";
                                                    break;

                                                case "130":
                                                    data.F_Class_ID = "030330";
                                                    break;

                                                case "131":
                                                    data.F_Class_ID = "030331";
                                                    break;

                                                case "132":
                                                    data.F_Class_ID = "030332";
                                                    break;

                                                case "133":
                                                    data.F_Class_ID = "030333";
                                                    break;

                                                case "134":
                                                    data.F_Class_ID = "030334";
                                                    break;

                                                case "135":
                                                    data.F_Class_ID = "030335";
                                                    break;

                                                case "136":
                                                    data.F_Class_ID = "030336";
                                                    break;

                                                case "137":
                                                    data.F_Class_ID = "030337";
                                                    break;

                                                case "138":
                                                    data.F_Class_ID = "030338";
                                                    break;

                                                case "139":
                                                    data.F_Class_ID = "030339";
                                                    break;

                                                case "140":
                                                    data.F_Class_ID = "030340";
                                                    break;

                                                case "141":
                                                    data.F_Class_ID = "030341";
                                                    break;

                                                case "142":
                                                    data.F_Class_ID = "030342";
                                                    break;
                                            }

                                            #endregion F_Grade_ID 2018级

                                            data.F_Grade_ID = "0303";
                                            break;
                                    }

                                    #endregion F_Divis_ID 精品高中

                                    break;

                                case "04":

                                    #region F_Divis_ID 国际小学

                                    switch (data.F_Grade_ID)
                                    {
                                        case "2013级":

                                            #region F_Class_ID 2013级

                                            switch (data.F_Class_ID)
                                            {
                                                case "6A":
                                                    data.F_Class_ID = "040101";
                                                    break;

                                                case "6B":
                                                    data.F_Class_ID = "040102";
                                                    break;

                                                case "6C":
                                                    data.F_Class_ID = "040103";
                                                    break;
                                            }

                                            #endregion F_Class_ID 2013级

                                            data.F_Grade_ID = "0401";
                                            break;

                                        case "2014级":

                                            #region F_Class_ID 2014级

                                            switch (data.F_Class_ID)
                                            {
                                                case "5A":
                                                    data.F_Class_ID = "040201";
                                                    break;

                                                case "5B":
                                                    data.F_Class_ID = "040202";
                                                    break;

                                                case "5C":
                                                    data.F_Class_ID = "040203";
                                                    break;

                                                case "5D":
                                                    data.F_Class_ID = "040204";
                                                    break;

                                                case "5E":
                                                    data.F_Class_ID = "040205";
                                                    break;
                                            }

                                            #endregion F_Class_ID 2014级

                                            data.F_Grade_ID = "0402";
                                            break;

                                        case "2015级":

                                            #region F_Grade_ID 2015级

                                            switch (data.F_Class_ID)
                                            {
                                                case "4A":
                                                    data.F_Class_ID = "040301";
                                                    break;
                                            }

                                            #endregion F_Grade_ID 2015级

                                            data.F_Grade_ID = "0403";
                                            break;

                                        case "2016级":

                                            #region F_Grade_ID 2016级

                                            switch (data.F_Class_ID)
                                            {
                                                case "3A":
                                                    data.F_Class_ID = "040401";
                                                    break;

                                                case "3B":
                                                    data.F_Class_ID = "040402";
                                                    break;
                                            }

                                            #endregion F_Grade_ID 2016级

                                            data.F_Grade_ID = "0404";
                                            break;

                                        case "2017级":

                                            #region F_Grade_ID 2017级

                                            switch (data.F_Class_ID)
                                            {
                                                case "2A":
                                                    data.F_Class_ID = "040501";
                                                    break;

                                                case "2B":
                                                    data.F_Class_ID = "040502";
                                                    break;
                                            }

                                            #endregion F_Grade_ID 2017级

                                            data.F_Grade_ID = "0405";
                                            break;

                                        case "2018级":

                                            #region F_Grade_ID 2017级

                                            switch (data.F_Class_ID)
                                            {
                                                case "1A":
                                                    data.F_Class_ID = "040601";
                                                    break;

                                                case "1B":
                                                    data.F_Class_ID = "040602";
                                                    break;
                                            }

                                            #endregion F_Grade_ID 2017级

                                            data.F_Grade_ID = "0406";
                                            break;
                                    }

                                    #endregion F_Divis_ID 国际小学

                                    break;

                                case "05":

                                    #region F_Divis_ID 国际初中

                                    switch (data.F_Grade_ID)
                                    {
                                        case "2016级":

                                            #region F_Grade_ID 2016级

                                            switch (data.F_Class_ID)
                                            {
                                                case "9A":
                                                    data.F_Class_ID = "050101";
                                                    break;

                                                case "9B":
                                                    data.F_Class_ID = "050102";
                                                    break;

                                                case "9C":
                                                    data.F_Class_ID = "050103";
                                                    break;

                                                case "9D":
                                                    data.F_Class_ID = "050104";
                                                    break;
                                            }

                                            #endregion F_Grade_ID 2016级

                                            data.F_Grade_ID = "0501";
                                            break;

                                        case "2017级":

                                            #region F_Grade_ID 2017级

                                            switch (data.F_Class_ID)
                                            {
                                                case "8A":
                                                    data.F_Class_ID = "050201";
                                                    break;

                                                case "8B":
                                                    data.F_Class_ID = "050202";
                                                    break;

                                                case "8C":
                                                    data.F_Class_ID = "050203";
                                                    break;

                                                case "8D":
                                                    data.F_Class_ID = "050204";
                                                    break;

                                                case "8E":
                                                    data.F_Class_ID = "050205";
                                                    break;
                                            }

                                            #endregion F_Grade_ID 2017级

                                            data.F_Grade_ID = "0502";
                                            break;

                                        case "2018级":

                                            #region F_Grade_ID 2018级

                                            switch (data.F_Class_ID)
                                            {
                                                case "7A":
                                                    data.F_Class_ID = "050301";
                                                    break;

                                                case "7B":
                                                    data.F_Class_ID = "050302";
                                                    break;

                                                case "7C":
                                                    data.F_Class_ID = "050303";
                                                    break;

                                                case "7D":
                                                    data.F_Class_ID = "050304";
                                                    break;

                                                case "7E":
                                                    data.F_Class_ID = "050305";
                                                    break;

                                                case "7F":
                                                    data.F_Class_ID = "050306";
                                                    break;
                                            }

                                            #endregion F_Grade_ID 2018级

                                            data.F_Grade_ID = "0503";
                                            break;
                                    }

                                    #endregion F_Divis_ID 国际初中

                                    break;

                                case "06":

                                    #region F_Divis_ID 国际高中

                                    switch (data.F_Grade_ID)
                                    {
                                        case "2016级":

                                            #region F_Grade_ID 2016级

                                            switch (data.F_Class_ID)
                                            {
                                                case "3A":
                                                    data.F_Class_ID = "060101";
                                                    break;

                                                case "3B":
                                                    data.F_Class_ID = "060102";
                                                    break;

                                                case "3C":
                                                    data.F_Class_ID = "060103";
                                                    break;

                                                case "3D":
                                                    data.F_Class_ID = "060104";
                                                    break;

                                                case "3E":
                                                    data.F_Class_ID = "060105";
                                                    break;
                                            }

                                            #endregion F_Grade_ID 2016级

                                            data.F_Grade_ID = "0601";
                                            break;

                                        case "2017级":

                                            #region F_Grade_ID 2017级

                                            switch (data.F_Class_ID)
                                            {
                                                case "2A":
                                                    data.F_Class_ID = "060201";
                                                    break;

                                                case "2B":
                                                    data.F_Class_ID = "060202";
                                                    break;

                                                case "2C":
                                                    data.F_Class_ID = "060203";
                                                    break;

                                                case "2D":
                                                    data.F_Class_ID = "060204";
                                                    break;

                                                case "2E":
                                                    data.F_Class_ID = "060205";
                                                    break;

                                                case "2F":
                                                    data.F_Class_ID = "060206";
                                                    break;

                                                case "2G":
                                                    data.F_Class_ID = "060207";
                                                    break;

                                                case "2H":
                                                    data.F_Class_ID = "060208";
                                                    break;
                                            }

                                            #endregion F_Grade_ID 2017级

                                            data.F_Grade_ID = "0602";
                                            break;

                                        case "2018级":

                                            #region F_Grade_ID 2018级

                                            switch (data.F_Class_ID)
                                            {
                                                case "1A":
                                                    data.F_Class_ID = "060301";
                                                    break;

                                                case "1B":
                                                    data.F_Class_ID = "060302";
                                                    break;

                                                case "1C":
                                                    data.F_Class_ID = "060303";
                                                    break;

                                                case "1D":
                                                    data.F_Class_ID = "060304";
                                                    break;

                                                case "1E":
                                                    data.F_Class_ID = "060305";
                                                    break;
                                            }

                                            #endregion F_Grade_ID 2018级

                                            data.F_Grade_ID = "0603";
                                            break;
                                    }

                                    #endregion F_Divis_ID 国际高中

                                    break;
                            }

                            data.F_DepartmentId = data.F_Divis_ID;
                            try
                            {
                                data.F_Subjects_ID = db.FindEntity<SysOrganize>(t => t.F_EnCode.Equals(data.F_Class_ID))
                                    .F_Class_Type;
                            }
                            catch
                            {
                                list.Add("<p>" + k + ". 学生姓名为'" + data.F_Name + "'新增失败,未找到班级" + data.F_Class_ID +
                                         "</p> ");
                                k++;
                                continue;
                            }

                            data.F_CurStatu = "1";
                            //注册人信息
                            //OperatorModel Oper = OperatorProvider.Current;
                            entity.F_RegUsers_ID = "xuexiaozhaosheng"; //tea.First().F_User_ID;
                            entity.F_RegUserType = "teacherDuty";
                            entity.F_RegisterName = "xuexiaozhaosheng"; //data.F_RegisterName;
                            entity.F_RegisterNum = "123456789"; //tea.First().F_Num;
                            entity.F_RegisterPhone = "123456789"; //tea.First().F_MobilePhone;

                            entity = ExtObj.ClonePropValue(data, entity);
                            entity.F_Year = Convert.ToInt32(data.F_Year);
                            //签字协议状态
                            entity.F_Signed_License_Status = "1";
                            //学费缴纳状态 F_Sundry_Status
                            entity.F_Sundry_Status = "1";
                            //保额金缴纳状态 F_Prepay_Status
                            entity.F_Prepay_Status = "1";
                            //信息完善状态 F_Date_Status
                            entity.F_Date_Status = "1";
                            //报名协议状态
                            entity.F_Statu = "4";
                            //报名序号
                            entity.F_InitNum = "s" + NumberBuilder.Build_18bit();
                            entity.F_DepartmentId = entity.F_Divis_ID;
                            entity.F_Charge_mode = "分学年";
                            if (string.IsNullOrEmpty(entity.F_StudentNum))
                            {
                                var stuNo = new NoSeedRepository().getStuNum(Convert.ToInt32(entity.F_Year),
                                    entity.F_Divis_ID);
                                entity.F_StudentNum = stuNo;
                            }
                            else
                            {
                                var entry = new EntrySignUpRepository().QueryAsNoTracking()
                                    .Where(t => t.F_StudentNum.Equals(entity.F_StudentNum));
                                //是否被使用
                                if (entry.Count() <= 0)
                                {
                                    var seed = db.FindEntity<NoSeed>(t =>
                                        t.F_Divis == entity.F_Divis_ID && t.F_Year == entity.F_Year &&
                                        t.F_Type == "student");
                                    if (seed == null)
                                    {
                                        seed = new NoSeed();
                                        seed.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                                        seed.F_Divis = entity.F_Divis_ID;
                                        seed.F_Year = entity.F_Year;
                                        seed.F_Type = "student";
                                        seed.F_No = 1;
                                        db.Insert(seed);
                                    }
                                    else
                                    {
                                        if (Convert.ToInt32(
                                                entity.F_StudentNum.Substring(entity.F_StudentNum.Length - 4, 4)) + 1 >
                                            seed.F_No)
                                        {
                                            seed.F_No = Convert.ToInt32(
                                                            entity.F_StudentNum.Substring(
                                                                entity.F_StudentNum.Length - 4, 4)) + 1;
                                            db.Update(seed);
                                        }
                                        else
                                        {
                                            seed.F_No = seed.F_No + 1;
                                            db.Update(seed);
                                        }
                                    }
                                }
                                else
                                {
                                    list.Add("<p>" + k + ". 学生姓名为'" + entity.F_Name + "'新增失败（学号已被使用）</p> ");
                                    k++;
                                    continue;
                                }
                            }

                            entity.Create();
                            entity.F_Subjects_Name = entity.F_Subjects_ID;
                            //data.Create();
                            var user = new SysUser
                            {
                                F_Id = Guid.NewGuid().ToString(),
                                F_Account = entity.F_StudentNum,
                                F_DutyId = "studentDuty",
                                F_Gender = Convert.ToBoolean(Convert.ToInt32(entity.F_Gender)),
                                F_RoleId = "student",
                                F_MobilePhone = entity.F_Tel,
                                F_RealName = entity.F_Name,
                                F_OrganizeId = "1",
                                //分班的时候赋值
                                //u.F_DepartmentId
                                F_NickName = entity.F_Name,
                                F_IsAdministrator = false,
                                F_EnabledMark = true,
                                F_HeadIcon = entity.F_FacePic_File
                            };
                            var userLogOnEntity = new SysUserLogin();
                            userLogOnEntity.F_Id = user.F_Id;
                            userLogOnEntity.F_UserId = user.F_Id;
                            userLogOnEntity.F_UserSecretkey = Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                            userLogOnEntity.F_UserPassword = Md5EncryptHelper
                                .Encrypt(
                                    DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt("123456", 32).ToLower(),
                                        userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                            //userservice.Insert(user);
                            //userlogservice.Insert(userLogOnEntity);
                            data.F_Users_ID = user.F_Id;
                            var us = new SysUserRole();
                            us.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                            us.F_User = user.F_Id;
                            us.F_Role = user.F_RoleId;
                            db.Insert(us);
                            if (!string.IsNullOrEmpty(entity.F_Guarder_Tel) && !string.IsNullOrEmpty(entity.F_Guarder))
                            {
                                var F_Guarder_Tel = entity.F_Guarder_Tel.Split('，');
                                for (var i = 0; i < F_Guarder_Tel.Length; i++)
                                {
                                    var j = db.FindEntity<SysUser>(t => t.F_Account == entity.F_Guarder_Tel);
                                    var userLogOnEntity2 = new SysUserLogin();
                                    //bool guarder = false;
                                    if (j == null)
                                    {
                                        //guarder = true;
                                        j = new SysUser();
                                        j.F_Account = F_Guarder_Tel[i];
                                        j.F_DutyId = "parentDuty";
                                        j.F_Gender = "1".Equals(entity.F_Gender) ? true : false;
                                        j.F_RoleId = "parent";
                                        j.F_MobilePhone = F_Guarder_Tel[i];
                                        j.F_RealName = entity.F_Guarder;
                                        j.F_OrganizeId = "1";
                                        //分班的时候赋值
                                        j.F_DepartmentId = "parent";
                                        j.F_NickName = entity.F_Guarder;
                                        j.F_IsAdministrator = false;
                                        j.F_EnabledMark = true;
                                        //j.F_HeadIcon = e.F_FacePic_File;
                                        j.Create();

                                        userLogOnEntity2.F_Id = j.F_Id;
                                        userLogOnEntity2.F_UserId = j.F_Id;
                                        userLogOnEntity2.F_UserSecretkey =
                                            Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                                        //生成密码 监护人证件号后6位
                                        var pwd2 = "000000";
                                        if (!entity.F_Guarder_CredNum.IsEmpty() && entity.F_Guarder_CredNum.Length >= 6)
                                            pwd2 = entity.F_Guarder_CredNum.Substring(
                                                entity.F_Guarder_CredNum.Length - 6, 6);
                                        pwd2 = pwd2.ToLower().Replace("x", "0"); //密码x变为0
                                        userLogOnEntity2.F_UserPassword = Md5EncryptHelper
                                            .Encrypt(
                                                DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(pwd2, 32),
                                                    userLogOnEntity2.F_UserSecretkey), 32).ToLower();
                                        db.Insert(j);
                                        db.Insert(userLogOnEntity2);

                                        var jus = new SysUserRole();
                                        jus.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                                        jus.F_User = j.F_Id;
                                        jus.F_Role = j.F_RoleId;
                                        db.Insert(jus);
                                    }

                                    entity.F_FamilyID = j.F_Id; //家长ID
                                    entity.F_Guarder_Tel = F_Guarder_Tel[i];
                                    data.F_Guarder_Tel = F_Guarder_Tel[i];
                                }
                            }
                            //else {
                            //    list.Add("<p>" + k + ". 学生姓名为'" + data.F_Name + "'家长信息不全</p> ");
                            //    k++;
                            //    continue;
                            //}

                            //entservice.Insert(entity);

                            var s = new Student();
                            s = ExtObj.ClonePropValue(data, s);
                            //s.Create();
                            s.F_InitNum = entity.F_InitNum;
                            s.F_StudentNum = entity.F_StudentNum;
                            //service.Insert(s);
                            db.Insert(entity);
                            db.Insert(user);
                            db.Insert(userLogOnEntity);

                            s.Create();
                            db.Insert(s);
                            //service.AddDatas(entity, user, userLogOnEntity, j, userLogOnEntity2, s, guarder);
                            db.Commit();
                        }
                    }
                    catch
                    {
                        list.Add("<p>" + k + ". 学生姓名为'" + data.F_Name + "'新增失败</p> ");
                        k++;
                        continue;
                    }
                else
                    try
                    {
                        using (var db = new Data.UnitWork().BeginTrans())
                        {
                            var stuentity = db.Find<Student>(data.F_Id);

                            var expression = ExtLinq.True<EntrySignUp>();
                            if (!string.IsNullOrEmpty(stuentity.F_InitNum))
                                expression = expression.And(t => t.F_InitNum.Equals(stuentity.F_InitNum));
                            expression = expression.And(t => t.F_Statu != "3");
                            var entity = db.FindEntity(expression);
                            //School_EntrySignUp_Entity entity = new School_EntrySignUp_App().GetFormByF_InitNum(stuentity.F_InitNum);

                            entity = ExtObj.ClonePropValue(data, entity);

                            //stuentity = ExtObj.clonePropValue(data, stuentity);
                            //stuentity.F_Id = data.F_Id;
                            var entry = new EntrySignUpRepository().QueryAsNoTracking()
                                .Where(t => t.F_StudentNum.Equals(entity.F_StudentNum));
                            if (entry.Count() > 0)
                            {
                                if (entry.First().F_Id != entity.F_Id)
                                {
                                    list.Add("<p>" + k + ". 学生姓名为'" + entity.F_Name + "'更新失败（学号已被使用）</p> ");
                                    k++;
                                    continue;
                                }
                            }
                            else
                            {
                                var org = db.FindEntity<SysOrganize>(t => t.F_Id == entity.F_Divis_ID);
                                var no = org.F_EnCode + entity.F_Year;
                                var len = org.F_EnCode.Length + 4;
                                if (entity.F_StudentNum.Substring(0, len) == no)
                                {
                                    var F_Year = Convert.ToInt32(entity.F_Year);
                                    var seed = db.FindEntity<NoSeed>(t =>
                                        t.F_Divis == entity.F_Divis_ID && t.F_Year == F_Year && t.F_Type == "student");
                                    if (seed == null)
                                    {
                                        seed = new NoSeed();
                                        seed.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                                        seed.F_Divis = entity.F_Divis_ID;
                                        seed.F_Year = entity.F_Year;
                                        seed.F_Type = "student";
                                        seed.F_No = 1;
                                        db.Insert(seed);
                                    }
                                    else
                                    {
                                        if (Convert.ToInt32(
                                                entity.F_StudentNum.Substring(entity.F_StudentNum.Length - 4, 4)) + 1 >
                                            seed.F_No)
                                        {
                                            seed.F_No = Convert.ToInt32(
                                                            entity.F_StudentNum.Substring(
                                                                entity.F_StudentNum.Length - 4, 4)) + 1;
                                            db.Update(seed);
                                        }
                                        else
                                        {
                                            seed.F_No = seed.F_No + 1;
                                            db.Update(seed);
                                        }
                                    }
                                    //seed.F_No = Convert.ToInt32(entity.F_StudentNum.Substring(entity.F_StudentNum.Length - 4, 4)) + 1;
                                    //seedservice.Update(seed);

                                    //getStuNum(Convert.ToInt32(data.F_Year), data.F_Divis_ID);
                                }
                                else
                                {
                                    list.Add("<p>" + k + ". 学生姓名为'" + entity.F_Name + "'更新失败（学号规则错误）</p> ");
                                    k++;
                                    continue;
                                }
                            }

                            var u = db.Find<SysUser>(stuentity.F_Users_ID);
                            u.F_Account = entity.F_StudentNum;
                            u.F_Gender = Convert.ToBoolean(Convert.ToInt32(entity.F_Gender));
                            u.F_MobilePhone = entity.F_Tel;
                            u.F_RealName = entity.F_Name;
                            u.F_NickName = entity.F_Name;
                            u.F_HeadIcon = entity.F_FacePic_File;
                            var userLogOnEntity = new SysUserLogin();
                            userLogOnEntity.F_Id = u.F_Id;
                            userLogOnEntity.F_UserId = u.F_Id;
                            userLogOnEntity.F_UserSecretkey = Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                            var pwd1 = "000000";
                            if (!entity.F_Guarder_CredNum.IsEmpty() && entity.F_Guarder_CredNum.Length >= 6)
                                pwd1 = entity.F_Guarder_CredNum.Substring(entity.F_Guarder_CredNum.Length - 6, 6);
                            pwd1 = pwd1.ToLower().Replace("x", "0"); //密码x变为0
                            userLogOnEntity.F_UserPassword = Md5EncryptHelper
                                .Encrypt(
                                    DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(pwd1, 32).ToLower(),
                                        userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                            //userservice.Insert(user);
                            //userlogservice.Insert(userLogOnEntity);
                            data.F_Users_ID = u.F_Id;

                            var j = db.FindEntity<SysUser>(t => t.F_Account == entity.F_Guarder_Tel);
                            var userLogOnEntity2 = new SysUserLogin();
                            //bool guarder = false;
                            if (j == null)
                            {
                                //guarder = true;
                                j = new SysUser();
                                j.F_Account = entity.F_Guarder_Tel;
                                j.F_DutyId = "parentDuty";
                                j.F_Gender = "1".Equals(entity.F_Gender) ? true : false;
                                j.F_RoleId = "parent";
                                j.F_MobilePhone = entity.F_Guarder_Tel;
                                j.F_RealName = entity.F_Guarder;
                                j.F_OrganizeId = "1";
                                //分班的时候赋值
                                j.F_DepartmentId = "parent";
                                j.F_NickName = entity.F_Guarder;
                                j.F_IsAdministrator = false;
                                j.F_EnabledMark = true;
                                //j.F_HeadIcon = e.F_FacePic_File;
                                j.Create();

                                userLogOnEntity2.F_Id = j.F_Id;
                                userLogOnEntity2.F_UserId = j.F_Id;
                                userLogOnEntity2.F_UserSecretkey =
                                    Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                                //生成密码 监护人证件号后6位
                                var pwd2 = "000000";
                                if (!entity.F_Guarder_CredNum.IsEmpty() && entity.F_Guarder_CredNum.Length >= 6)
                                    pwd2 = entity.F_Guarder_CredNum.Substring(entity.F_Guarder_CredNum.Length - 6, 6);
                                pwd2 = pwd2.ToLower().Replace("x", "0"); //密码x变为0
                                userLogOnEntity2.F_UserPassword = Md5EncryptHelper
                                    .Encrypt(
                                        DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(pwd2, 32),
                                            userLogOnEntity2.F_UserSecretkey), 32).ToLower();
                                db.Insert(j);
                                db.Insert(userLogOnEntity2);

                                var jus = new SysUserRole();
                                jus.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                                jus.F_User = j.F_Id;
                                jus.F_Role = j.F_RoleId;
                                db.Insert(jus);
                            }
                            else
                            {
                                //guarder = false;
                                j.F_Account = entity.F_Guarder_Tel;
                                j.F_Gender = "1".Equals(entity.F_Gender) ? true : false;
                                j.F_MobilePhone = entity.F_Guarder_Tel;
                                j.F_RealName = entity.F_Guarder;
                                j.F_NickName = entity.F_Guarder;

                                userLogOnEntity2.F_Id = j.F_Id;
                                userLogOnEntity2.F_UserId = j.F_Id;
                                userLogOnEntity2.F_UserSecretkey =
                                    Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                                //生成密码 监护人证件号后6位
                                var pwd2 = "000000";
                                if (!entity.F_Guarder_CredNum.IsEmpty() && entity.F_Guarder_CredNum.Length >= 6)
                                    pwd2 = entity.F_Guarder_CredNum.Substring(entity.F_Guarder_CredNum.Length - 6, 6);
                                pwd2 = pwd2.ToLower().Replace("x", "0"); //密码x变为0
                                userLogOnEntity2.F_UserPassword = Md5EncryptHelper
                                    .Encrypt(
                                        DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(pwd2, 32),
                                            userLogOnEntity2.F_UserSecretkey), 32).ToLower();
                                db.Update(j);
                                db.Update(userLogOnEntity2);
                            }

                            entity.F_FamilyID = j.F_Id; //家长ID
                            //entservice.Insert(entity);

                            stuentity = ExtObj.ClonePropValue(data, stuentity);
                            //s.Create();
                            stuentity.F_InitNum = entity.F_InitNum;
                            stuentity.F_StudentNum = entity.F_StudentNum;
                            //service.Insert(s);
                            db.Update(entity);
                            db.Update(u);
                            db.Update(userLogOnEntity);
                            stuentity.Modify(stuentity.F_Id);
                            db.Update(stuentity);
                            //service.AddDatas(entity, u, userLogOnEntity, j, userLogOnEntity2, stuentity, guarder);
                            db.Commit();
                        }
                    }
                    catch
                    {
                        list.Add("<p>" + k + ". 学生姓名为'" + data.F_Name + "'更新失败</p> ");
                        k++;
                        continue;
                    }

            if (list.Count() > 0)
            {
                for (var i = 0; i < list.Count(); i++)
                    //if (list.Count() - i == 1)
                    //{
                    mod += list[i];
                //}
                //else
                //    mod += list[i].ToString() + " and ";

                throw new Exception(mod);
            }
        }

        public void AddDatas_gb(List<Student> datas)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                try
                {
                    //var list = new List<string>();
                    //var mod = "<p>导入错误如下：</p>";
                    //var k = 1;
                    foreach (var data in datas)
                        if (data.F_Id.IsEmpty())
                            try
                            {
                                if (string.IsNullOrEmpty(data.F_StudentNum))
                                {
                                    var org = db.FindEntity<SysOrganize>(t => t.F_Id == data.F_Divis_ID);
                                    var F_Year = Convert.ToInt32(data.F_Year);
                                    var seed = db.FindEntity<NoSeed>(t =>
                                        t.F_Divis == data.F_Divis_ID && t.F_Year == F_Year &&
                                        t.F_Type == "student");

                                    var no = org.F_EnCode + data.F_Year;
                                    if (seed == null)
                                    {
                                        seed = new NoSeed();
                                        seed.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                                        seed.F_Divis = data.F_Divis_ID;
                                        seed.F_Year = F_Year;
                                        seed.F_Type = "student";
                                        seed.F_No = 1;
                                        db.Insert(seed);
                                    }
                                    else
                                    {
                                        seed.F_No = seed.F_No + 1;
                                        db.Update(seed);
                                    }

                                    var stuNo = no + getStrNum(Convert.ToString(seed.F_No));
                                    //string stuNo = getStuNum(Convert.ToInt32(entity.F_Year), entity.F_Divis_ID,db);
                                    data.F_StudentNum = stuNo;
                                }
                                else
                                {
                                    var entry = new EntrySignUpRepository().QueryAsNoTracking()
                                        .Where(t => t.F_StudentNum.Equals(data.F_StudentNum));
                                    //是否被使用
                                    if (!entry.Any())
                                    {
                                        var org = db.FindEntity<SysOrganize>(t => t.F_Id == data.F_Divis_ID);
                                        var F_Year = Convert.ToInt32(data.F_Year);
                                        var no = org.F_EnCode + data.F_Year;
                                        var len = org.F_EnCode.Length + 4;
                                        if (data.F_StudentNum.Substring(0, len) == no)
                                        {
                                            var seed = db.FindEntity<NoSeed>(t =>
                                                t.F_Divis == data.F_Divis_ID && t.F_Year == F_Year &&
                                                t.F_Type == "student");
                                            seed.F_No = Convert.ToInt32(
                                                            data.F_StudentNum.Substring(data.F_StudentNum.Length - 4,
                                                                4)) + 1;
                                            db.Update(seed);
                                        }
                                        else
                                        {
                                            throw new Exception("学生姓名为'" + data.F_Name + "'新增失败（学号规则错误）");
                                            //list.Add("<p>" + k + ". 学生姓名为'" + data.F_Name + "'新增失败（学号规则错误）</p> ");
                                            //k++;
                                            //continue;
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("学生姓名为'" + data.F_Name + "'新增失败（学号已被使用）");
                                        //list.Add("<p>" + k + ". 学生姓名为'" + data.F_Name + "'新增失败（学号已被使用）</p> ");
                                        //k++;
                                        //continue;
                                    }
                                }

                                //entity.Create();

                                //data.Create();
                                var user = new SysUser
                                {
                                    F_Id = Guid.NewGuid().ToString(),
                                    F_Account = data.F_StudentNum,
                                    F_DutyId = "studentDuty",
                                    F_Gender = Convert.ToBoolean(Convert.ToInt32(data.F_Gender)),
                                    F_RoleId = "student",
                                    F_MobilePhone = data.F_Tel,
                                    F_RealName = data.F_Name,
                                    F_OrganizeId = "1",
                                    //分班的时候赋值
                                    //u.F_DepartmentId
                                    F_NickName = data.F_Name,
                                    F_IsAdministrator = false,
                                    F_EnabledMark = true,
                                    F_HeadIcon = data.F_FacePic_File
                                };
                                var userLogOnEntity = new SysUserLogin
                                {
                                    F_Id = user.F_Id,
                                    F_UserId = user.F_Id,
                                    F_UserSecretkey = Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower()
                                };
                                userLogOnEntity.F_UserPassword = Md5EncryptHelper
                                    .Encrypt(
                                        DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt("123456", 32).ToLower(),
                                            userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                                db.Insert(user);
                                db.Insert(userLogOnEntity);

                                var us = new SysUserRole();
                                us.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                                us.F_User = user.F_Id;
                                us.F_Role = user.F_RoleId;
                                db.Insert(us);

                                var isPost = true;
                                var F_Birthday = "";
                                if (user.F_Birthday != null) F_Birthday = user.F_Birthday.ToDateTimeString();
                                var parameters = "sysid=" + user.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                                 user.F_RealName + "&amp;username=" + user.F_Account +
                                                 "&amp;password=" + userLogOnEntity.F_UserPassword +
                                                 "&amp;sysgroupid=" + user.F_DepartmentId + "&amp;headicon=" +
                                                 user.F_HeadIcon + "&amp;birthday=" + F_Birthday + "";
                                WebHelper.SendRequest(CallUrlAdd, parameters, isPost, "application/json");
                                data.F_Users_ID = user.F_Id;

                                var j = db.FindEntity<SysUser>(t => t.F_Account == data.F_Guarder_Tel);

                                if (j == null)
                                {
                                    j = new SysUser();
                                    j.F_Account = data.F_Guarder_Tel;
                                    j.F_DutyId = "parentDuty";
                                    j.F_Gender = "1".Equals(data.F_Gender) ? true : false;
                                    j.F_RoleId = "parent";
                                    j.F_MobilePhone = data.F_Guarder_Tel;
                                    j.F_RealName = data.F_Guarder;
                                    j.F_OrganizeId = "1";
                                    //分班的时候赋值
                                    j.F_DepartmentId = "parent";
                                    j.F_NickName = data.F_Guarder;
                                    j.F_IsAdministrator = false;
                                    j.F_EnabledMark = true;
                                    //j.F_HeadIcon = e.F_FacePic_File;
                                    j.Create();

                                    var userLogOnEntity2 = new SysUserLogin();
                                    userLogOnEntity2.F_Id = j.F_Id;
                                    userLogOnEntity2.F_UserId = j.F_Id;
                                    userLogOnEntity2.F_UserSecretkey =
                                        Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                                    //生成密码 监护人证件号后6位
                                    var pwd2 = "000000";
                                    if (!data.F_Guarder_CredNum.IsEmpty() && data.F_Guarder_CredNum.Length >= 6)
                                        pwd2 = data.F_Guarder_CredNum.Substring(data.F_Guarder_CredNum.Length - 6, 6);
                                    pwd2 = pwd2.ToLower().Replace("x", "0"); //密码x变为0
                                    userLogOnEntity2.F_UserPassword = Md5EncryptHelper
                                        .Encrypt(
                                            DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(pwd2, 32),
                                                userLogOnEntity2.F_UserSecretkey), 32).ToLower();
                                    db.Insert(j);
                                    db.Insert(userLogOnEntity2);

                                    var jus = new SysUserRole();
                                    jus.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                                    jus.F_User = j.F_Id;
                                    jus.F_Role = j.F_RoleId;
                                    db.Insert(jus);

                                    if (j.F_Birthday != null)
                                        F_Birthday = j.F_Birthday.ToDateTimeString();
                                    else F_Birthday = "";
                                    parameters = "sysid=" + j.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                                 j.F_RealName + "&amp;username=" + j.F_Account + "&amp;password=" +
                                                 userLogOnEntity2.F_UserPassword + "&amp;sysgroupid=" +
                                                 j.F_DepartmentId + "&amp;headicon=" + j.F_HeadIcon + "&amp;birthday=" +
                                                 F_Birthday + "";
                                    WebHelper.SendRequest(CallUrlAdd, parameters, isPost, "application/json");

                                    data.F_Users_ID = user.F_Id;
                                }

                                //School_Students_Entity s = new School_Students_Entity();
                                //s = ExtObj.clonePropValue(data, s);
                                data.Create();
                                data.F_InitNum = "s" + NumberBuilder.Build_18bit();
                                //data.F_StudentNum = entity.F_StudentNum;
                                db.Insert(data);
                            }
                            catch
                            {
                                throw new Exception("学生姓名为'" + data.F_Name + "'新增失败");
                                //list.Add("<p>" + k + ". 学生姓名为'" + data.F_Name + "'新增失败</p> ");
                                //k++;
                                //continue;
                            }
                        else
                            try
                            {
                                //School_Students_Entity stuentity = db.FindEntity<School_Students_Entity>(t => t.F_Id == data.F_Id); //GetForm(data.F_Id);

                                //entity = ExtObj.clonePropValue(data, entity);

                                //stuentity = ExtObj.clonePropValue(data, stuentity);
                                //stuentity.F_Id = data.F_Id;
                                var entrystu = new StudentRepository().QueryAsNoTracking()
                                    .Where(t => t.F_StudentNum.Equals(data.F_StudentNum));
                                if (entrystu.Any())
                                {
                                    if (entrystu.First().F_Id != data.F_Id)
                                    {
                                        throw new Exception("学生姓名为'" + data.F_Name + "'更新失败（学号已被使用）");
                                        //list.Add("<p>" + k + ". 学生姓名为'" + data.F_Name + "'更新失败（学号已被使用）</p> ");
                                        //k++;
                                        //continue;
                                    }
                                }
                                else
                                {
                                    var org = db.FindEntity<SysOrganize>(t => t.F_Id == data.F_Divis_ID);
                                    var no = org.F_EnCode + data.F_Year;
                                    var len = org.F_EnCode.Length + 4;
                                    if (data.F_StudentNum.Substring(0, len) == no)
                                    {
                                        var F_Year = Convert.ToInt32(data.F_Year);
                                        var seed = db.FindEntity<NoSeed>(t =>
                                            t.F_Divis == data.F_Divis_ID && t.F_Year == F_Year &&
                                            t.F_Type == "student");
                                        seed.F_No = Convert.ToInt32(
                                                        data.F_StudentNum.Substring(data.F_StudentNum.Length - 4,
                                                            4)) + 1;
                                        db.Update(seed);

                                        //getStuNum(Convert.ToInt32(data.F_Year), data.F_Divis_ID);
                                    }
                                    else
                                    {
                                        throw new Exception("学生姓名为'" + data.F_Name + "'更新失败（学号规则错误）");
                                        //list.Add("<p>" + k + ". 学生姓名为'" + data.F_Name + "'更新失败（学号规则错误）</p> ");
                                        //k++;
                                        //continue;
                                    }
                                }

                                var u = db.FindEntity<SysUser>(t =>
                                    t.F_Id == data.F_Users_ID); //new UserApp().GetForm(stuentity.F_Users_ID);
                                u.F_Account = data.F_StudentNum;
                                u.F_Gender = Convert.ToBoolean(Convert.ToInt32(data.F_Gender));
                                u.F_MobilePhone = data.F_Tel;
                                u.F_RealName = data.F_Name;
                                u.F_NickName = data.F_Name;
                                u.F_HeadIcon = data.F_FacePic_File;
                                var userLogOnEntity = new SysUserLogin();
                                userLogOnEntity.F_Id = u.F_Id;
                                userLogOnEntity.F_UserId = u.F_Id;
                                userLogOnEntity.F_UserSecretkey =
                                    Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                                var pwd1 = "000000";
                                if (!data.F_Guarder_CredNum.IsEmpty() && data.F_Guarder_CredNum.Length >= 6)
                                    pwd1 = data.F_Guarder_CredNum.Substring(data.F_Guarder_CredNum.Length - 6, 6);
                                pwd1 = pwd1.ToLower().Replace("x", "0"); //密码x变为0
                                userLogOnEntity.F_UserPassword = Md5EncryptHelper
                                    .Encrypt(
                                        DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(pwd1, 32).ToLower(),
                                            userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                                db.Update(u);
                                db.Update(userLogOnEntity);

                                var isPost = true;
                                var F_Birthday = "";
                                if (u.F_Birthday != null) F_Birthday = u.F_Birthday.ToDateTimeString();
                                var parameters = "sysid=" + u.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                                 u.F_RealName + "&amp;username=" + u.F_Account + "&amp;password=" +
                                                 userLogOnEntity.F_UserPassword + "&amp;sysgroupid=" +
                                                 u.F_DepartmentId + "&amp;headicon=" + u.F_HeadIcon + "&amp;birthday=" +
                                                 F_Birthday + "";
                                WebHelper.SendRequest(CallUrlUpt, parameters, isPost, "application/json");

                                data.F_Users_ID = u.F_Id;

                                var j = db.FindEntity<SysUser>(t => t.F_Account == data.F_Guarder_Tel);
                                var userLogOnEntity2 = new SysUserLogin();
                                //bool guarder = false;
                                if (j == null)
                                {
                                    //guarder = true;
                                    j = new SysUser();
                                    j.F_Account = data.F_Guarder_Tel;
                                    j.F_DutyId = "parentDuty";
                                    j.F_Gender = "1".Equals(data.F_Gender) ? true : false;
                                    j.F_RoleId = "parent";
                                    j.F_MobilePhone = data.F_Guarder_Tel;
                                    j.F_RealName = data.F_Guarder;
                                    j.F_OrganizeId = "1";
                                    //分班的时候赋值
                                    j.F_DepartmentId = "parent";
                                    j.F_NickName = data.F_Guarder;
                                    j.F_IsAdministrator = false;
                                    j.F_EnabledMark = true;
                                    //j.F_HeadIcon = e.F_FacePic_File;
                                    j.Create();

                                    userLogOnEntity2.F_Id = j.F_Id;
                                    userLogOnEntity2.F_UserId = j.F_Id;
                                    userLogOnEntity2.F_UserSecretkey =
                                        Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                                    //生成密码 监护人证件号后6位
                                    var pwd2 = "000000";
                                    if (!data.F_Guarder_CredNum.IsEmpty() && data.F_Guarder_CredNum.Length >= 6)
                                        pwd2 = data.F_Guarder_CredNum.Substring(data.F_Guarder_CredNum.Length - 6,
                                            6);
                                    pwd2 = pwd2.ToLower().Replace("x", "0"); //密码x变为0
                                    userLogOnEntity2.F_UserPassword = Md5EncryptHelper
                                        .Encrypt(
                                            DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(pwd2, 32),
                                                userLogOnEntity2.F_UserSecretkey), 32).ToLower();
                                    db.Insert(j);
                                    db.Insert(userLogOnEntity2);

                                    var jus = new SysUserRole();
                                    jus.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                                    jus.F_User = j.F_Id;
                                    jus.F_Role = j.F_RoleId;
                                    db.Insert(jus);

                                    if (j.F_Birthday != null)
                                        F_Birthday = j.F_Birthday.ToDateTimeString();
                                    else F_Birthday = "";
                                    parameters = "sysid=" + j.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                                 j.F_RealName + "&amp;username=" + j.F_Account + "&amp;password=" +
                                                 userLogOnEntity2.F_UserPassword + "&amp;sysgroupid=" +
                                                 j.F_DepartmentId + "&amp;headicon=" + j.F_HeadIcon + "&amp;birthday=" +
                                                 F_Birthday + "";
                                    WebHelper.SendRequest(CallUrlAdd, parameters, isPost, "application/json");
                                }
                                else
                                {
                                    //guarder = false;
                                    j.F_Account = data.F_Guarder_Tel;
                                    j.F_Gender = "1".Equals(data.F_Gender) ? true : false;
                                    j.F_MobilePhone = data.F_Guarder_Tel;
                                    j.F_RealName = data.F_Guarder;
                                    j.F_NickName = data.F_Guarder;

                                    userLogOnEntity2.F_Id = j.F_Id;
                                    userLogOnEntity2.F_UserId = j.F_Id;
                                    userLogOnEntity2.F_UserSecretkey =
                                        Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                                    //生成密码 监护人证件号后6位
                                    var pwd2 = "000000";
                                    if (!data.F_Guarder_CredNum.IsEmpty() && data.F_Guarder_CredNum.Length >= 6)
                                        pwd2 = data.F_Guarder_CredNum.Substring(data.F_Guarder_CredNum.Length - 6,
                                            6);
                                    pwd2 = pwd2.ToLower().Replace("x", "0"); //密码x变为0
                                    userLogOnEntity2.F_UserPassword = Md5EncryptHelper
                                        .Encrypt(
                                            DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(pwd2, 32),
                                                userLogOnEntity2.F_UserSecretkey), 32).ToLower();
                                    db.Update(j);
                                    db.Update(userLogOnEntity2);

                                    if (j.F_Birthday != null)
                                        F_Birthday = j.F_Birthday.ToDateTimeString();
                                    else F_Birthday = "";
                                    parameters = "sysid=" + j.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                                 j.F_RealName + "&amp;username=" + j.F_Account + "&amp;password=" +
                                                 userLogOnEntity2.F_UserPassword + "&amp;sysgroupid=" +
                                                 j.F_DepartmentId + "&amp;headicon=" + j.F_HeadIcon + "&amp;birthday=" +
                                                 F_Birthday + "";
                                    WebHelper.SendRequest(CallUrlUpt, parameters, isPost, "application/json");
                                }

                                //stuentity = ExtObj.clonePropValue(data, stuentity);
                                //stuentity.Modify(stuentity.F_Id);
                                //stuentity.F_InitNum = entity.F_InitNum;
                                //stuentity.F_StudentNum = entity.F_StudentNum;
                                db.Update(data);
                            }
                            catch //(Exception ex)
                            {
                                throw new Exception("学生姓名为'" + data.F_Name + "'更新失败");
                                //list.Add("<p>" + k + ". 学生姓名为'" + data.F_Name + "'更新失败</p> ");
                                //k++;
                                //continue;
                            }

                    //if (list.Any())
                    //{
                    //    for (var i = 0; i < list.Count(); i++) mod += list[i];

                    //    throw new Exception(mod);
                    //}

                    db.Commit();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}