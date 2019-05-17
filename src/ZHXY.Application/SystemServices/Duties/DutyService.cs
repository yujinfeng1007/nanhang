using ZHXY.Domain;
using System.Data;
using System.Data.Entity;
using System.Linq;
using ZHXY.Common;
using System.Collections.Generic;

namespace ZHXY.Application
{
    /// <summary>
    /// 岗位管理
    /// </summary>
    public class DutyService : AppService
    {

        public DutyService(IZhxyRepository r) : base(r) { }

        public List<Duty> GetList(string keyword = "")
        {
            var query = Read<Duty>();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(t => t.Name.Contains(keyword) || t.Code.Contains(keyword));
            }
            return query.OrderBy(t => t.SortCode).ToListAsync().Result;
        }

        public dynamic Get(string id) => Get<Duty>(id);


        public void Add(AddDutyDto dto)
        {
            var duty = dto.MapTo<Duty>();
            AddAndSave(duty);
        }

        public void Update(UpdateDutyDto dto)
        {
            var duty = Get<Duty>(dto.Id);
            dto.MapTo(duty);
            SaveChanges();
        }

        public string GetCode(string dutyId)
        {
            return Read<Duty>(p => p.Id.Equals(dutyId)).Select(p => p.Code).FirstOrDefaultAsync().Result;
        }

        public void Delete(string[] id)
        {
            foreach (var item in id)
            {
                Del<Duty>(item);
            }
            SaveChanges();
        }
    }
}