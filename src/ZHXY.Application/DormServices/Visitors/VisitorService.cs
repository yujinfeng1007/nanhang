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
    public class VisitorService :  AppService
    {
        public VisitorService(IZhxyRepository r) : base(r)
        {
        }

        public List<VisitApply> GetList(Pagination pagination, string F_Building, int Time_Type, string startTime, string endTime)
        {
            ////获取记录数
            var CountSql = new StringBuilder("select COUNT(1) from Dorm_VisitLog visit left join Dorm_Dorm dorm on dorm.F_Id=visit.F_Building_ID where visit.F_CreatorTime > '" + startTime + "' and visit.F_CreatorTime < '" + endTime + "'");
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
            sqlStr.Append(" visit.* from Dorm_VisitLog visit left join Dorm_Dorm dorm on dorm.F_Id=visit.F_Building_ID where visit.F_CreatorTime > '" + startTime + "' and visit.F_CreatorTime < '" + endTime + "'");
            if (F_Building != null && F_Building.Trim().Length != 0)
            {
                sqlStr.Append(" and visit.F_Building_Id = '" + F_Building + "'");
            }
            sqlStr.Append(" order by " + pagination.Sidx + ") w order by w.F_Id");
            var ListData = R.Db.Database.SqlQuery<VisitApply>(sqlStr.ToString()).ToList();
            foreach(var visit in ListData)
            {
                visit.DormId = R.Db.Set<DormStudent>().Where(p => p.StudentId == visit.ApplicantId).Select(p => p.Description).FirstOrDefault();
                visit.ApplicantId = R.Db.Set<Student>().Where(p => p.Id == visit.ApplicantId).Select(p => p.Name).FirstOrDefault();
            }
            return ListData;
        }

        public object GetBuilding(string KeyWords)
        {
            var SqlStr = new StringBuilder("SELECT  DISTINCT F_Building_No FROM [dbo].[Dorm_Dorm]  ");
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
            var query = Read<DormVisitLimit>().Join(Read<Student>(),a=>a.StudentId,b=>b.Id,(a, b)=>new{a,b});
            if (KeyWords != null && KeyWords.Length != 0)
            {
                query = IsNumeric.isNumeric(KeyWords) ? query.Where(p => p.b.StudentNumber.Equals(KeyWords)) : query.Where(p => p.b.Name.Contains(KeyWords));
            }
            return query.Select(p => new { id = p.b.Id, text = p.b.Name, limit = p.a.UsableLimit }).OrderBy(p => p.id).Take(20).ToList();
        }

        public object GetForm(string keyValue) => throw new NotImplementedException();

        public object SupervisorByStudent(string StudentId)
        {
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
            return R.Db.Set<DormVisitLimit>().Where(p => p.StudentId == StudentId).Select(p => new { id = StudentId, text = p.UsableLimit}).FirstOrDefault();
        }

        /// <summary>
        /// 审批
        /// </summary>
        public void Approval(string id,bool pass)
        {
            var v = Get<VisitApply>(id);
            v.Status = pass ? "1" : "-1";
            v.ApprovedTime = DateTime.Now;
            SaveChanges();
        }


        /// <summary>
        /// 提交申请
        /// </summary>
        /// <param name="input"></param>
        public void Submit(AddVisitApplyDto input)
        {
            var currentUserId = Operator.GetCurrent().Id;
           // var visit = input.MapTo<VisitApply>();//映射到数据库中对应的表
            var visit = new VisitApply
            {
                ApplicantId = currentUserId,
                VisitorGender = input.VisitorGender,
                VisitorName = input.VisitorName,
                VisitorIDCard = input.VisitorIDCard,
                VisitReason = input.VisitReason,
                VisitStartTime = input.VisitStartTime,
                VisitEndTime = input.VisitEndTime,
                Relationship = input.Relationship,
                Status = "0"
            };
            visit.ApplicantId = currentUserId;
            
            //获取学生所在的dormid
            var dormid = Read<DormStudent>(p => p.StudentId.Equals(currentUserId)).Select(p => p.DormId).FirstOrDefault();
            //获取dorm对应的楼栋Id
            var buildingId = Read<DormRoom>(p => p.Id.Equals(dormid)).Select(p => p.BuildingId).FirstOrDefault();
            //获取楼栋对应的宿管
            var Approvers = Read<Relevance>(p => p.FirstKey.Equals(buildingId)).Select(p => p.SecondKey).ToListAsync().Result;
            if (null == Approvers) throw new Exception("请先绑定宿管!");
            foreach (var approver in Approvers) {
                Add(new VisitApprove {
                    ApproverId = approver,
                    VisitId = visit.Id,
                    ApproveLevel = 1

                });
            }            
            visit.DormId = dormid;
            visit.BuildingId = buildingId;
            AddAndSave(visit);
        }

    }

}

