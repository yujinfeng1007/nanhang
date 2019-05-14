using ZHXY.Common;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using ZHXY.Domain;
using System;

namespace ZHXY.Application
{
    public class InOutReceiveAppService : AppService
    {
        public InOutReceiveAppService(IZhxyRepository r) : base(r) { }
        public List<InOutReceive> GetList(Pagination pagination)
        {
            var list = Read<InOutReceive>().Paging(pagination).ToList();
            return list.Select(t =>
            {
                var ids = t.F_ReceiveUser.Split(',');
                var names = string.Join(",", Read<User>(x => ids.Contains(x.Id)).Select(x => x.Name).ToArray());
                return new InOutReceive
                {
                    F_Id = t.F_Id,
                    F_Type = t.F_Type,
                    F_ReceiveUser = names
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

