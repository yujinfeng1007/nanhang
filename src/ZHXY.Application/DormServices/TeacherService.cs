using ZHXY.Domain;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using ZHXY.Common;

namespace ZHXY.Application
{
    /// <summary>
    /// 老师管理
    /// </summary>
    public class TeacherService : AppService
    {
        public TeacherService(IZhxyRepository r):base(r)
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
        
        //班级绑定班主任
        public void BindTeacherOrg(string classId, string teacherId)
        {                      

            //var rel = new Relevance
            //{
            //    Name = Relation.ClassLeader,
            //    FirstKey = classId,
            //    SecondKey = teacherId
            //};
            //AddAndSave(rel);

            var orgUser= new OrgLeader
            {
                OrgId = classId,
                UserId = teacherId
            };
            AddAndSave(orgUser);

        }

        //获取班主任所绑定的班级
        public List<Organ> GetBindClass(string teacherId) {
            //var classIds = Read<Relevance>(p => p.Name.Equals(Relation.ClassLeader) && p.SecondKey.Equals(teacherId)).Select(p => p.FirstKey).ToArray();
            var classIds = Read<OrgLeader>(p => p.UserId.Equals(teacherId)).Select(p => p.OrgId).ToArray();
            var lists = Read<Organ>(p => classIds.Contains(p.Id)).ToList(); 
            return lists;

        }

    }
}