using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using ZHXY.Domain;

namespace ZHXY.Application
{
    /// <summary>
    /// 绩效管理
    /// </summary>
    public class PerformanceAppService : AppService
    {
        public PerformanceAppService(DbContext r) : base(r) { }

        /// <summary>
        /// 获取登录考核
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public dynamic GetLoginStatistics(GetLoginStatisticsDto input)
        {
            if (string.IsNullOrEmpty(input.OrgId)) return null;
            var orgs = new List<string> { input.OrgId };
            this.GetChildOrg(input.OrgId, orgs);
            var userQuery = Read<Teacher>(p => orgs.Contains(p.OrganId));
            userQuery = string.IsNullOrWhiteSpace(input.Keyword) ? userQuery : userQuery.Where(p => p.Name.Contains(input.Keyword));
            var query = userQuery.GroupJoin(
                Read<SysLog>(p => p.Type == "Login" && !string.IsNullOrEmpty(p.UserId) && p.Result == true && p.CreateTime >= input.StartTime && p.CreateTime <= input.EndOfTime),
                u => u.Id,
                l => l.UserId,
                (user, log) => new LoginStatisticsView
                {
                    UserId = user.Id,
                    Name = user.Name,
                    LastLoginTime = log.Max(p => p.CreateTime),
                    LoginTimes = log.Count(),
                    StartTime = input.StartTime,
                    EndOfTime = input.EndOfTime
                });
            return query.Paging(input).ToListAsync().Result;
        }

        /// <summary>
        /// 获取登录详情列表
        /// </summary>
        /// <param name="input"></param>
        public dynamic GetLoginDetail(GetLoginDetailDto input)
        {
            var user = Read<User>(p => p.Id.Equals(input.UserId)).FirstOrDefaultAsync().Result;
            if (null == user) return null;
            var departmentName = Read<Organ>(p => p.Id.Equals(user.OrganId)).Select(p => p.Name).FirstOrDefaultAsync().Result;
            var query = Read<SysLog>(p => p.Type == "Login" && p.UserId.Equals(input.UserId) && p.Result == true && p.CreateTime >= input.StartTime && p.CreateTime <= input.EndOfTime);
            //var ordering = input.GetOrdering<SysLog>();
            return query.Paging(input)
              .Select(p => new LoginDetailView
              {
                  Name = user.Name,
                  Department = departmentName,
                  LoginTime = p.CreateTime,
                  WayOfLogin = p.ModuleName
              }).ToListAsync().Result;
        }

        /// <summary>
        /// 获取楼栋进出考核
        /// </summary>
        public (List<BuildingAccessStatisticsView> list, int recordCount, int pageCount) GetBuildingAccessStatistics(GetBuildingAccessStatisticsDto input)
        {

            return (null, 0, 0);
        }

        public List<BuildingAccessDetailView> GetBuildingAccessDetail(GetBuildingAccessDetailDto input)
        {
            return null;

        }

    }


}