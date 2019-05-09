using System.Collections.Generic;
using System.Data.Entity;
using ZHXY.Common;
using ZHXY.Domain;

namespace ZHXY.Application
{
    /// <summary>
    /// 宿舍考勤规则管理
    /// </summary>
    public class DormRuleAppService : AppService
    {
        public DormRuleAppService(IZhxyRepository r) : base(r) { }
        public DormRule GetById(string id) => Get<DormRule>(id);
        public void Update(UpdateDormRuleDto input)
        {
            var rule= Get<DormRule>(input.DayOfWeek);
            input.MapTo(rule);
            SaveChanges();
        }

        public List<DormRule> Load() => Read<DormRule>().ToListAsync().Result;
    }
}
