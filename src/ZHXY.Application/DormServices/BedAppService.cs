using ZHXY.Common;
using System.Collections.Generic;
using ZHXY.Domain;

namespace ZHXY.Application
{
    public class BedAppService :  AppService
    {
        public BedAppService(IZhxyRepository r) : base(r) { }
        public List< DormBed> GetList(Pagination pagination)
        {
            return null;
        }

       
    }
}

