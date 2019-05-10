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
    public class SysUserAppService : AppService
    {
        private IUserRepository UserRepository { get; }
        private SysOrganizeAppService OrgApp { get; }

        public SysUserAppService(IZhxyRepository repos, IUserRepository userRepository, SysOrganizeAppService organizeApp)
        {
            R = repos;
            UserRepository = userRepository;
            OrgApp = organizeApp;
        }
        public SysUserAppService()
        {
            R = new ZhxyRepository();
            UserRepository = new UserRepository();
            OrgApp = new SysOrganizeAppService();
        }
        public List<User> GetList(Expression<Func<User, bool>> expr = null)
        {
            return Read(expr).ToListAsync().Result;
        }

        /// <summary>
        ///   批量导入数据
        /// </summary>
        /// <param name="data"></param>
        public void Import(List<User> data)
        {
            UserRepository.AddDatas(data);
        }

        public List<User> GetList(Pagination pagination, string keyword, string F_DepartmentId, string F_DutyId, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            var query = Read<User>(p => p.F_Account != "admin");
            query = string.IsNullOrWhiteSpace(keyword) ? query : query.Where(p => p.F_Account.Contains(keyword) || p.F_RealName.Contains(keyword) || p.F_MobilePhone.Contains(keyword));
            query = string.IsNullOrWhiteSpace(F_DutyId) ? query : query.Where(p => p.F_DutyId.Equals(F_DutyId));
            if (!string.IsNullOrEmpty(F_CreatorTime_Start))
            {
                var time = Convert.ToDateTime(F_CreatorTime_Start + " 00:00:00");
                query = query.Where(p => p.F_CreatorTime >= time);
            }
            if (!string.IsNullOrEmpty(F_CreatorTime_Stop))
            {
                var time = Convert.ToDateTime(F_CreatorTime_Stop + " 23:59:59");
                query = query.Where(p => p.F_CreatorTime <= time);
            }
            if (!string.IsNullOrEmpty(F_DepartmentId))
            {
                var orgs = OrgApp.GetListByParentId(F_DepartmentId);
                if (orgs != null && orgs.Count != 0)
                {
                    var deps = orgs.Select(p => p.F_Id).ToArray();
                    query = query.Where(p => deps.Contains(p.F_DepartmentId) || p.F_DepartmentId.Equals(F_DepartmentId));
                }
                else
                {
                    query = query.Where(p => p.F_DepartmentId.Equals(F_DepartmentId));
                }
            }
            pagination.Records = query.CountAsync().Result;
            query = query.OrderBy(pagination.Sidx).Skip(pagination.Rows * (pagination.Page - 1)).Take(pagination.Rows);
            return query.ToListAsync().Result;
        }

        public List<User> GetParentsList(Pagination pag, string account, string keyword, string deptId, string creatorTimeStart, string creatorTimeStop)
        {
            var query = Read<User>(t => t.F_DepartmentId.Equals("parent") && t.F_Account != "admin");
            query = string.IsNullOrEmpty(keyword) ? query : query.Where(t => t.F_Account.Contains(keyword) || t.F_RealName.Contains(keyword) || t.F_MobilePhone.Contains(keyword));
            query = string.IsNullOrEmpty(deptId) ? query : query.Where(t => t.F_DepartmentId.Equals(deptId));
            query = string.IsNullOrEmpty(account) ? query : query.Where(t => t.F_Account.Contains(account));

            if (!string.IsNullOrEmpty(creatorTimeStart))
            {
                var CreatorTime_Start = Convert.ToDateTime(creatorTimeStart + " 00:00:00");
                query = query.Where(t => t.F_CreatorTime >= CreatorTime_Start);
            }
            if (!string.IsNullOrEmpty(creatorTimeStop))
            {
                var CreatorTime_Stop = Convert.ToDateTime(creatorTimeStop + " 23:59:59");
                query = query.Where(t => t.F_CreatorTime <= CreatorTime_Stop);
            }
            pag.Records = query.CountAsync().Result;
            pag.GetOrdering<User>();
            return query.OrderBy(pag.Sidx).Skip(pag.Skip).Take(pag.Rows).ToListAsync().Result;

        }

        //用作导出
        public List<User> GetAll()
        {
            return Read<User>().ToListAsync().Result;
        }

        public User GetByAccount(string account)
        {
            return Read<User>(t => t.F_Account.Equals(account)).FirstOrDefaultAsync().Result;
        }

        public List<User> GetByOrg(string orgId, bool recursive = true)
        {
            var query = Read<User>(p => p.F_Account != "admin");
            if (string.IsNullOrEmpty(orgId)) return query.ToList();
            var orgs = recursive ? OrgApp.GetListByParentId(orgId)?.Select(p => p.F_Id).ToList() : Read<Organize>(p => p.F_ParentId.Equals(orgId)).Select(p => p.F_Id).ToListAsync().Result;
            orgs.Add(orgId);
            return query.Where(p => orgs.Contains(p.F_DepartmentId)).ToListAsync().Result;
        }


        public User Get(string id) => Read<User>(p => p.F_Id.Equals(id)).FirstOrDefaultAsync().Result;

        public string GetRoleById(string id) => Read<User>(p => p.F_Id.Equals(id)).FirstOrDefault()?.F_RoleId;

        public int UpdateByStudentNum(string studentNum, DateTime updateTime, string file)
        {
            var user = Query<User>(p => p.F_Account.Equals(studentNum)).FirstOrDefaultAsync().Result;
            if (null == user) return 0;
            user.F_UpdateTime = updateTime;
            user.F_File = file;
            return SaveChanges();
        }

        public void Delete(string ids)
        {
            var idList = ids.Split('|');
            var removeList = Query<User>(p => idList.Contains(p.F_Id)).ToListAsync().Result;
            var url = Configs.GetValue("CallUrlDel");
            foreach (var id in idList)
            {
                var parameters = "sysid=" + id + "&amp;appid=" + Configs.GetValue("appid") + "";
                var Result = WebHelper.SendRequest(url, parameters, true, "application/json");
            }
            DelAndSave<User>(removeList);
        }

        public void Submit(User userEntity, UserLogin userLogOnEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(userEntity.F_Account))
            {
                var u = UserRepository.FirstOrDefault(t => t.F_Account == userEntity.F_Account);
                if (!u.IsEmpty())
                {
                    if (string.IsNullOrEmpty(keyValue) || (!keyValue.Equals(u.F_Id)))
                    {
                        throw new Exception("该用户已存在");
                    }
                }
            }
            if (!string.IsNullOrEmpty(keyValue))
            {
                userEntity.Modify(keyValue);
            }
            else
            {
                userLogOnEntity.F_UserPassword = Configs.GetValue("UserPassword");
                userEntity.Create();
            }

            var userRoles = new List<UserRole>();
            if (!string.IsNullOrEmpty(userEntity.F_RoleId))
            {
                var roles = userEntity.F_RoleId.Split(',');
                userRoles.AddRange(from r in roles where !r.IsEmpty() select new UserRole { F_Id = Guid.NewGuid().ToString("N").ToUpper(), F_Role = r, F_User = userEntity.F_Id });
            }
            UserRepository.SubmitForm(userEntity, userLogOnEntity, keyValue, userRoles);
        }

        public void SubmitSetUp(User userEntity)
        {
            userEntity.Modify(OperatorProvider.Current.UserId);
            UserRepository.Update(userEntity);
        }

        public void ParentSubmit(User userEntity, UserLogin userLogOnEntity, string id)
        {
            if (Read<User>(t => t.F_Account == userEntity.F_Account).Any()) throw new Exception("该账号已存在");

            if (!string.IsNullOrEmpty(id))
            {
                userEntity.Modify(id);
            }
            else
            {
                userLogOnEntity.F_UserPassword = Configs.GetValue("UserPassword");
                userEntity.Create();
            }

            var userRoles = new List<UserRole>();
            if (!string.IsNullOrEmpty(userEntity.F_RoleId))
            {
                var roles = userEntity.F_RoleId.Split(',');
                userRoles.AddRange(from r in roles where !r.IsEmpty() select new UserRole { F_Id = Guid.NewGuid().ToString("N").ToUpper(), F_Role = r, F_User = userEntity.F_Id });
            }
            UserRepository.SubmitForm(userEntity, userLogOnEntity, id, userRoles);
        }

        public void Update(User userEntity)
        {
            UserRepository.Update(userEntity);
        }

        public User CheckLogin(string username, string password)
        {
            var user = Read<User>(p => p.F_Account.Equals(username)).FirstOrDefaultAsync().Result;
            if (user == null) throw new Exception("账户不存在，请重新输入");
            if (user.F_EnabledMark != true) throw new Exception("账户被系统锁定,请联系管理员");
            var userLogin = Get<UserLogin>(user.F_Id);

            var dbPassword = Md5EncryptHelper.Encrypt(DESEncryptHelper.Encrypt(password.ToLower(), userLogin.F_UserSecretkey).ToLower(), 32).ToLower();
            if (dbPassword != userLogin.F_UserPassword) throw new Exception("密码不正确，请重新输入");
            userLogin.F_PreviousVisitTime = userLogin.F_LastVisitTime.HasValue ? userLogin.F_LastVisitTime : null;
            userLogin.F_LastVisitTime = DateTime.Now;
            userLogin.F_LogOnCount = Convert.ToInt32(userLogin.F_LogOnCount) + 1;
            SaveChanges();
            return user;
        }

        public User SsoLogin(string username)
        {
            var user = Read<User>().FirstOrDefaultAsync(t => t.F_Account == username).Result;
            if (user == null) throw new Exception("账户不存在，请重新输入");
            if (user.F_EnabledMark != true) throw new Exception("账户被系统锁定,请联系管理员");
            var userLogin = Get<UserLogin>(user.F_Id);
            userLogin.F_PreviousVisitTime = userLogin.F_LastVisitTime.HasValue ? userLogin.F_LastVisitTime : null;
            userLogin.F_PreviousVisitTime = userLogin.F_LastVisitTime.ToDate();
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
        public string GetIndexById(string Id)
        {
            var user = Read<User>(p => p.F_Id.Equals(Id)).FirstOrDefault();
            if (user == null) throw new Exception("未找到该用户");
            if (user.F_RoleId.Equals("student")) return user.F_Account;
            else if (user.F_RoleId.Equals("teacher") || user.F_DutyId.Equals("teacherDuty")) return user.F_Id;
            else return null;
        }


        public dynamic GetUserByOrg(string orgId,string keyword)
        {
            if (string.IsNullOrWhiteSpace(orgId) && string.IsNullOrWhiteSpace(keyword)) return null;
            var userQuery = Read<User>();
            userQuery = !string.IsNullOrWhiteSpace(keyword) ? userQuery.Where(p => p.F_RealName.Contains(keyword)) : userQuery.Where(p => p.F_OrganizeId.Equals(orgId));
            return userQuery.Join(
                Read<Organize>(),
                u => u.F_OrganizeId,
                d => d.F_Id,
                (u, o) => new
                {
                    userId = u.F_Id,
                    userName = u.F_RealName,
                    orgName = o.F_FullName,
                }).ToListAsync().Result;
        }
    }
}