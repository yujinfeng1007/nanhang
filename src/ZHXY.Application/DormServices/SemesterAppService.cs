using ZHXY.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using ZHXY.Common;

namespace ZHXY.Application
{
    public class SemesterAppService : AppService
    {
        public SemesterAppService(IZhxyRepository r) => R=r;
        public List<Semester> GetAll() => Read<Semester>().ToList();

    }
}