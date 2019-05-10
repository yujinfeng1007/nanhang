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
    public class StudentAppService : AppService
    {
        public StudentAppService(IZhxyRepository r) : base(r)
        {
        }

        public StudentAppService()
        {
            R = new ZhxyRepository();
        }
        public Student GetByStuNum(string stuNum)
        {
            return Read<Student>(p => p.F_StudentNum.Equals(stuNum)).FirstOrDefaultAsync().Result;
        }

        /// <summary>
        /// 根据卡号
        /// </summary>
        /// <returns></returns>
        public dynamic GetByMacNo(string macNo) => Read<Student>(p => p.F_Mac_No.Equals(macNo)).FirstOrDefaultAsync().Result;

        public string GetIdByStuNum(string stuNum) => Read<Student>(p => p.F_StudentNum.Equals(stuNum)).Select(p => p.F_Id).FirstOrDefaultAsync().Result;

        public string GetMacNoById(string id) => Read<Student>(p => p.F_Id.Equals(id)).Select(p => p.F_Mac_No).FirstOrDefault();
        public dynamic GetStudentByCardNum(string f_CardNum) => Read<Student>(p => p.F_Mac_No == f_CardNum).FirstOrDefaultAsync().Result;
        public dynamic GetListByClassId(string classId) => Read<Student>(t => t.F_Class_ID.Equals(classId)).ToListAsync().Result;

        public dynamic GetList(Expression<Func<Student, bool>> predicate) => Read(predicate).ToListAsync().Result;

        public dynamic GetList(string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            var expression = ExtLinq.True<Student>();
            return Read(expression).ToListAsync().Result;
        }

        public dynamic GetList(Pagination pagination, string keyword, string F_DepartmentId, string F_Grade, string F_Year, string F_ClassType, string F_Class, bool IsSetClass)
        {
            var expression = ExtLinq.True<Student>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_Name.Contains(keyword));
                expression = expression.Or(t => t.F_StudentNum.Contains(keyword));
            }
            if (!string.IsNullOrEmpty(F_DepartmentId))
            {
                expression = expression.And(t => t.F_Divis_ID.Equals(F_DepartmentId));
            }

            if (!string.IsNullOrEmpty(F_Grade))
            {
                expression = expression.And(t => t.F_Grade_ID == F_Grade);
            }

            if (IsSetClass)
            {
                if (!string.IsNullOrEmpty(F_Class))
                {
                    expression = expression.And(t => t.F_Class_ID == F_Class);
                }
                else
                {
                    expression = expression.And(t => t.F_Class_ID != null);
                }
            }
            else
            {
                expression = expression.And(t => t.F_Class_ID == null);
            }
            if (!string.IsNullOrEmpty(F_Year))
            {
                expression = expression.And(t => t.F_Year == F_Year);
            }
            if (!string.IsNullOrEmpty(F_ClassType))
            {
                expression = expression.And(t => t.F_Subjects_ID == F_ClassType);
            }
            return Read(expression).Paging(pagination).ToList();
        }

        public dynamic GetList(Pagination pagination, string keyword, string F_DepartmentId, string F_Grade, string F_Class, string F_Year)
        {
            var expression = ExtLinq.True<Student>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_Name.Contains(keyword));
                expression = expression.Or(t => t.F_StudentNum.Contains(keyword));
            }
            if (!string.IsNullOrEmpty(F_DepartmentId))
            {
                expression = expression.And(t => t.F_Divis_ID.Equals(F_DepartmentId));
            }

            if (!string.IsNullOrEmpty(F_Grade))
            {
                expression = expression.And(t => t.F_Grade_ID == F_Grade);
            }

            if (!string.IsNullOrEmpty(F_Class))
            {
                expression = expression.And(t => t.F_Class_ID == F_Class);
            }
            if (!string.IsNullOrEmpty(F_Year))
            {
                expression = expression.And(t => t.F_Year == F_Year);
            }
            return Read(expression).Paging(pagination).ToList();
        }

        public dynamic GetList() => Read<Student>().ToListAsync().Result;

        public dynamic Get(string id) => Get<Student>(id);
        public dynamic GetOrDefault(string id) => Read<Student>(p => p.F_Id.Equals(id)).FirstOrDefault();

        public dynamic GetBykeyValue(string keyValue) => Read<Student>(t => t.F_StudentNum == keyValue).FirstOrDefaultAsync().Result;

        public void Delete(string id)
        {
            var student= Get<Student>(id);
            var user= new SysUserAppService().Get(student.F_Users_ID);
            if(user!=null)
            {
                throw new Exception("请先删除相关联用户！");
            }
            DelAndSave<Student>(id);
        }


        public void UpdClass(string keyValue, string F_Class_ID, string F_Grade_ID)
        {
            var entitys = new List<Student>();
            var F_Id = keyValue.Split('|');
            for (var i = 0; i < F_Id.Length - 1; i++)
            {
                entitys.Add(new Student
                {
                    F_Id = F_Id[i],
                    F_Class_ID = F_Class_ID,
                    F_Grade_ID = F_Grade_ID,
                });
            }
        }

        public void SubmitForm(StudentDto entity)
        {
            
        }

        public void Update(StudentDto entity)
        {
            var stu = Get<Student>(entity.F_Id);
            entity.MapTo(stu);
            SaveChanges();
        }
    }

    public class StudentDto
    {

        /// <summary>
        /// 学生ID
        /// </summary>
        public string F_Id { get; set; }

        /// <summary>
        /// 年级编码
        /// </summary>


        public string F_Year { get; set; }

        /// <summary>
        /// 学部ID
        /// </summary>

        public string F_Divis_ID { get; set; }

        /// <summary>
        /// 年级ID
        /// </summary>

        public string F_Grade_ID { get; set; }


        public string F_Class_ID { get; set; }

        /// <summary>
        /// 班级类型ID
        /// </summary>
        public string F_Subjects_ID { get; set; }

        /// <summary>
        /// 学号
        /// </summary>
        public string F_StudentNum { get; set; }

        /// <summary>
        /// 入学时间
        /// </summary>

        public DateTime? F_InitDTM { get; set; }

        /// <summary>
        /// 学生系统用户
        /// </summary>
        public string F_Users_ID { get; set; }

        public virtual User studentSysUser { get; set; }

        public string F_Mac_No { get; set; }

    }
}