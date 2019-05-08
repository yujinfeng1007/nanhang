using System;
using System.Collections.Generic;
using System.Linq;
using ZHXY.Domain;
using ZHXY.Domain.Entity;

namespace ZHXY.Repository
{
    public class ClassDutyRepository : Data.Repository<ClassDuty>, IClassDutyRepository
    {
       
        public void SaveClassDuty(List<ClassDuty> entity)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                if (entity.Count > 0)
                    foreach (var e in entity)
                    {
                        db.Delete<ClassDuty>(t => t.F_Date == e.F_Date);
                        e.Create();
                        db.Insert(e);
                    }

                db.Commit();
            }
        }

        public List<ClassDuty> GetClassDuty(string F_Class)
        {
            using (var db = new Data.UnitWork().BeginTrans())
            {
                var index = (int)DateTime.Now.DayOfWeek;
                index = index == 0 ? 7 : index;
                var d1 = DateTime.Parse(DateTime.Now.AddDays(-(index - 1)).ToString("yyyy-MM-dd") + " 00:00:00");
                var d2 = DateTime.Parse(DateTime.Now.AddDays(7 - index).ToString("yyyy-MM-dd") + " 23:59:59");
                var list = new List<ClassDuty>();
                list = db.QueryAsNoTracking<ClassDuty>().Where(t => t.F_Date >= d1 && t.F_Date <= d2).ToList();
                db.Commit();
                return list;
            }
        }
    }
}