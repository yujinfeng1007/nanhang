using System;
using System.Collections.Generic;
using System.Linq;
using ZHXY.Common;

namespace ZHXY.Domain
{
    public class TeacherRepository : Repository<Teacher>, ITeacherRepository
    {
        public TeacherRepository( ) : base()
        {
        }


        private string getStrNum(string num, int leng = 4)
        {
            var len = num.Length;
            for (var i = 0; i < leng - len; i++) num = "0" + num;
            return num;
        }

        public object GetPropertyValue(object info, string field)
        {
            if (info == null) return null;
            var t = info.GetType();
            var property = from pi in t.GetProperties() where pi.Name.ToLower() == field.ToLower() select pi;
            return property.First().GetValue(info, null);
        }

        public object GetRoleList()
        {
            var expression = ExtLinq.True<Role>();
            expression = expression.And(t => t.Category == 1);
            var data = role.QueryAsNoTracking(expression).OrderBy(t => t.SortCode).ToList();
            var dictionary = new Dictionary<string, object>();
            foreach (var item in data)
            {
                var fieldItem = new fieldItem();
                fieldItem.encode = item.EnCode;
                fieldItem.fullname = item.Name;
                dictionary.Add(item.Id, fieldItem);
            }

            return dictionary;
        }

        public Dictionary<string, object> GetRoleListByCache()
        {
            var cache = CacheFactory.Cache();
            if (CacheFactory.Cache().GetCache<Dictionary<string, object>>("role").IsEmpty())
                cache.WriteCache((Dictionary<string, object>)GetRoleList(), "role");

            return cache.GetCache<Dictionary<string, object>>("role");
        }

        private readonly string appid = Configs.GetValue("appid");
        private readonly string CallUrlAdd = Configs.GetValue("CallUrlAdd");
        private readonly string CallUrlUpt = Configs.GetValue("CallUrlUpt");
        private readonly RoleRepository role = new RoleRepository();
        private readonly WebHelper webhelp = new WebHelper();

        public new void Delete(string keyValue)
        {
            using (var db = new UnitWork().BeginTrans())
            {
                var F_Id = keyValue.Split('|');
                for (var i = 0; i < F_Id.Length; i++)
                {
                    var model = db.Find<Teacher>(F_Id[i]);
                    if (model != null)
                    {
                        db.Delete<User>(t => t.F_Id == model.UserId);
                        db.Delete(model);

                        var isPost = true;
                        var parameters = "sysid=" + F_Id[i] + "&amp;appid=" + Configs.GetValue("appid") + "";
                        var Result = WebHelper.SendRequest(Configs.GetValue("CallUrlDel"), parameters, isPost, "application/json");
                    }
                }
                db.Commit();
            }
                
        }

