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
            AddAndSave(user);
        }

        public void Delete(string[] id)
        {
            var users=Query<User>(p => id.Contains(p.Id)).ToList();
            DelAndSave<User>(users);
        }

        public User CheckLogin(string username, string password)
        {
            var user = Read<User>(p => p.Account.Equals(username)).FirstOrDefaultAsync().Result;
            if (user == null) throw new Exception("账户不存在，请重新输入");
            var userLogin = Get<UserLogin>(user.Id);
            var dbPassword = Md5EncryptHelper.Encrypt(DESEncryptHelper.Encrypt(password.ToLower(), userLogin.UserSecretkey).ToLower(), 32).ToLower();
            if (dbPassword != userLogin.UserPassword) throw new Exception("密码不正确，请重新输入");
            userLogin.PreviousVisitTime = userLogin.LastVisitTime.HasValue ? userLogin.LastVisitTime : null;
            userLogin.LastVisitTime = DateTime.Now;
            userLogin.LogOnCount = Convert.ToInt32(userLogin.LogOnCount) + 1;
            SaveChanges();
            return user;
        }



        public void Enable(string id) => throw new NotImplementedException();
        public void Disable(string id) => throw new NotImplementedException();

        public bool VerifyPwd(string userid, string password)
        {
            var userLogin = Read<UserLogin>(p => p.Id.Equals(userid)).Select(p => new { SecretKey = p.UserSecretkey, Password = p.UserPassword }).FirstOrDefaultAsync().Result;
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


        public void RevisePassword(string userPassword, string userId)
        {
            var user = Query<UserLogin>(p => p.Id.Equals(userId)).FirstOrDefaultAsync().Result;
            user.UserSecretkey = Md5EncryptHelper.Encrypt(NumberBuilder.Build_18bit(), 16).ToLower();
            user.UserPassword = Md5EncryptHelper.Encrypt(DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt(userPassword, 32).ToLower(), user.UserSecretkey).ToLower(), 32).ToLower();
            SaveChanges();
        }


        public UserData GetUserData(CurrentUser user)
        {
            var d = new UserData
            {
                UserId = user.Id,
                Organ = user.Organ,
                Duty = user.Duty,
                Roles = user.Roles
            };
            //var menus=Read<Relevance>(p => p.Name.Equals(Relation.RoleMenu) && d.Roles.Contains(p.FirstKey)).Select(p => p.SecondKey).Distinct().ToArrayAsync().Result;
            //var buttons =Read<Relevance>(p => p.Name.Equals(Relation.RoleButton) && d.Roles.Contains(p.FirstKey)).Select(p => p.SecondKey).Distinct().ToArrayAsync().Result;
            d.Menus = Query<Menu>(p=>p.ParentId.Equals("0")).ToListAsync().Result;
            d.Buttons = Read<Button>().ToListAsync().Result;
            var o = Read<User>(p => p.Id.Equals(user.Id)).FirstOrDefaultAsync().Result;
            d.UserName = o.Name;
            d.HeadIcon = o.HeadIcon;
            return d;
        }
    }
}