using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ZHXY.Common;
using ZHXY.Domain;

namespace ZHXY.Application
{
    public class FilterIpService : AppService
    {
        public FilterIpService(DbContext r) => R = r;

        public List<FilterIp> GetList(string keyword)
        {
            var expression = ExtLinq.True<FilterIp>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.StartIp.Contains(keyword));
            }
            return Read(expression).ToList();
        }

        public FilterIp GetById(string id) => Get<FilterIp>(id);

        public void Delete(string id)
        {
            var obj = Get<FilterIp>(id);
            DelAndSave(obj);
        }

   

        public void Add(AddFilterIpDto dto)
        {
            var obj = dto.MapTo<FilterIp>();
            AddAndSave(obj);
        }

        public void Update(UpdateFilterIpDto dto)
        {
            var obj = Get<FilterIp>(dto.Id);
            dto.MapTo(obj);
            SaveChanges();
        }
    }
}