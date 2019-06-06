using ZHXY.Domain;
using System.Collections.Generic;
using System.Linq;
using ZHXY.Common;
using System.Data.Entity;

namespace ZHXY.Application
{
    public class SemesterService : AppService
    {
        public SemesterService(DbContext r) : base(r) { }
        public List<Semester> GetAll() => Read<Semester>().ToList();

    }
}