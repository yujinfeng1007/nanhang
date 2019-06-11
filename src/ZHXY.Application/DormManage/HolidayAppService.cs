using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ZHXY.Common;
using ZHXY.Domain;
using ZHXY.Web.Shared;

namespace ZHXY.Application
{
    /// <summary>
    /// 宿舍考勤规则管理
    /// </summary>
    public class HolidayAppService : AppService
    {
        public HolidayAppService(DbContext r) : base(r) { }
        public Holiday GetById(string id) => Get<Holiday>(id);
        public void Update(UpdateHolidayDto input)
        {
            var rule= Get<Holiday>(input.Id);
            input.MapTo(rule);
            SaveChanges();
        }

        public List<Holiday> Load(Pagination pag, string keyword)
        {
            var query = Read<Holiday>();
            query = string.IsNullOrWhiteSpace(keyword) ? query : query.Where(p => p.Name.Contains(keyword));
            return query.Paging(pag).ToListAsync().Result;
        }

        public void Add(AddHolidayDto dto)
        {
            var user = dto.MapTo<Holiday>();
            AddAndSave(user);
        }

        public void Delete(string[] id)
        {
            var users = Query<Holiday>(p => id.Contains(p.Id)).ToList();
            DelAndSave<Holiday>(users);
        }

    }
}
