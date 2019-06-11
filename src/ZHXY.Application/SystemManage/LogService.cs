using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using ZHXY.Common;
using ZHXY.Domain;
using ZHXY.Web.Shared;

namespace ZHXY.Application
{
    /// <summary>
    /// 日志管理
    /// </summary>
    public class LogService : AppService
    {
        public LogService(DbContext r) => R = r;

        public LogService()
        {
            R = new EFContext();
        }
        public void AddLog(AddLogDto input)
        {
            var log = input.MapTo<SysLog>();
            AddAndSave(log);
        }

        public dynamic Load(Pagination input)
        {
            return Read<SysLog>().Paging(input).ToListAsync().Result;
        }


        public dynamic GetLoginKpi(GetLoginStatisticsDto input)
        {
            var query = Read<SysLog>(p => p.Type == "Login" && !string.IsNullOrEmpty(p.UserId) && p.Result == true && p.CreateTime >= input.StartTime && p.CreateTime <= input.EndOfTime)
                .GroupBy(p => new { p.UserId }).Select(g => new LoginStatisticsView
                {
                    UserId = g.Key.UserId,
                    Name = g.Where(p => p.UserId.Equals(g.Key.UserId)).Select(p => p.NickName).FirstOrDefault(),
                    LastLoginTime = g.Where(p => p.UserId.Equals(g.Key.UserId)).Max(p => p.CreateTime),
                    LoginTimes = g.Count(p => p.UserId.Equals(g.Key.UserId)),
                    StartTime = input.StartTime,
                    EndOfTime = input.EndOfTime
                });
            query = string.IsNullOrWhiteSpace(input.Keyword) ? query : query.Where(p => p.Name.Contains(input.Keyword));
            return query.Paging(input).ToListAsync().Result;
        }

    }
}