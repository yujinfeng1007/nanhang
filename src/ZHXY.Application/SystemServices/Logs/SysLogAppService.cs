using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using ZHXY.Common;
using ZHXY.Domain;

namespace ZHXY.Application
{
    /// <summary>
    /// 日志管理
    /// </summary>
    public class SysLogAppService : AppService
    {
        public SysLogAppService(IZhxyRepository r) => R = r;

        public SysLogAppService()
        {
            R = new ZhxyRepository();
        }
        public void AddLog(AddLogDto input)
        {
            var log = input.MapTo<SysLog>();
            AddAndSave(log);
        }

        public (List<SysLog> list, int recordCount, int pageCount) Load(GetLogListDto input)
        {
            var query = Read<SysLog>();
            var (recordCount, pageCount) = query.CountAsync().Result.ComputePage(input.Take);
            return (query.OrderBy(input.Sort).Skip(input.Skip).Take(input.Take).ToListAsync().Result, recordCount, pageCount);
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