using ZHXY.Domain;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using ZHXY.Common;
using System.Collections.Generic;
using ZHXY.Web.Shared;

namespace ZHXY.Application
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class UserService : AppService
    {
        private SysUserRoleAppService userRoleAppService { get; }
        public UserService(DbContext r) : base(r) { }
        public UserService(DbContext r, SysUserRoleAppService userRoleService) : base(r)
        {
            userRoleAppService = userRoleService;
        }

        public List<UserView> GetList(Pagination pag, string orgId, string keyword)
        {
            if (string.IsNullOrWhiteSpace(orgId)) return null;
            var query = Read<User>(p => p.OrganId.Equals(orgId));
            query = string.IsNullOrWhiteSpace(keyword) ? query : query.Where(p => p.Name.Contains(keyword));
            return query.Paging(pag).ToListAsync().Result.MapToList<UserView>();
        }

        public List<User> GetList(Pagination pagination, string keyword, string orgId, string dutyId)
        {
            var query = Read<User>(p => p.Account != "admin");
            query = string.IsNullOrWhiteSpace(keyword) ? query : query.Where(p => p.Account.Contains(keyword) || p.Name.Contains(keyword) || p.MobilePhone.Contains(keyword));
            query = string.IsNullOrWhiteSpace(dutyId) ? query : query.Where(p => p.DutyId.Equals(dutyId));

            if (!string.IsNullOrEmpty(orgId))
            {
                query = query.Where(p => p.OrganId.Equals(orgId));
            }

            pagination.Records = query.CountAsync().Result;
            return query.Paging(pagination).ToListAsync().Result;
        }

        public User GetById(string id) => Read<User>(p => p.Id.Equals(id)).FirstOrDefaultAsync().Result;

        public void Update(UpdateUserDto dto)
        {
            var user = Get<User>(dto.Id);
            dto.MapTo(user);
            SaveChanges();
        }

        public void Add(AddUserDto dto)
        {
            var user = dto.MapTo<User>();
            AddAndSave(user);
            SaveChanges();
        }

        public void Submit(User userEntity, string F_RoleId, string keyValue)
        {
            if (!string.IsNullOrEmpty(userEntity.Account))
            {
                var u = Read<User>().FirstOrDefault(t => t.Account == userEntity.Account);
                if (!u.IsEmpty())
                {
                    if (string.IsNullOrEmpty(keyValue) || (!keyValue.Equals(u.Id)))
                    {
                        throw new Exception("该用户已存在");
                    }
                }
            }

            if (!string.IsNullOrEmpty(keyValue))
            {
                var user = Get<User>(keyValue);
                userEntity.MapTo(user);
                user.Id = keyValue;
            }
            else
            {
                var user = userEntity.MapTo<User>();
                AddAndSave(user);
            }

            var userRoles = new List<SysUserRole>();
            if (!string.IsNullOrEmpty(F_RoleId))
            {
                var roles = F_RoleId.Split(',');
                userRoles.AddRange(from r in roles where !r.IsEmpty() select new SysUserRole { F_Id = Guid.NewGuid().ToString("N").ToUpper(), F_Role = r, F_User = keyValue });
            }
            Del<SysUserRole>(t => t.F_User == keyValue);
            AddRange<SysUserRole>(userRoles);

            SaveChanges();
        }
        public void Delete(string[] id)
        {
            var users = Query<User>(p => id.Contains(p.Id)).ToList();
            DelAndSave<User>(users);
        }

        public void UpdIco(string userId, string filepath)
        {
            var user = Get<User>(userId);
            user.HeadIcon = filepath;
            SaveChanges();
        }

        public CurrentUser CheckLogin(string username, string password)
        {
            var user = Query<User>(p => p.Account.Equals(username)).FirstOrDefaultAsync()?.Result;
            if (user == null) throw new Exception("账户不存在，请重新输入");
            var dbPassword = Md5EncryptHelper.Encrypt(DESEncryptHelper.Encrypt(password.ToLower(), user.Secretkey).ToLower(), 32).ToLower();
            if (dbPassword != user.Password) throw new Exception("密码不正确，请重新输入");
            user.PreVisitTime = user.LastVisitTime.HasValue ? user.LastVisitTime : null;
            user.LastVisitTime = DateTime.Now;
            user.LoginCount = Convert.ToInt32(user.LoginCount) + 1;
            SaveChanges();
            var cuser = user.MapTo<CurrentUser>();
            cuser.LoginToken = DESEncryptHelper.Encrypt(Guid.NewGuid().ToString());
            cuser.Ip = Net.Ip;
            cuser.IpLocation = Net.GetLocation(cuser.Ip);
            cuser.Roles = userRoleAppService.GetListByUserId(user.Id).Select(t => t.F_Role).ToArray();
            if (cuser.Account == "admin")
            {
                cuser.DutyId = "admin";
                cuser.IsSystem = true;
            }
            return cuser;
        }

        public bool VerifyPwd(string userId, string password)
        {
            var userLogin = Read<User>(p => p.Id.Equals(userId)).Select(p => new { p.Secretkey, p.Password }).FirstOrDefaultAsync().Result;
            var dbPassword = Md5EncryptHelper.Encrypt(DESEncryptHelper.Encrypt(password.ToLower(), userLogin.Secretkey).ToLower(), 32).ToLower();
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

        public void RevisePassword(string userPassword, string userId)
        {
            var user = Query<User>(p => p.Id.Equals(userId)).FirstOrDefaultAsync().Result;
            user.Secretkey = Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
            user.Password = Md5EncryptHelper.Encrypt(DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(userPassword, 32).ToLower(), user.Secretkey).ToLower(), 32).ToLower();
            SaveChanges();
        }
    }
}