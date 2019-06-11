using System.Data;
using System.Collections.Generic;
using System.Linq;
using ZHXY.Domain;
using System;
using System.Data.Entity;
using ZHXY.Web.Shared;

namespace ZHXY.Application
{
    public class InOutReceiveAppService : AppService
    {
        public InOutReceiveAppService(DbContext r) : base(r) { }
        public List<InOutReceive> GetList(Pagination pagination)
        {
            var list = Read<InOutReceive>().Paging(pagination).ToList();
            return list.Select(t =>
            {
                var ids = t.ReceiveUser.Split(',');
                var names = string.Join(",", Read<User>(x => ids.Contains(x.Id)).Select(x => x.Name).ToArray());
                return new InOutReceive
                {
                    Id = t.Id,
                    Type = t.Type,
                    ReceiveUser = names
                };
            }).ToList();
        }

        public object GetById(string id)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id) => throw new NotImplementedException();
    }
}

