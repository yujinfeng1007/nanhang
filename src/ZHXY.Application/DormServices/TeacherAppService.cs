using ZHXY.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using ZHXY.Common;

namespace ZHXY.Application
{
    /// <summary>
    /// 老师管理
    /// </summary>
    public class TeacherAppService : AppService
    {
        public TeacherAppService(IZhxyRepository r):base(r)
        {
        }


        public dynamic GetByJobNumber(string num)
        {
            return Read<Teacher>(p => p.JobNumber.Equals(num)).FirstOrDefaultAsync().Result;
        }

       
        public dynamic GetList(Pagination pagination, string keyword)
        {
            var query = Read<Teacher>();
            query = string.IsNullOrWhiteSpace(keyword) ? query : query.Where(p => p.Name.Contains(keyword) );
            return query.Paging(pagination).ToListAsync().Result;
        }

       
        public dynamic GetById(string id) => Get<Teacher>(id);

       
    }
}