        public void AddDatas(List<Teacher> datas)
        {
            using (var db = new UnitWork().BeginTrans())
            {
                foreach (var data in datas)
                    if (data.Id.IsEmpty())
                    {
                        var teach = new TeacherRepository().QueryAsNoTracking()
                                    .Where(t => t.F_Num.Equals(data.F_Num));
                        //是否被使用
                        if (teach.Count() > 0)
                        {
                            throw new Exception("教师姓名为'" + data.F_Name + "'新增失败（手机号已被使用）");
                        }
                        var user = new User
                        {
                            F_Id = Guid.NewGuid().ToString(),
                            F_Account = data.F_Num,
                            F_DutyId = "teacherDuty",
                            F_Gender = Convert.ToBoolean(Convert.ToInt32(data.F_Gender)),
                            F_RoleId = "teacher",
                            OrgId = data.F_Divis_ID,
                            //F_MobilePhone = data.,
                            F_RealName = data.F_Name,
                            F_OrganizeId = "1",
                            //分班的时候赋值
                            //u.F_DepartmentId
                            F_NickName = data.F_Name,
                            F_IsAdministrator = false,
                            F_EnabledMark = true,
                            F_HeadIcon = data.F_FacePhoto,
                            F_Birthday = data.F_Birthday,
                            F_MobilePhone = data.F_MobilePhone,
                            F_CreatorTime = DateTime.Now
                        };
                        var userLogOnEntity = new UserLogin();
                        userLogOnEntity.F_Id = user.F_Id;
                        userLogOnEntity.F_UserId = user.F_Id;
                        userLogOnEntity.F_UserSecretkey = Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                        var pwd = "000000";
                        if (!data.F_CredNum.IsEmpty() && data.F_CredNum.Length >= 6)
                            pwd = data.F_CredNum.Substring(data.F_CredNum.Length - 6, 6);
                        pwd = pwd.ToLower().Replace("x", "0"); //密码x变为0
                        userLogOnEntity.F_UserPassword = Md5EncryptHelper
                            .Encrypt(
                                DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(pwd, 32).ToLower(),
                                    userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                        db.Insert(user);
                        db.Insert(userLogOnEntity);
                        data.UserId = user.F_Id;
                        var us = new UserRole();
                        us.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                        us.F_User = data.UserId;
                        us.F_Role = user.F_RoleId;

                        var isPost = true;
                        var F_Birthday = "";
                        if (user.F_Birthday != null) F_Birthday = user.F_Birthday.ToDateTimeString();
                        var parameters = "sysid=" + user.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                         user.F_RealName + "&amp;username=" + user.F_Account + "&amp;password=" +
                                         userLogOnEntity.F_UserPassword + "&amp;sysgroupid=" + user.OrgId +
                                         "&amp;headicon=" + user.F_HeadIcon + "&amp;birthday=" + F_Birthday + "";
                        var Result = WebHelper.SendRequest(CallUrlAdd, parameters, isPost, "application/json");

                        db.Insert(us);
                        db.Insert(data);
                    }
                    else
                    {
                        var teach = new TeacherRepository().QueryAsNoTracking()
                                .Where(t => t.F_Num.Equals(data.F_Num));
                        //是否被使用
                        if (teach.Count() <= 0)
                        {
                        }
                        else
                        {
                            if (data.Id != teach.First().Id)
                            {
                                throw new Exception("教师姓名为'" + data.F_Name + "'更新失败（手机号已被使用）");
                            }
                        }
                        var user = db.FindEntity<User>(t => t.F_Id == data.UserId);
                        user.F_Gender = Convert.ToBoolean(Convert.ToInt32(data.F_Gender));
                        user.OrgId = data.F_Divis_ID;
                        user.F_RealName = data.F_Name;
                        user.F_NickName = data.F_Name;
                        user.F_HeadIcon = data.F_FacePhoto;
                        user.F_Birthday = data.F_Birthday;
                        user.F_MobilePhone = data.F_MobilePhone;
                        var userLogOnEntity = new UserLogin();
                        userLogOnEntity.F_Id = user.F_Id;
                        userLogOnEntity.F_UserId = user.F_Id;
                        userLogOnEntity.F_UserSecretkey = Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                        var pwd = "000000";
                        if (!data.F_CredNum.IsEmpty() && data.F_CredNum.Length >= 6)
                            pwd = data.F_CredNum.Substring(data.F_CredNum.Length - 6, 6);
                        pwd = pwd.ToLower().Replace("x", "0"); //密码x变为0
                        userLogOnEntity.F_UserPassword = Md5EncryptHelper
                            .Encrypt(
                                DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(pwd, 32).ToLower(),
                                    userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                        db.Update(user);
                        db.Update(userLogOnEntity);

                        var isPost = true;
                        var F_Birthday = "";
                        if (user.F_Birthday != null) F_Birthday = user.F_Birthday.ToDateTimeString();
                        var parameters = "sysid=" + user.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                         user.F_RealName + "&amp;username=" + user.F_Account + "&amp;password=" +
                                         userLogOnEntity.F_UserPassword + "&amp;sysgroupid=" + user.OrgId +
                                         "&amp;headicon=" + user.F_HeadIcon + "&amp;birthday=" + F_Birthday + "";
                        var Result = WebHelper.SendRequest(CallUrlUpt, parameters, isPost, "application/json");

                        db.Update(data);
                    }

                db.Commit();
            }
        }

        public void AddDatasImport(List<Teacher> datas)
        {
            var list = new List<string>();
            var mod = "<p>导入错误如下：</p>";
            var kk = 1;
            foreach (var data in datas)
                if (data.Id.IsEmpty())
                    try
                    {
                        using (var db = new UnitWork().BeginTrans())
                        {
                            if (string.IsNullOrEmpty(data.F_Num))
                            {
                                data.F_Num = data.F_MobilePhone;

                                var teach = new TeacherRepository().QueryAsNoTracking()
                                    .Where(t => t.F_Num.Equals(data.F_Num));
                                //是否被使用
                                if (teach.Count() > 0)
                                {
                                    list.Add("<p>" + kk + ". 教师姓名为'" + data.F_Name + "'新增失败（手机号已被使用）</p> ");
                                    kk++;
                                    continue;
                                }
                            }
                            else
                            {
                                var teach = new TeacherRepository().QueryAsNoTracking()
                                    .Where(t => t.F_Num.Equals(data.F_Num));
                                //是否被使用
                                if (teach.Count() > 0)
                                {
                                    list.Add("<p>" + kk + ". 教师姓名为'" + data.F_Name + "'新增失败（手机号已被使用）</p> ");
                                    kk++;
                                    continue;
                                }
                            }
                            var role = "";
                            if (!string.IsNullOrEmpty(data.F_RoleId))
                            {
                                db.Delete<UserRole>(t => t.F_User == data.UserId);
                                var rolelength = data.F_RoleId.Length;
                                var rolelengthlast = data.F_RoleId.LastIndexOf(',');
                                var k = 0;
                                if (rolelength - rolelengthlast == 1) k = 1;
                                var F_RoleId = data.F_RoleId.Split(',');

                                for (var i = 0; i < F_RoleId.Length - k; i++)
                                {
                                    role += GetRoleListByCache().FirstOrDefault(q =>
                                        GetPropertyValue(q.Value, "fullname").Equals(F_RoleId[i].ToString())).Key + ",";
                                }
                            }
                            var user = new User
                            {
                                F_Id = Guid.NewGuid().ToString(),
                                F_Account = data.F_Num,
                                F_DutyId = "teacherDuty",
                                F_Gender = Convert.ToBoolean(Convert.ToInt32(data.F_Gender)),
                                F_RoleId = !string.IsNullOrEmpty(role) ? role : "teacher",
                                OrgId = data.F_Divis_ID,
                                //F_MobilePhone = data.,
                                F_RealName = data.F_Name,
                                F_OrganizeId = "1",
                                //分班的时候赋值
                                //u.F_DepartmentId
                                F_NickName = data.F_Name,
                                F_IsAdministrator = false,
                                F_EnabledMark = true,
                                F_HeadIcon = data.F_FacePhoto,
                                F_Birthday = data.F_Birthday,
                                F_MobilePhone = data.F_MobilePhone,
                                F_CreatorTime = DateTime.Now
                            };
                            var userLogOnEntity = new UserLogin();
                            userLogOnEntity.F_Id = user.F_Id;
                            userLogOnEntity.F_UserId = user.F_Id;
                            userLogOnEntity.F_UserSecretkey = Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                            var pwd = "000000";
                            if (!data.F_CredNum.IsEmpty() && data.F_CredNum.Length >= 6)
                                pwd = data.F_CredNum.Substring(data.F_CredNum.Length - 6, 6);
                            pwd = pwd.ToLower().Replace("x", "0"); //密码x变为0
                            userLogOnEntity.F_UserPassword = Md5EncryptHelper
                                .Encrypt(
                                    DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(pwd, 32).ToLower(),
                                        userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                            //db.Insert(user);
                            //db.Insert(userLogOnEntity);
                            data.UserId = user.F_Id;
                            //service.AddDatasImport(user, userLogOnEntity, data);

                            db.Insert(user);
                            db.Insert(userLogOnEntity);

                            var isPost = true;
                            var F_Birthday = "";
                            if (user.F_Birthday != null) F_Birthday = user.F_Birthday.ToDateTimeString();
                            var parameters = "sysid=" + user.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                             user.F_RealName + "&amp;username=" + user.F_Account + "&amp;password=" +
                                             userLogOnEntity.F_UserPassword + "&amp;sysgroupid=" + user.OrgId +
                                             "&amp;headicon=" + user.F_HeadIcon + "&amp;birthday=" + F_Birthday + "";
                            var Result = WebHelper.SendRequest(CallUrlAdd, parameters, isPost, "application/json");

                            if (!string.IsNullOrEmpty(data.F_RoleId))
                            {
                                var rolelength = data.F_RoleId.Length;
                                var rolelengthlast = data.F_RoleId.LastIndexOf(',');
                                var k = 0;
                                if (rolelength - rolelengthlast == 1) k = 1;
                                var F_RoleId = data.F_RoleId.Split(',');
                                for (var i = 0; i < F_RoleId.Length - k; i++)
                                {
                                    var us = new UserRole();
                                    us.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                                    us.F_User = data.UserId;
                                    us.F_Role = GetRoleListByCache().FirstOrDefault(q =>
                                        GetPropertyValue(q.Value, "fullname").Equals(F_RoleId[i].ToString())).Key;
                                    db.Insert(us);
                                }
                            }
                            else
                            {
                                var us = new UserRole();
                                us.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                                us.F_User = data.UserId;
                                us.F_Role = user.F_RoleId;
                                db.Insert(us);
                            }

                            db.Insert(data);
                            db.Commit();
                        }
                    }
                    catch (Exception)
                    {
                        list.Add("<p>" + kk + ". 教师姓名为'" + data.F_Name + "'新增失败</p> ");
                        kk++;
                    }
                else
                    try
                    {
                        using (var db = new UnitWork().BeginTrans())
                        {
                            data.F_Num = data.F_MobilePhone;
                            var teachentity = db.FindEntity<Teacher>(t => t.Id == data.Id);
                            var teach = new TeacherRepository().QueryAsNoTracking()
                                .Where(t => t.F_Num.Equals(data.F_Num));
                            //是否被使用
                            if (teach.Count() <= 0)
                            {
                            }
                            else
                            {
                                if (data.Id != teach.First().Id)
                                {
                                    list.Add("<p>" + kk + ". 教师姓名为'" + data.F_Name + "'更新失败（手机号已被使用）</p> ");
                                    kk++;
                                    continue;
                                }
                            }
                            var role = "";
                            if (!string.IsNullOrEmpty(data.F_RoleId))
                            {
                                db.Delete<UserRole>(t => t.F_User == data.UserId);
                                var rolelength = data.F_RoleId.Length;
                                var rolelengthlast = data.F_RoleId.LastIndexOf(',');
                                var k = 0;
                                if (rolelength - rolelengthlast == 1) k = 1;
                                var F_RoleId = data.F_RoleId.Split(',');

                                for (var i = 0; i < F_RoleId.Length - k; i++)
                                {
                                    role += GetRoleListByCache().FirstOrDefault(q =>
                                        GetPropertyValue(q.Value, "fullname").Equals(F_RoleId[i].ToString())).Key + ",";
                                }
                            }

                            var user = db.FindEntity<User>(t => t.F_Id == teachentity.UserId);
                            user.F_Account = data.F_Num;
                            user.F_Gender = Convert.ToBoolean(Convert.ToInt32(data.F_Gender));
                            user.OrgId = data.F_Divis_ID;
                            //F_MobilePhone = data.,
                            user.F_RoleId = !string.IsNullOrEmpty(role) ? role : user.F_RoleId;
                            user.F_RealName = data.F_Name;
                            user.F_NickName = data.F_Name;
                            user.F_HeadIcon = data.F_FacePhoto;
                            user.F_Birthday = data.F_Birthday;
                            user.F_MobilePhone = data.F_MobilePhone;
                            var userLogOnEntity = new UserLogin();
                            userLogOnEntity.F_Id = user.F_Id;
                            userLogOnEntity.F_UserId = user.F_Id;
                            userLogOnEntity.F_UserSecretkey = Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                            var pwd = "000000";
                            if (!data.F_CredNum.IsEmpty() && data.F_CredNum.Length >= 6)
                                pwd = data.F_CredNum.Substring(data.F_CredNum.Length - 6, 6);
                            pwd = pwd.ToLower().Replace("x", "0"); //密码x变为0
                            userLogOnEntity.F_UserPassword = Md5EncryptHelper
                                .Encrypt(
                                    DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(pwd, 32).ToLower(),
                                        userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                            //db.Insert(user);
                            //db.Insert(userLogOnEntity);
                            data.UserId = user.F_Id;
                            //service.AddDatasImport(user, userLogOnEntity, data);
                            db.Update(user);
                            db.Update(userLogOnEntity);

                            var isPost = true;
                            var F_Birthday = "";
                            if (user.F_Birthday != null) F_Birthday = user.F_Birthday.ToDateTimeString();
                            var parameters = "sysid=" + user.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                             user.F_RealName + "&amp;username=" + user.F_Account + "&amp;password=" +
                                             userLogOnEntity.F_UserPassword + "&amp;sysgroupid=" + user.OrgId +
                                             "&amp;headicon=" + user.F_HeadIcon + "&amp;birthday=" + F_Birthday + "";
                            var Result = WebHelper.SendRequest(CallUrlUpt, parameters, isPost, "application/json");

                            if (!string.IsNullOrEmpty(data.F_RoleId))
                            {
                                db.Delete<UserRole>(t => t.F_User == data.UserId);
                                var rolelength = data.F_RoleId.Length;
                                var rolelengthlast = data.F_RoleId.LastIndexOf(',');
                                var k = 0;
                                if (rolelength - rolelengthlast == 1) k = 1;
                                var F_RoleId = data.F_RoleId.Split(',');
                                for (var i = 0; i < F_RoleId.Length - k; i++)
                                {
                                    var us = new UserRole();
                                    us.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                                    us.F_User = data.UserId;
                                    us.F_Role = GetRoleListByCache().FirstOrDefault(q =>
                                        GetPropertyValue(q.Value, "fullname").Equals(F_RoleId[i].ToString())).Key;
                                    db.Insert(us);
                                }
                            }

                            db.Update(data);
                            db.Commit();
                        }
                    }
                    catch
                    {
                        list.Add("<p>" + kk + ". 主键为'" + data.Id + "'更新失败 </p>");
                        kk++;
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
        }

        public void AddDatasImport(User userentity, UserLogin userlogentity, Teacher entity)
        {
            using (var db = new UnitWork().BeginTrans())
            {
                if (string.IsNullOrEmpty(entity.Id))
                {
                    db.Insert(userentity);
                    db.Insert(userlogentity);

                    var isPost = true;
                    var F_Birthday = "";
                    if (userentity.F_Birthday != null) F_Birthday = userentity.F_Birthday.ToDateTimeString();
                    var parameters = "sysid=" + userentity.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                     userentity.F_RealName + "&amp;username=" + userentity.F_Account +
                                     "&amp;password=" + userlogentity.F_UserPassword + "&amp;sysgroupid=" +
                                     userentity.OrgId + "&amp;headicon=" + userentity.F_HeadIcon +
                                     "&amp;birthday=" + F_Birthday + "";
                    var Result = WebHelper.SendRequest(CallUrlAdd, parameters, isPost, "application/json");

                    if (!string.IsNullOrEmpty(entity.F_RoleId))
                    {
                        var rolelength = entity.F_RoleId.Length;
                        var rolelengthlast = entity.F_RoleId.LastIndexOf(',');
                        var k = 0;
                        if (rolelength - rolelengthlast == 1) k = 1;
                        var F_RoleId = entity.F_RoleId.Split(',');
                        for (var i = 0; i < F_RoleId.Length - k; i++)
                        {
                            var us = new UserRole();
                            us.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                            us.F_User = entity.UserId;
                            us.F_Role = GetRoleListByCache().FirstOrDefault(q =>
                                GetPropertyValue(q.Value, "fullname").Equals(F_RoleId[i].ToString())).Key;
                            db.Insert(us);
                        }
                    }

                    db.Insert(entity);
                }
                else
                {
                    db.Update(userentity);
                    db.Update(userlogentity);

                    var isPost = true;
                    var F_Birthday = "";
                    if (userentity.F_Birthday != null) F_Birthday = userentity.F_Birthday.ToDateTimeString();
                    var parameters = "sysid=" + userentity.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                     userentity.F_RealName + "&amp;username=" + userentity.F_Account +
                                     "&amp;password=" + userlogentity.F_UserPassword + "&amp;sysgroupid=" +
                                     userentity.OrgId + "&amp;headicon=" + userentity.F_HeadIcon +
                                     "&amp;birthday=" + F_Birthday + "";
                    var Result = WebHelper.SendRequest(CallUrlUpt, parameters, isPost, "application/json");

                    if (!string.IsNullOrEmpty(entity.F_RoleId))
                    {
                        var rolelength = entity.F_RoleId.Length;
                        var rolelengthlast = entity.F_RoleId.LastIndexOf(',');
                        var k = 0;
                        if (rolelength - rolelengthlast == 1) k = 1;
                        var F_RoleId = entity.F_RoleId.Split(',');
                        for (var i = 0; i < F_RoleId.Length - k; i++)
                        {
                            var us = new UserRole();
                            us.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                            us.F_User = entity.UserId;
                            us.F_Role = GetRoleListByCache().FirstOrDefault(q =>
                                GetPropertyValue(q.Value, "fullname").Equals(F_RoleId[i].ToString())).Key;
                            db.Delete<UserRole>(t => t.F_User == entity.UserId);
                            db.Insert(us);
                        }
                    }

                    db.Update(entity);
                }

                db.Commit();
            }
        }

        [Serializable]
        private class fieldItem
        {
            public string encode { get; set; }
            public string fullname { get; set; }
            public string parentid { get; set; }
            public int? level { get; set; }
        }
    }
}