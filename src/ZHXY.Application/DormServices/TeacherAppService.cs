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

        public Teacher GetTeacherByCardNum(string f_CardNum) => Read<Teacher>(p => p.F_Mac_No == f_CardNum).FirstOrDefaultAsync().Result;
        public string GetMacNoById(string Id)=> Read<Teacher>(p => p.Id.Equals(Id)).Select(p => p.F_Mac_No).FirstOrDefaultAsync().Result;
        public List<Teacher> GetSelect(string F_Name, string F_Divis_ID)
        {
            var expression = ExtLinq.True<Teacher>();
            if (!string.IsNullOrEmpty(F_Name))
            {
                expression = expression.And(t => t.F_Name.Contains(F_Name));
                if (!string.IsNullOrEmpty(F_Divis_ID))
                {
                    expression = expression.And(t => t.F_Divis_ID.Equals(F_Divis_ID));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(F_Divis_ID))
                {
                    expression = expression.And(t => t.F_Divis_ID.Equals(F_Divis_ID));
                }
                else
                {
                    throw new Exception("学部不能为空！");
                }
            }
            return Read(expression).OrderBy(t => t.F_Num).ToList();
        }

        public Teacher GetByNum(string num) => Read<Teacher>(p => p.F_Num.Equals(num)).FirstOrDefaultAsync().Result;

        public Teacher GetByUser(string userId) => Read<Teacher>(p => p.UserId.Equals(userId)).FirstOrDefaultAsync().Result;

        /// <summary>
        /// 批量导入数据
        /// </summary>
        public void import(List<Teacher> datas) => Repository.AddDatasImport(datas);

        public new DataTable GetDataTable(string sql, DbParameter[] dbParameter) => Repository.GetDataTable(sql, dbParameter);


        public List<Teacher> GetList(Expression<Func<Teacher, bool>> predicate) => Read(predicate).ToListAsync().Result;

        public List<Teacher> GetList(Pagination pagination, string keyword, string F_DepartmentId, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            var expression = ExtLinq.True<Teacher>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_Name.Contains(keyword));
                expression = expression.Or(t => t.F_Num.Contains(keyword));
            }
            if (!string.IsNullOrEmpty(F_DepartmentId))
            {
                expression = expression.And(t => t.F_Divis_ID.Equals(F_DepartmentId));
            }

            return Repository.FindList(expression, pagination);
        }

        public List<Teacher> GetListSelect(string teacherId)
        {
            var expression = ExtLinq.True<Teacher>();
            if (!string.IsNullOrEmpty(teacherId))
            {
                expression = expression.And(t => t.Id.Equals(teacherId));
            }
            return Read(expression).OrderBy(t => t.F_Num).ToListAsync().Result;
        }

        public List<Teacher> GetList() => Read<Teacher>().ToList();

        public Teacher GetById(string id) => Get<Teacher>(id);

        public void Delete(string id)
        {
            if (!Read<Teacher>(p => p.Id.Equals(id)).Any()) throw new Exception("未找到老师!");
            var teacher = Get<Teacher>(id);
            if (Read<User>(p => p.F_Id.Equals(teacher.UserId)).Any()) throw new Exception("请先删除相关联用户!");
            DelAndSave(teacher);
        }

        public void SubmitForm(Teacher entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                var oldEntity = Repository.Query().Where(t => t.Id == keyValue).FirstOrDefault();
                var F_EntryTime = DateTime.Parse(entity.F_EntryTime.ToString());
                var datenow = DateTime.Parse(DateTime.Now.ToString());
                entity.F_GL = (datenow.Year - F_EntryTime.Year).ToString();
                entity.F_XL = entity.F_GL;

                var F_Birthday = DateTime.Parse(entity.F_Birthday.ToString());
                entity.F_NL = (datenow.Year - F_Birthday.Year).ToString();
                entity.F_Num = entity.F_MobilePhone;
                Repository.AddDatas(new List<Teacher> { entity });
            }
            else
            {
                entity.F_Num = entity.F_MobilePhone;
                Repository.AddDatas(new List<Teacher> { entity });
            }
        }

        public void UpdateForm(Teacher entity) => Repository.Update(entity);
    }
}