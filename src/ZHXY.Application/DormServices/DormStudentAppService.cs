using ZHXY.Common;
using System.Collections.Generic;
using System.Linq;
using ZHXY.Domain;
using System;

namespace ZHXY.Application
{
    /// <summary>
    /// 宿舍学生管理
    /// </summary>
    public class DormStudentAppService : AppService
    {
        public DormStudentAppService(IZhxyRepository r) => R = r;
        public object GetGridJson(Pagination pagination, string keyword, string F_Year, string F_Semester, string F_Divis, string F_Grade, string F_Class)
        {
            var param = new
            {
                keyword,
                F_Year,
                F_Semester,
                F_Divis,
                F_Grade,
                F_Class
            };
            return Read<DormStudent>().Paging(pagination).Where(p => p.F_Bed_ID == "s").ToList();
            
        }

        public List<DormStudent> GetList(Pagination pagination, string F_Year, string F_Semester, string F_Divis, string F_Grade, string F_Class)
        {
            return Read<DormStudent>().Paging(pagination).ToList();
        }

        public object GetById(string keyValue) => throw new NotImplementedException();
    }
}

