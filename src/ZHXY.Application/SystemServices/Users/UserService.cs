using ZHXY.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using ZHXY.Common;

namespace ZHXY.Application
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class UserService : AppService
    {
        public UserService(IZhxyRepository r) : base(r) { }

        public dynamic GetList(Pagination pag, string orgId,string keyword)
        {
            if (string.IsNullOrWhiteSpace(orgId)) return null;
            var query = Read<User>(p=>p.OrganId.Equals(orgId));
            query = string.IsNullOrWhiteSpace(keyword) ? query : query.Where(p => p.Name.Contains(keyword));
            return query.Paging(pag).ToListAsync().Result;

        }

        public dynamic Get(string id) => Read<User>(p => p.Id.Equals(id)).FirstOrDefaultAsync().Result;

        public void Update(User userEntity)
        {

        }

        public void UpdIco(string userId,string filepath)
        {
            var user = Get<User>(userId);
            user.HeadIcon = filepath;
            SaveChanges();
        }

        public User CheckLogin(string username, string password)
        {
            var user = Read<User>(p => p.Account.Equals(username)).FirstOrDefaultAsync().Result;
            if (user == null) throw new Exception("账户不存在，请重新输入");
            if (user.EnabledMark != true) throw new Exception("账户被系统锁定,请联系管理员");
            var userLogin = Get<UserLogin>(user.Id);

            var dbPassword = Md5EncryptHelper.Encrypt(DESEncryptHelper.Encrypt(password.ToLower(), userLogin.F_UserSecretkey).ToLower(), 32).ToLower();
            if (dbPassword != userLogin.F_UserPassword) throw new Exception("密码不正确，请重新输入");
            userLogin.F_PreviousVisitTime = userLogin.F_LastVisitTime.HasValue ? userLogin.F_LastVisitTime : null;
            userLogin.F_LastVisitTime = DateTime.Now;
            userLogin.F_LogOnCount = Convert.ToInt32(userLogin.F_LogOnCount) + 1;
            SaveChanges();
            return user;
        }

        public bool VerifyPwd(string userid, string password)
        {
            var userLogin = Read<UserLogin>(p => p.F_Id.Equals(userid)).Select(p => new { SecretKey = p.F_UserSecretkey, Password = p.F_UserPassword }).FirstOrDefaultAsync().Result;
            var dbPassword = Md5EncryptHelper.Encrypt(DESEncryptHelper.Encrypt(password.ToLower(), userLogin.SecretKey).ToLower(), 32).ToLower();
            return dbPassword == userLogin.Password;
        }

        public dynamic GetByOrg(string orgId, string keyword = null)
        {
            if (string.IsNullOrEmpty(orgId)) return null;
            var query = Read<User>(p => p.OrganId.Equals(orgId));
            query = string.IsNullOrWhiteSpace(keyword) ? query : query.Where(p => p.Name.Contains(keyword));
            return query.Select(p => new
            {
                id = p.Id,
                name = p.Name
            }).ToListAsync().Result;
        }
    }
}