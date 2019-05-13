using ZHXY.Domain;
using System.Data;
using System.Data.Entity;
using System.Linq;

using ZHXY.Common;
using System;

namespace ZHXY.Application
{
    /// <summary>
    /// 岗位管理
    /// </summary>
    public class DutyService : AppService
    {

        public DutyService(IZhxyRepository r) : base(r) { }

        public dynamic GetList(string keyword = "")
        {
            var query = Read<Role>(t => t.Category == 2);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(t => t.Name.Contains(keyword) || t.EnCode.Contains(keyword));
            }
            return query.OrderBy(t => t.SortCode).ToListAsync().Result;
        }

        public dynamic Get(string id) => Get<Role>(id);


        public void Add(AddDutyDto dto)
        {
            var duty = dto.MapTo<Role>();
            duty.Category = 2;
            AddAndSave(duty);
        }

        public void Update(UpdateDutyDto dto)
        {
            var duty = Get<Role>(dto.Id);
            dto.MapTo(duty);
            SaveChanges();
        }

        public string GetEnCode(string dutyId)
        {
            return Read<Role>(p => p.Category == 2 && p.Id.Equals(dutyId)).Select(p => p.EnCode).FirstOrDefaultAsync().Result;
        }

        public void Delete(string[] id)
        {
            foreach (var item in id)
            {
                Del<Role>(item);
            }
            SaveChanges();
        }
    }
}