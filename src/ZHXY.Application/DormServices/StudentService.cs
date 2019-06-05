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
            return query.Paging(pagination).Join(Read<Organ>(), p => p.GradeId, s=> s.Id, (stu, organ) => new {
                id = stu.Id,
                facePic = stu.FacePic,
                name = stu.Name,
                gender = stu.Gender,
                studentNumber = stu.StudentNumber,
                credType = stu.CredType,
                credNumber = stu.CredNumber,
                grade = organ.Name,
                mobilePhone = stu.MobilePhone,
                stu.ClassId,
                stu.DivisId
            }).Join(Read<Organ>(), p => p.ClassId, s=> s.Id, (temp, organ) => new {
                temp.id,
                temp.facePic,
                temp.name,
                temp.gender,
                temp.studentNumber,
                temp.credType,
                temp.credNumber,
                temp.grade,
                temp.mobilePhone,
                classId = organ.Name,
                temp.DivisId
            }).Join(Read<Organ>(), p => p.DivisId, s => s.Id, (temp, organ) => new {
                temp.id,
                temp.facePic,
                temp.name,
                temp.gender,
                temp.studentNumber,
                temp.credType,
                temp.credNumber,
                temp.grade,
                temp.mobilePhone,
                temp.classId,
                divis = organ.Name
            }).ToListAsync().Result;
        }       


        public dynamic GetById(string id) => Get<Student>(id);

        public List<Student> GetListByClassId(string classId) => Read<Student>(t => t.ClassId.Equals(classId)).ToListAsync().Result;

        //获取宿舍学生
        public dynamic GetDormyById(string id)
        {
            return Read<DormStudent>(p => p.StudentId.Equals(id)).FirstOrDefaultAsync().Result;            
        }
        //更新学生头像
        public void UpdIco(string userId, string filepath)
        {
            var student = Get<Student>(userId);
            student.FacePic = filepath;
            SaveChanges();
        }
    }
}