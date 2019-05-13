using System;
using System.Collections.Generic;
using System.Linq;
using ZHXY.Common;

namespace ZHXY.Domain
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository( ) : base()
        {
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

      

        public void SubmitForm(User userEntity, UserLogin userLogOnEntity, string keyValue, List<UserRole> userRoles)
        {
            using (var db = new UnitWork().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(userEntity);

                    var F_UserPassword = db.FindEntity<UserLogin>(t => t.F_Id == keyValue).F_UserPassword;
                    var isPost = true;
                    var F_Birthday = "";
                    if (userEntity.Birthday != null) F_Birthday = userEntity.Birthday.ToDateTimeString();
                    var parameters = "sysid=" + userEntity.Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                     userEntity.Name + "&amp;username=" + userEntity.Account +
                                     "&amp;password=" + F_UserPassword + "&amp;sysgroupid=" +
                                     userEntity.OrganId + "&amp;headicon=" + userEntity.HeadIcon +
                                     "&amp;birthday=" + F_Birthday + "";
                    var Result = WebHelper.SendRequest(CallUrlUpt, parameters, isPost, "application/json");
                }
                else
                {
                    userLogOnEntity.F_Id = userEntity.Id;
                    userLogOnEntity.F_UserId = userEntity.Id;
                    userLogOnEntity.F_UserSecretkey = Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                    userLogOnEntity.F_UserPassword = Md5EncryptHelper
                        .Encrypt(
                            DESEncryptHelper
                                .Encrypt(Md5EncryptHelper.Encrypt(userLogOnEntity.F_UserPassword, 32).ToLower(),
                                    userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                    db.Insert(userEntity);
                    db.Insert(userLogOnEntity);

                    var isPost = true;
                    var F_Birthday = "";
                    if (userEntity.Birthday != null) F_Birthday = userEntity.Birthday.ToDateTimeString();
                    var parameters = "sysid=" + userEntity.Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                     userEntity.Name + "&amp;username=" + userEntity.Account +
                                     "&amp;password=" + userLogOnEntity.F_UserPassword + "&amp;sysgroupid=" +
                                     userEntity.OrganId + "&amp;headicon=" + userEntity.HeadIcon +
                                     "&amp;birthday=" + F_Birthday + "";
                    var Result = WebHelper.SendRequest(CallUrlAdd, parameters, isPost, "application/json");
                }

                db.Delete<UserRole>(t => t.F_User == userEntity.Id);
                db.BatchInsert(userRoles);
                db.Commit();
            }
        }

        public void AddDatas(List<User> data)
        {
            using (var db = new UnitWork().BeginTrans())
            {
                var list = new List<string>();
                var mod = "<p>导入错误如下：</p>";
                var kk = 1;
                foreach (var user in data)
                    if (user.Id.IsEmpty())
                        try
                        {
                            if (!string.IsNullOrEmpty(user.RoleId))
                            {
                                var role = "";
                                var rolelength = user.RoleId.Length;
                                var rolelengthlast = user.RoleId.LastIndexOf(',');
                                var k = 0;
                                if (rolelength - rolelengthlast == 1) k = 1;
                                var F_RoleId = user.RoleId.Split(',');
                                for (var i = 0; i < F_RoleId.Length - k; i++)
                                {
                                    var us = new UserRole();
                                    us.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                                    us.F_User = user.Id;
                                    us.F_Role = GetRoleListByCache().FirstOrDefault(q =>
                                        GetPropertyValue(q.Value, "fullname").Equals(F_RoleId[i].ToString())).Key;
                                    db.Insert(us);
                                    role += us.F_Role + ",";
                                }
                                user.RoleId = role;
                            }

                            var userLogOnEntity = new UserLogin();
                            userLogOnEntity.F_Id = user.Id;
                            userLogOnEntity.F_UserId = user.Id;
                            userLogOnEntity.F_UserSecretkey = Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                            userLogOnEntity.F_UserPassword = Md5EncryptHelper
                                .Encrypt(
                                    DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt("123456", 32).ToLower(),
                                        userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                            db.Insert(userLogOnEntity);

                            user.EnabledMark = true;

                            var isPost = true;
                            var F_Birthday = "";
                            if (user.Birthday != null) F_Birthday = user.Birthday.ToDateTimeString();
                            var parameters = "sysid=" + user.Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                             user.Name + "&amp;username=" + user.Account + "&amp;password=" +
                                             userLogOnEntity.F_UserPassword + "&amp;sysgroupid=" + user.OrganId +
                                             "&amp;headicon=" + user.HeadIcon + "&amp;birthday=" + F_Birthday + "";
                            var Result = WebHelper.SendRequest(CallUrlAdd, parameters, isPost, "application/json");
                            db.Insert(user);
                        }
                        catch (Exception)
                        {
                            list.Add("<p>" + kk + ". 标题为'" + user.Account + "'新增失败</p> ");
                            kk++;
                        }
                    else
                        try
                        {
                            if (!string.IsNullOrEmpty(user.RoleId))
                            {
                                var role = "";
                                var rolelength = user.RoleId.Length;
                                var rolelengthlast = user.RoleId.LastIndexOf(',');
                                var k = 0;
                                if (rolelength - rolelengthlast == 1) k = 1;
                                var F_RoleId = user.RoleId.Split(',');
                                for (var i = 0; i < F_RoleId.Length - k; i++)
                                {
                                    var us = new UserRole();
                                    us.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                                    us.F_User = user.Id;
                                    us.F_Role = GetRoleListByCache().FirstOrDefault(q =>
                                        GetPropertyValue(q.Value, "fullname").Equals(F_RoleId[i].ToString())).Key;
                                    db.Delete<UserRole>(t => t.F_User == user.Id);
                                    db.Insert(us);
                                    role += us.F_Role + ",";
                                }
                                user.RoleId = role;
                            }

                            var F_UserPassword = db.FindEntity<UserLogin>(t => t.F_Id == user.Id).F_UserPassword;
                            var isPost = true;
                            var F_Birthday = "";
                            if (user.Birthday != null) F_Birthday = user.Birthday.ToDateTimeString();
                            var parameters = "sysid=" + user.Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                             user.Name + "&amp;username=" + user.Account + "&amp;password=" +
                                             F_UserPassword + "&amp;sysgroupid=" + user.OrganId +
                                             "&amp;headicon=" + user.HeadIcon + "&amp;birthday=" + F_Birthday + "";
                            var Result = WebHelper.SendRequest(CallUrlUpt, parameters, isPost, "application/json");
                            db.Update(user);
                        }
                        catch (Exception)
                        {
                            list.Add("<p>" + kk + ". 主键为'" + user.Id + "'更新失败 </p>");
                            kk++;
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