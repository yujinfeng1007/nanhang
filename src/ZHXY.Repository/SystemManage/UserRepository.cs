using System;
using System.Collections.Generic;
using System.Linq;
using ZHXY.Common;
using ZHXY.Domain;
using ZHXY.Domain.Entity;

namespace ZHXY.Repository
{
    public class UserRepository : Data.Repository<SysUser>, IUserRepository
    {
        public UserRepository(string schoolCode) : base(schoolCode)
        {
        }

        public UserRepository()
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
            var expression = ExtLinq.True<SysRole>();
            expression = expression.And(t => t.F_Category == 1);
            var data = role.QueryAsNoTracking(expression).OrderBy(t => t.F_SortCode).ToList();
            var dictionary = new Dictionary<string, object>();
            foreach (var item in data)
            {
                var fieldItem = new fieldItem();
                fieldItem.encode = item.F_EnCode;
                fieldItem.fullname = item.F_FullName;
                //{
                //    encode = item.F_EnCode,
                //    fullname = item.F_FullName
                //};
                //var fieldItem = new
                //{
                //    encode = item.F_EnCode,
                //    fullname = item.F_FullName
                //};
                dictionary.Add(item.F_Id, fieldItem);
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

      

        public void SubmitForm(SysUser userEntity, SysUserLogin userLogOnEntity, string keyValue, List<SysUserRole> userRoles)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                //var db = new RepositoryBase());

                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(userEntity);

                    var F_UserPassword = db.FindEntity<SysUserLogin>(t => t.F_Id == keyValue).F_UserPassword;
                    var isPost = true;
                    var F_Birthday = "";
                    if (userEntity.F_Birthday != null) F_Birthday = userEntity.F_Birthday.ToDateTimeString();
                    var parameters = "sysid=" + userEntity.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                     userEntity.F_RealName + "&amp;username=" + userEntity.F_Account +
                                     "&amp;password=" + F_UserPassword + "&amp;sysgroupid=" +
                                     userEntity.F_DepartmentId + "&amp;headicon=" + userEntity.F_HeadIcon +
                                     "&amp;birthday=" + F_Birthday + "";
                    var Result = WebHelper.SendRequest(CallUrlUpt, parameters, isPost, "application/json");
                }
                else
                {
                    userLogOnEntity.F_Id = userEntity.F_Id;
                    userLogOnEntity.F_UserId = userEntity.F_Id;
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
                    if (userEntity.F_Birthday != null) F_Birthday = userEntity.F_Birthday.ToDateTimeString();
                    var parameters = "sysid=" + userEntity.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                     userEntity.F_RealName + "&amp;username=" + userEntity.F_Account +
                                     "&amp;password=" + userLogOnEntity.F_UserPassword + "&amp;sysgroupid=" +
                                     userEntity.F_DepartmentId + "&amp;headicon=" + userEntity.F_HeadIcon +
                                     "&amp;birthday=" + F_Birthday + "";
                    var Result = WebHelper.SendRequest(CallUrlAdd, parameters, isPost, "application/json");
                }

                db.Delete<SysUserRole>(t => t.F_User == userEntity.F_Id);
                db.BatchInsert(userRoles);
                db.Commit();
            }
        }

        public void AddDatas(List<SysUser> data)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                var list = new List<string>();
                var mod = "<p>导入错误如下：</p>";
                var kk = 1;
                foreach (var user in data)
                    if (user.F_Id.IsEmpty())
                        try
                        {
                            user.Create();
                            if (!string.IsNullOrEmpty(user.F_RoleId))
                            {
                                var role = "";
                                var rolelength = user.F_RoleId.Length;
                                var rolelengthlast = user.F_RoleId.LastIndexOf(',');
                                var k = 0;
                                if (rolelength - rolelengthlast == 1) k = 1;
                                var F_RoleId = user.F_RoleId.Split(',');
                                for (var i = 0; i < F_RoleId.Length - k; i++)
                                {
                                    var us = new SysUserRole();
                                    us.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                                    us.F_User = user.F_Id;
                                    us.F_Role = GetRoleListByCache().FirstOrDefault(q =>
                                        GetPropertyValue(q.Value, "fullname").Equals(F_RoleId[i].ToString())).Key;
                                    db.Insert(us);
                                    role += us.F_Role + ",";
                                }
                                user.F_RoleId = role;
                            }

                            var userLogOnEntity = new SysUserLogin();
                            userLogOnEntity.F_Id = user.F_Id;
                            userLogOnEntity.F_UserId = user.F_Id;
                            userLogOnEntity.F_UserSecretkey = Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
                            userLogOnEntity.F_UserPassword = Md5EncryptHelper
                                .Encrypt(
                                    DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt("123456", 32).ToLower(),
                                        userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
                            db.Insert(userLogOnEntity);

                            user.F_EnabledMark = true;

                            var isPost = true;
                            var F_Birthday = "";
                            if (user.F_Birthday != null) F_Birthday = user.F_Birthday.ToDateTimeString();
                            var parameters = "sysid=" + user.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                             user.F_RealName + "&amp;username=" + user.F_Account + "&amp;password=" +
                                             userLogOnEntity.F_UserPassword + "&amp;sysgroupid=" + user.F_DepartmentId +
                                             "&amp;headicon=" + user.F_HeadIcon + "&amp;birthday=" + F_Birthday + "";
                            var Result = WebHelper.SendRequest(CallUrlAdd, parameters, isPost, "application/json");
                            db.Insert(user);
                        }
                        catch (Exception)
                        {
                            list.Add("<p>" + kk + ". 标题为'" + user.F_Account + "'新增失败</p> ");
                            kk++;
                        }
                    else
                        try
                        {
                            if (!string.IsNullOrEmpty(user.F_RoleId))
                            {
                                var role = "";
                                var rolelength = user.F_RoleId.Length;
                                var rolelengthlast = user.F_RoleId.LastIndexOf(',');
                                var k = 0;
                                if (rolelength - rolelengthlast == 1) k = 1;
                                var F_RoleId = user.F_RoleId.Split(',');
                                for (var i = 0; i < F_RoleId.Length - k; i++)
                                {
                                    var us = new SysUserRole();
                                    us.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                                    us.F_User = user.F_Id;
                                    us.F_Role = GetRoleListByCache().FirstOrDefault(q =>
                                        GetPropertyValue(q.Value, "fullname").Equals(F_RoleId[i].ToString())).Key;
                                    db.Delete<SysUserRole>(t => t.F_User == user.F_Id);
                                    db.Insert(us);
                                    role += us.F_Role + ",";
                                }
                                user.F_RoleId = role;
                            }

                            user.Modify(user.F_Id);
                            var F_UserPassword = db.FindEntity<SysUserLogin>(t => t.F_Id == user.F_Id).F_UserPassword;
                            var isPost = true;
                            var F_Birthday = "";
                            if (user.F_Birthday != null) F_Birthday = user.F_Birthday.ToDateTimeString();
                            var parameters = "sysid=" + user.F_Id + "&amp;appid=" + appid + "&amp;nickname=" +
                                             user.F_RealName + "&amp;username=" + user.F_Account + "&amp;password=" +
                                             F_UserPassword + "&amp;sysgroupid=" + user.F_DepartmentId +
                                             "&amp;headicon=" + user.F_HeadIcon + "&amp;birthday=" + F_Birthday + "";
                            var Result = WebHelper.SendRequest(CallUrlUpt, parameters, isPost, "application/json");
                            db.Update(user);
                        }
                        catch (Exception)
                        {
                            list.Add("<p>" + kk + ". 主键为'" + user.F_Id + "'更新失败 </p>");
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