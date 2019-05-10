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
        private ITeacherRepository Repository { get; }
        public TeacherAppService(ITeacherRepository repos,IZhxyRepository r):base(r)
        {
            Repository = repos;
            R = r;
        }

        public TeacherAppService()
        {
            Repository = new TeacherRepository();
            R = new ZhxyRepository();
        }

        public dynamic GetSelect(string F_Name, string F_Divis_ID)
        {
            var expression = ExtLinq.True<Teacher>();
            if (!string.IsNullOrEmpty(F_Name))
            {
                expression = expression.And(t => t.Name.Contains(F_Name));
                if (!string.IsNullOrEmpty(F_Divis_ID))
                {
                    expression = expression.And(t => t.DivisId.Equals(F_Divis_ID));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(F_Divis_ID))
                {
                    expression = expression.And(t => t.DivisId.Equals(F_Divis_ID));
                }
                else
                {
                    throw new Exception("学部不能为空！");
                }
            }
            return Read(expression).OrderBy(t => t.JobNumber).ToList();
        }

        public dynamic GetByNum(string num) => Read<Teacher>(p => p.JobNumber.Equals(num)).FirstOrDefaultAsync().Result;

        public dynamic GetByUser(string userId) => Read<Teacher>(p => p.UserId.Equals(userId)).FirstOrDefaultAsync().Result;

        /// <summary>
        /// 批量导入数据
        /// </summary>
        public void Import(List<Teacher> datas) => Repository.AddDatasImport(datas);

        public new DataTable GetDataTable(string sql, DbParameter[] dbParameter) => Repository.GetDataTable(sql, dbParameter);


        public dynamic GetList(Expression<Func<Teacher, bool>> predicate) => Read(predicate).ToListAsync().Result;

        public dynamic GetList(Pagination pagination, string keyword, string F_DepartmentId, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            var expression = ExtLinq.True<Teacher>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.Name.Contains(keyword));
                expression = expression.Or(t => t.JobNumber.Contains(keyword));
            }
            if (!string.IsNullOrEmpty(F_DepartmentId))
            {
                expression = expression.And(t => t.DivisId.Equals(F_DepartmentId));
            }

            if (!string.IsNullOrEmpty(F_CreatorTime_Start))
            {
                var CreatorTime_Start = Convert.ToDateTime(F_CreatorTime_Start + " 00:00:00");
                expression = expression.And(t => t.CreatorTime >= CreatorTime_Start);
            }

            if (!string.IsNullOrEmpty(F_CreatorTime_Stop))
            {
                var CreatorTime_Stop = Convert.ToDateTime(F_CreatorTime_Stop + " 23:59:59");
                expression = expression.And(t => t.CreatorTime <= CreatorTime_Stop);
            }
            expression = expression.And(t => t.DeleteMark == false);
            return Read(expression).Paging(pagination).ToList();
        }

        public dynamic GetListSelect(string teacherId)
        {
            var expression = ExtLinq.True<Teacher>();
            if (!string.IsNullOrEmpty(teacherId))
            {
                expression = expression.And(t => t.Id.Equals(teacherId));
            }
            expression = expression.And(t => t.DeleteMark == false);
            return Read(expression).OrderBy(t => t.JobNumber).ToListAsync().Result;
        }

        public dynamic GetById(string id) => Get<Teacher>(id);

        public void Delete(string id)
        {
            if (!Read<Teacher>(p => p.Id.Equals(id)).Any()) throw new Exception("未找到老师!");
            var teacher = Get<Teacher>(id);
            if (Read<User>(p => p.F_Id.Equals(teacher.UserId)).Any()) throw new Exception("请先删除相关联用户!");
            DelAndSave(teacher);
        }

        public void Submit(TeacherDto entity )
        {
            // todo
            CacheFactory.Cache().RemoveCache(SYS_CONSTS.CLASSTEACHERS);
        }


        /// <summary>
        /// 老师绑定班级
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="classId"></param>
        public void BindClass(string userId, string classId)
        {
            var rel = new Relevance { Name = SYS_CONSTS.REL_TEACHER_CLASS, FirstKey = userId, SecondKey = classId };
            AddAndSave(rel);
        }

        /// <summary>
        /// 获取老师绑定的班级
        /// </summary>
        public string[] GetBoundClass(string userId)
        {
           return Read<Relevance>(p => p.Name.Equals(SYS_CONSTS.REL_TEACHER_CLASS) && p.FirstKey.Equals(userId)).Select(p => p.SecondKey).ToArray();
        }
    }
}