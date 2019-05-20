using ZHXY.Domain;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using ZHXY.Common;

namespace ZHXY.Application
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class UserService : AppService
    {
        public UserService(IZhxyRepository r) : base(r) { }

        public dynamic GetList(Pagination pag, string orgId, string keyword)
        {
            if (string.IsNullOrWhiteSpace(orgId)) return null;
            var query = Read<User>(p => p.OrganId.Equals(orgId));
            query = string.IsNullOrWhiteSpace(keyword) ? query : query.Where(p => p.Name.Contains(keyword));
            return query.Paging(pag).ToListAsync().Result;

        }

        public dynamic GetById(string id) => Read<User>(p => p.Id.Equals(id)).FirstOrDefaultAsync().Result;

        public void Update(UpdateUserDto dto)
        {
            var user = Get<User>(dto.Id);
            dto.MapTo(user);
            SaveChanges();
        }

        public void Add(AddUserDto dto)
        {
            var user = dto.MapTo<User>();
            Add(user);
            var userLogin = new UserLogin
            {
                UserId=user.Id,
                Password = "17f123f731bca5b8925979dc1a228548",
                Secretkey = "4a7d1ed414474e40"
            };
            Add(userLogin);
            SaveChanges();
        }

        public void Delete(string[] id)
        {
            var users=Query<User>(p => id.Contains(p.Id)).ToList();
            DelAndSave<User>(users);
        }

        public void UpdIco(string userId,string filepath)
        {
            var user = Get<User>(userId);
            user.HeadIcon = filepath;
            SaveChanges();
        }

        public CurrentUser CheckLogin(string username, string password)
        {
            var user = Read<User>(p => p.Account.Equals(username)).FirstOrDefaultAsync().Result;
            if (user == null) throw new Exception("账户不存在，请重新输入");
            var loginInfo = Get<UserLogin>(user.Id);
            var dbPassword = Md5EncryptHelper.Encrypt(DESEncryptHelper.Encrypt(password.ToLower(), loginInfo.Secretkey).ToLower(), 32).ToLower();
            if (dbPassword != loginInfo.Password) throw new Exception("密码不正确，请重新输入");
            loginInfo.PreVisitTime = loginInfo.LastVisitTime.HasValue ? loginInfo.LastVisitTime : null;
            loginInfo.LastVisitTime = DateTime.Now;
            loginInfo.LoginCount = Convert.ToInt32(loginInfo.LoginCount) + 1;
            SaveChanges();
            var cuser = user.MapTo<CurrentUser>();
            cuser.LoginToken = DESEncryptHelper.Encrypt(Guid.NewGuid().ToString());
            cuser.Ip = Net.Ip;
            cuser.IpLocation = Net.GetLocation(cuser.Ip);
            cuser.Roles = GetUserRolesId(user.Id);
            return cuser;
        }

        public void Enable(string id) => throw new NotImplementedException();
        public void Disable(string id) => throw new NotImplementedException();

        public bool VerifyPwd(string userid, string password)
        {
            var userLogin = Read<UserLogin>(p => p.UserId.Equals(userid)).Select(p => new { SecretKey = p.Secretkey, Password = p.Password }).FirstOrDefaultAsync().Result;
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


        public void SetRole(string userId, string[] roleId)
        {
            var roles = Query<Relevance>(p => p.Name.Equals(Relation.UserRole) && p.FirstKey.Equals(userId)).ToArrayAsync().Result;
            Del<Relevance>(roles);
            foreach (var item in roleId)
            {
                Add(new Relevance { Name = Relation.UserRole, FirstKey = userId, SecondKey = item });
            }
            SaveChanges();
        }

        public void AddRole(string userId,string[] roleIds)
        {
            var existingRoles = GetUserRolesId(userId);
            var addRoles = roleIds.Except(existingRoles);
            foreach (var item in addRoles)
            {
                Add(new Relevance { Name = Relation.UserRole, FirstKey = userId, SecondKey = item });
            }
            SaveChanges();
        }

        public void RemoveRole(string userId,string[] roleIds)
        {
            var removeList = Query<Relevance>(p =>
             p.Name.Equals(Relation.UserRole) &&
             p.FirstKey.Equals(userId) &&
             roleIds.Contains(p.SecondKey)
             ).ToArrayAsync().Result;
            DelAndSave<Relevance>(removeList);
        }

        public dynamic GetRolesExcludeUser(string userId)
        {
            var exclude=GetUserRolesId(userId);
           return Read<Role>(p => !exclude.Contains(p.Id)).ToListAsync().Result;
        }

        public void RevisePassword(string userPassword, string userId)
        {
            var user = Query<UserLogin>(p => p.UserId.Equals(userId)).FirstOrDefaultAsync().Result;
            user.Secretkey = Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
            user.Password = Md5EncryptHelper.Encrypt(DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(userPassword, 32).ToLower(), user.Secretkey).ToLower(), 32).ToLower();
            SaveChanges();
        }

        public UserData GetUserData(CurrentUser user)
        {
            var d = new UserData
            {
                UserId = user.Id,
                Organ = user.OrganId,
                Duty = user.DutyId,
                Roles = user.Roles
            };
            var query = Read<Menu>(p => p.BelongSys.Equals("1"));
            if (user.Name=="超级管理员")
            {
                var list = query.ToListAsync().Result;
                d.Menus = TreeHelper.GetMenuJson(list, SYS_CONSTS.DbNull);
                d.Funcs = Read<Function>().ToListAsync().Result.ToCamelJson();
            }
            else
            {
                d.Menus = GetUserMenuJson(user.Id,"1");
                d.Funcs = GetUserFuncJson(user.Id);
            }
            
           
            var o = Read<User>(p => p.Id.Equals(user.Id)).FirstOrDefaultAsync().Result;
            d.UserName = o.Name;
            d.HeadIcon = o.HeadIcon;
            return d;
        }

        public dynamic GetUserRoles(string userId)
        {
            var roles = Read<Relevance>(p => p.Name.Equals(Relation.UserRole) && p.FirstKey.Equals(userId)).Select(p => p.SecondKey).ToArrayAsync().Result;
            return Read<Role>(p => roles.Contains(p.Id)).ToListAsync().Result;
        }
        public string[] GetUserRolesId(string userId)
        {
            return Read<Relevance>(p => p.Name.Equals(Relation.UserRole) && p.FirstKey.Equals(userId)).Select(p => p.SecondKey).ToArrayAsync().Result;
        }




        /// <summary>
        /// 用户鉴权
        /// 鉴定用户是否具有某项功能权限
        /// </summary>
        /// <returns></returns>
        public bool Authentication(string userId,string funcId)
        {
            var userRoles=Read<Relevance>(p => p.Name.Equals(Relation.UserRole) && p.FirstKey.Equals(userId)).Select(p => p.SecondKey).ToListAsync().Result;
            var userPowers = Read<Relevance>(p => p.Name.Equals(Relation.RolePower) && userRoles.Contains(p.FirstKey)).Select(p => p.ThirdKey).Distinct().ToArrayAsync().Result;
            return userPowers.Contains(funcId);
        }


        public string GetUserMenuJson(string userId,string system)
        {
            var userRoles = GetUserRolesId(userId);
            var userMenus = Read<Relevance>(p => p.Name.Equals(Relation.RolePower) && userRoles.Contains(p.FirstKey) && p.ThirdKey.Equals(SYS_CONSTS.DbNull)).Select(p => p.SecondKey).ToArrayAsync().Result;
            var list = Read<Menu>(p => userMenus.Contains(p.Id) && p.BelongSys.Equals(system)).ToListAsync().Result;
            return TreeHelper.GetMenuJson(list, SYS_CONSTS.DbNull);
        }

        public string GetUserFuncJson(string userId)
        {
            var userRoles = GetUserRolesId(userId);
            var userFuncs = Read<Relevance>(p => p.Name.Equals(Relation.RolePower) && userRoles.Contains(p.FirstKey) && !p.ThirdKey.Equals(SYS_CONSTS.DbNull)).Select(p => p.ThirdKey).Distinct().ToArrayAsync().Result;
            var list = Read<Function>(p => userFuncs.Contains(p.Id)).ToListAsync().Result;
            return list.ToCamelJson();
        }
    }
}