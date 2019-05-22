using ZHXY.Domain;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ZHXY.Common;

namespace ZHXY.Application
{
    /// <summary>
    /// 原始报表服务
    /// </summary>
    public class OriginalReportService : AppService
    {
        public OriginalReportService(IZhxyRepository r) : base(r) { }



        public Student GetOrganIdByStuNum(string stuNum) {
            return Read<Student>(p => p.StudentNumber.Equals(stuNum)).FirstOrDefault();
        }

        public DormStudent GetDormStuById(string studentId) {
            return Read<DormStudent>(p => p.StudentId.Equals(studentId)).FirstOrDefault();
        } 

        public List<OriginalReport> GetMonthOriginalList(Pagination pagination, string studentNum,string startTime,string endTime)
        {
            var now = DateTime.Now;
            string tablePart = now.ToString("yyyyMM");
            string sql = "select * from dhflow_" + tablePart;
            var data = GetDataTable(sql, new DbParameter[] { });
            var list = data.TableToList<OriginalReport>();
            if (!string.IsNullOrEmpty(studentNum))
                list = list.Where(p => p.Code.Equals(studentNum)).ToList();

            if (!string.IsNullOrEmpty(startTime))
            {
                var start= Convert.ToDateTime(startTime);
                if (DateHelper.IsExtendIntoNext("month", now, start)) throw new Exception("请选择当月时间");
                list = list.Where(p => p.Date >= start).ToList();
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                var end = Convert.ToDateTime(endTime);
                if (DateHelper.IsExtendIntoNext("month", now, end)) throw new Exception("请选择当月时间");
                list = list.Where(p => p.Date <= end).ToList();
            }
            pagination.Records = list.Count();
            return list.OrderByDescending(p => p.Date).Skip(pagination.Rows * (pagination.Page - 1)).Take(pagination.Rows).ToList();
        }
        public List<OriginalReport> GetOriginalList(Pagination pagination, string studentNum, string startTime, string endTime)
        {
            var list = new List<OriginalReport>();
            var hasStudentNum = !string.IsNullOrEmpty(studentNum);
            if (!string.IsNullOrEmpty(startTime)&&!string.IsNullOrEmpty(endTime))
            {
                var start = Convert.ToDateTime(startTime);
                var end = Convert.ToDateTime(endTime);
                var sb = new StringBuilder("select * from dhflow_");
                if (DateHelper.IsExtendIntoNext("month", end, start))
                {
                    sb.Append(start.ToString("yyyyMM"));
                    sb.Append(" where "+nameof(OriginalReport.Date)+">='" +startTime+"'");
                    if (hasStudentNum) sb.Append(" and code='"+studentNum+"'");
                    sb.Append(" UNION ALL");
                    sb.Append(" select * from dhflow_");
                    sb.Append(end.ToString("yyyyMM"));
                    sb.Append(" where "+ nameof(OriginalReport.Date) + "<='"+endTime+"'");
                    if (hasStudentNum) sb.Append(" and code='"+studentNum+"'");
                }
                else
                {
                    sb.Append(start.ToString("yyyyMM"));
                    sb.Append(" where "+ nameof(OriginalReport.Date) + ">='"+startTime+"' and "+ nameof(OriginalReport.Date) + "<='"+endTime+"'");
                    if (hasStudentNum) sb.Append(" and code='"+studentNum+"'");
                }
                var ressb = new StringBuilder();
                ressb.Append("select top "+pagination.Rows+" *");
                ressb.Append(" from(select row_number() over(order by "+nameof(OriginalReport.Date)+" asc) as rownumber,*");
                ressb.Append(" from(" + sb.ToString() + ") as a) temp_row");
                ressb.Append(" where rownumber>(("+pagination.Page+"-1)*"+pagination.Rows+ ") order by " + nameof(OriginalReport.Date) + " desc");
                var data = GetDataTable(ressb.ToString(), new DbParameter[] { });                
                list = data.TableToList<OriginalReport>();                 
                var countsb = new StringBuilder();
                countsb.Append("select COUNT(*) " + nameof(Pagination.Records));
                countsb.Append(" from ("+sb.ToString()+") as a");
                var countData = GetDataTable(countsb.ToString(), new DbParameter[] { });
                var count = countData.TableToList<Pagination>();
                pagination.Records = count.FirstOrDefault().Records;
            }
            else
            {
                var now = DateTime.Now;
                string tablePart = now.ToString("yyyyMM");
                var sb = new StringBuilder();
                sb.Append(" select top "+pagination.Rows+" *");
                sb.Append(" from(select row_number()");
                sb.Append(" over(order by "+nameof(OriginalReport.Date)+" asc) as rownumber, *");
                sb.Append(" from dhflow_" + tablePart);
                var countsb = new StringBuilder();
                countsb.Append("select COUNT(*) "+nameof(Pagination.Records)+" from DHFLOW_");
                countsb.Append(tablePart);
                if (hasStudentNum)
                {
                    countsb.Append(" where code='"+studentNum+"'");
                    sb.Append(" where code='" + studentNum + "'");
                }
                sb.Append(") temp_row where rownumber > ((" + pagination.Page + " - 1) * " + pagination.Rows + ") order by "+nameof(OriginalReport.Date)+" desc");
               var data = GetDataTable(sb.ToString(), new DbParameter[]{ });
                list = data.TableToList<OriginalReport>();
                var countData = GetDataTable(countsb.ToString(), new DbParameter[] { });
                var count = countData.TableToList<Pagination>();
                pagination.Records = count.FirstOrDefault().Records;
            }
            return list;
        }
        public List<OriginalReport> GetOriginalListBydate(Pagination pagination, string userId, string date)
        {
            var datetime = Convert.ToDateTime(date);
            var end = datetime.ToString("yyyy-MM-dd") + " 23:59:59";
            var start = datetime.ToString("yyyy-MM-dd") + " 00:00:00";
            var tablePart = datetime.ToString("yyyyMM");
            var sb = new StringBuilder("select * from dhflow_");
            sb.Append(tablePart);
            sb.Append(" where");
            sb.Append(" "+nameof(OriginalReport.Date)+" >=@start and "+nameof(OriginalReport.Date)+"<=@end");
            IDictionary<string, string> paramDic = new Dictionary<string, string>
            {
                { "start", start },
                { "end", end }
            };
            if (!string.IsNullOrEmpty(userId))
            {
                var studentNum = new StudentService().Query<Student>(t => t.UserId == userId).FirstOrDefault()?.StudentNumber;
                sb.Append(" and code=@code");
                paramDic.Add("code", studentNum);
            }
            var parameters = paramDic.Select(kp => new SqlParameter("@" + kp.Key, kp.Value)).Cast<DbParameter>().ToArray();
            var data = GetDataTable(sb.ToString(), parameters);
            var list = data.TableToList<OriginalReport>();
            pagination.Records = list.Count();
            return list.OrderByDescending(p => p.Date).Skip(pagination.Rows * (pagination.Page - 1)).Take(pagination.Rows).ToList();
        }
        public List<OriginalReport> GetOriginalListBydate(string userId, string date)
        {
            var datetime = Convert.ToDateTime(date);
            var end = datetime.ToString("yyyy-MM-dd") + " 23:59:59";
            var start = datetime.ToString("yyyy-MM-dd") + " 00:00:00";
            var tablePart = datetime.ToString("yyyyMM");
            var sb = new StringBuilder("select * from dhflow_");
            sb.Append(tablePart);
            sb.Append(" where");
            sb.Append(" "+nameof(OriginalReport.Date)+" >=@start and "+ nameof(OriginalReport.Date) + "<=@end");
            IDictionary<string, string> paramDic = new Dictionary<string, string>
            {
                { "start", start },
                { "end", end }
            };
            if (!string.IsNullOrEmpty(userId))
            {
                var studentNum = new StudentService().Read<Student>(t => t.UserId == userId).FirstOrDefault()?.StudentNumber;
                sb.Append(" and code=@code");
                paramDic.Add("code", studentNum);
            }
            var parameters = paramDic.Select(kp => new SqlParameter("@" + kp.Key, kp.Value)).Cast<DbParameter>().ToArray();
            var data = GetDataTable(sb.ToString(), parameters);
            var list = data.TableToList<OriginalReport>();
            return list.OrderByDescending(p => p.Date).ToList();
        }
        public string CreateSql(string stuId, ref IDictionary<string, string> parms)
        {
            string tablePart = DateTime.Now.ToString("yyyyMM");
            var sb = new StringBuilder();
            sb.Append("select * from dhflow_");
            sb.Append(tablePart);
            if (!string.IsNullOrEmpty(stuId))
            {
                sb.Append(" where personId=@stuId");
                parms.Add("stuId", stuId);
            }
            return sb.ToString();
        }
    }
}
