using ZHXY.Domain;
using System.Collections.Generic;
using System.Linq;
using ZHXY.Common;

namespace ZHXY.Application
{
    public class SemesterService : AppService
    {
        public SemesterService(IZhxyRepository r) : base(r) { }
        public List<Semester> GetAll() => Read<Semester>().ToList();

    }
}