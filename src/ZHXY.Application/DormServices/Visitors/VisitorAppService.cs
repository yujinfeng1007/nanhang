using ZHXY.Common;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using ZHXY.Domain;
using System;
using System.Text;
using ZHXY.Common.IsNumeric;
using System.Data.Entity;

namespace ZHXY.Application
{
    /// <summary>
    /// 访客管理
    /// </summary>
    public class VisitorAppService :  AppService
    {
        public VisitorAppService(IZhxyRepository r) : base(r)
        {
        }

        public List<VisitApply> GetList(Pagination pagination, string F_Building, int Time_Type, string startTime, string endTime)
        {
            ////获取记录数
            var CountSql = new StringBuilder("select COUNT(1) from Dorm_VisitLog visit left join Dorm_DormInfo dorm on dorm.F_Id=visit.F_Building_ID where visit.F_CreatorTime > '" + startTime + "' and visit.F_CreatorTime < '" + endTime + "'");
            if (F_Building != null && F_Building.Trim().Length != 0)
            {
                CountSql.Append(" and visit.F_Building_Id = '" + F_Building + "'");
            }
            pagination.Records = R.Db.Database.SqlQuery<int>(CountSql.ToString()).First();
            if (pagination.Page * pagination.Rows > pagination.Records)
            {
                pagination.Rows = pagination.Records % pagination.Rows;
            }
            var sqlStr = new StringBuilder("select top " + pagination.Rows + " * from (select top " + pagination.Page * pagination.Rows);
            sqlStr.Append(" visit.* from Dorm_VisitLog visit left join Dorm_DormInfo dorm on dorm.F_Id=visit.F_Building_ID where visit.F_CreatorTime > '" + startTime + "' and visit.F_CreatorTime < '" + endTime + "'");
            if (F_Building != null && F_Building.Trim().Length != 0)
            {
                sqlStr.Append(" and visit.F_Building_Id = '" + F_Building + "'");
            }
            sqlStr.Append(" order by " + pagination.Sidx + ") w order by w.F_Id");
            var ListData = R.Db.Database.SqlQuery<VisitApply>(sqlStr.ToString()).ToList();
            foreach(var visit in ListData)
            {
                visit.DormId = R.Db.Set<DormStudent>().Where(p => p.F_Student_ID == visit.ApplicantId).Select(p => p.F_Memo).FirstOrDefault();
                visit.ApplicantId = R.Db.Set<Student>().Where(p => p.F_Id == visit.ApplicantId).Select(p => p.F_Name).FirstOrDefault();
            }
            return ListData;
        }

        public object GetBuilding(string KeyWords)
        {
            var SqlStr = new StringBuilder("SELECT  DISTINCT F_Building_No FROM [dbo].[Dorm_DormInfo]  ");
            if(KeyWords != null && KeyWords.Length != 0)
            {
                SqlStr.Append(" WHERE F_Building_No LIKE '%" + KeyWords + "%'");
            }
            SqlStr.Append(" ORDER BY F_Building_No ASC ");
            return R.Db.Database.SqlQuery<string>(SqlStr.ToString()).Select(p => new
            {
                id = p,
                text = p
            }).ToList();
        }

        public object SearchStudents(string KeyWords)
        {
            var query = R.Db.Set<DormVisitLimit>().Where(p => p.student.F_DeleteMark == false);
            if (KeyWords != null && KeyWords.Length != 0)
            {
                query = IsNumeric.isNumeric(KeyWords) ? query.Where(p => p.student.F_StudentNum.Equals(KeyWords)) : query.Where(p => p.student.F_Name.Contains(KeyWords));
            }
            return query.Select(p => new { id = p.student.F_Id, text = p.student.F_Name, limit = p.UsableLimit }).OrderBy(p => p.id).Take(20).ToList();
        }

        public object GetForm(string keyValue) => throw new NotImplementedException();

        public object SupervisorByStudent(string StudentId)
        {
            //var ReturnVisor = new School_supervisor();
            //string StudentBuildName = R.Db.Database.SqlQuery<string>("SELECT left(F_Memo,charindex('栋',F_Memo)-1)  FROM [dbo].[Dorm_DormStudent]where F_Student_ID='"+StudentId+"'").FirstOrDefault();
            //var SuperVBisor = R.Db.Set<School_supervisor>().AsNoTracking().Where(p => p.SupervisorDorm.Contains(StudentBuildName)).ToList();
            //foreach(var data in SuperVBisor)
            //{
            //    var BuildNameArr = data.SupervisorDorm.Split(',');
            //    foreach(var BuildName in BuildNameArr)
            //    {
            //        if (BuildName.Equals(StudentBuildName))
            //        {
            //            ReturnVisor = data;
            //            break;
            //        }
            //    }
            //}
            //return ReturnVisor;
            return null;
        }

        /// <summary>
        /// 获取访问详情
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public object GetDetail(string id) => Get<VisitApply>(id);

        public object VisivorByStudent( Pagination pag, string userId,int status)
        {
            var query = Read<VisitApply>(p => p.ApplicantId.Equals(userId) && p.Status.Equals(status));
            return query.Paging(pag).ToListAsync().Result;
        }

        public void CheckVisitor(string UserId, int CheckType, string VisitLogId)
        {
            string updateSql = "update visit set f_memo = 1 from [dbo].[Dorm_VisitLog] visit where visit.F_Id = '"+ VisitLogId + "'";
            R.Db.Database.ExecuteSqlCommand(updateSql);
        }

        public object SearchStudentLimit(string StudentId)
        {
            return R.Db.Set<DormVisitLimit>().Where(p => p.Student_Id == StudentId).Select(p => new { id = StudentId, text = p.UsableLimit}).FirstOrDefault();
        }

        /// <summary>
        /// 审批
        /// </summary>
        public void Approval(string id,bool pass)
        {
            var v = Get<VisitApply>(id);
            v.Status = pass ? 1 : -1;
            v.ProcessingTime = DateTime.Now;
            SaveChanges();
        }


        /// <summary>
        /// 提交申请
        /// </summary>
        /// <param name="input"></param>
        public void Submit(AddVisitApplyDto input)
        {
            var visit = input.MapTo<VisitApply>();
            visit.ApplicantId = OperatorProvider.Current.UserId;
            visit.DormId = "";
            visit.BuildingId = "";
            AddAndSave(visit);
        }

    }

}

