using ZHXY.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using ZHXY.Common;

namespace ZHXY.Application
{
    /// <summary>
    /// 学生管理
    /// </summary>
    public class StudentService : AppService
    {
        public StudentService(IZhxyRepository r) : base(r)
        {
        }

        public StudentService()
        {
            R = new ZhxyRepository();
        }
        public dynamic GetByStudentNumber(string num)
        {
            return Read<Student>(p => p.StudentNumber.Equals(num)).FirstOrDefaultAsync().Result;
        }

        public dynamic GetIdByStudentNumber(string num)
        {
            return Read<Student>(p => p.StudentNumber.Equals(num)).Select(p=>p.Id).FirstOrDefaultAsync().Result;
        }


        public dynamic GetList(Pagination pagination, string keyword)
        {
            var query = Read<Student>();
            query = string.IsNullOrWhiteSpace(keyword) ? query : query.Where(p => p.Name.Contains(keyword));
            return query.Paging(pagination).ToListAsync().Result;
        }


        public dynamic GetById(string id) => Get<Student>(id);
    }
}