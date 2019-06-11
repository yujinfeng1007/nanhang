using System.Collections.Generic;
using System.Data.Entity;
using ZHXY.Common;
using ZHXY.Domain;
using ZHXY.Web.Shared;

namespace ZHXY.Application
{
    /// <summary>
    /// 宿舍考勤规则管理
    /// </summary>
    public class DormRuleAppService : AppService
    {
        public DormRuleAppService(DbContext r) : base(r) { }
        public DormTimeRule GetById(string id) => Get<DormTimeRule>(id);
        public void Update(UpdateDormRuleDto input)
        {
            var rule= Get<DormTimeRule>(input.DayOfWeek);
            input.MapTo(rule);
            SaveChanges();
        }

        public List<DormTimeRule> Load() => Read<DormTimeRule>().ToListAsync().Result;
    }
}
