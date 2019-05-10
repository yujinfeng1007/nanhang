using ZHXY.Common;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Data.Entity;
using ZHXY.Domain;
using System;

namespace ZHXY.Application
{
    /// <summary>
    /// 宿舍管理
    /// </summary>
    public class DormRoomAppService : AppService
    {
        public DormRoomAppService(IZhxyRepository r) : base(r)
        {
        }

        public dynamic Load(Pagination p)
        {
            return Read<DormRoom>().OrderBy($"{p.Sidx} {p.Sord}").Skip(p.Skip).Take(p.Rows).ToListAsync().Result;
        }

        public object GetById(string id) => throw new NotImplementedException();
    }
}

