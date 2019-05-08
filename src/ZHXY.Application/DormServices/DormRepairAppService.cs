using ZHXY.Common;
using System.Collections.Generic;
using ZHXY.Domain;
using System.Linq;
using System;

namespace ZHXY.Application
{
    /// <summary>
    /// 宿舍维修管理
    /// </summary>
    public class DormRepairAppService : AppService
    {
       public DormRepairAppService(IZhxyRepository r) => R = r;


        public List< DormRepair> GetList(Pagination p)
        {
            return Read<DormRepair>().Paging(p).ToList();
        }

        public object GetById(string keyValue) => throw new NotImplementedException();
    }
}

