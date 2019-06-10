using ZHXY.Domain;
using System;
using System.Linq;
using System.Data.Entity;

namespace ZHXY.Application
{
    public class VisitDormLimitService : AppService
    {
        public VisitDormLimitService(DbContext r) : base(r)
        {
        }
        public VisitDormLimitService() => R = new ZhxyDbContext();
        public object GetGridJson(Pagination pagination, string F_Building, string F_Floor)
        {
            var DormInfoQuery = Read<DormRoom>();
            if (null != F_Building && F_Building.Length > 0)
            {
                DormInfoQuery = DormInfoQuery.Where(p => p.BuildingId.Equals(F_Building));
            }
            if (null != F_Floor && F_Floor.Length > 0)
            {
                DormInfoQuery = DormInfoQuery.Where(p => p.FloorNumber.Equals(F_Floor));
            }
            return DormInfoQuery.Paging(pagination).Join(Read<Building>(), p => p.BuildingId, s => s.Id, (dorm, build) => new
            {
                BuildNo = build.BuildingNo,
                DormNo = dorm.Title,
                DormId = dorm.Id
            }).Join(Read<DormStudent>(), s => s.DormId, n => n.DormId, (dormInfo, student) => new {
                dormInfo.BuildNo,
                dormInfo.DormNo,
                student.StudentId
            }).Join(Read<DormVisitLimit>(), p => p.StudentId, s => s.StudentId, (dormInfo, limit) => new {
                limit.TotalLimit,
                limit.UsableLimit,
                limit.StudentId,
                dormInfo.BuildNo,
                dormInfo.DormNo
            }).Join(Read<Student>(), p => p.StudentId, s => s.Id, (temp, stu) => new {
                temp.TotalLimit,
                temp.UsableLimit,
                temp.StudentId,
                temp.BuildNo,
                temp.DormNo,
                stu.Name,
                stu.GradeId
            }).Join(Read<Organ>(), p => p.GradeId, s => s.Id, (temp, organ) => new {
                Total_Limit = temp.TotalLimit,
                Usable_Limit = temp.UsableLimit,
                F_Build = temp.BuildNo,
                F_Dorm_Num = temp.DormNo,
                F_Student_Name = temp.Name,
                F_College = organ.Name,
                F_EnabledMark = 1
            });
        }

        public object GetFloor(string BuildName)
        {
            return Read<DormRoom>(p => p.BuildingId.Equals(BuildName)).Select(p => new { id = p.FloorNumber, text = p.FloorNumber }).Distinct().ToList();
        }

        public void SubmitForm(int TimesOfWeek, string Organ, string OrganGrade, string OrganCourts, string OrganClass, int AutoSet)
        {
            var query = Read<Student>();
            //判断是否精确到班级
            if (null != OrganClass && !"".Equals(OrganClass))
            {
                var StudentIds = query.Where(p => p.ClassId.Equals(OrganClass)).Select(p => p.Id).ToArray();
                SetVisitTimes(StudentIds, TimesOfWeek, AutoSet);
                return;
            }
            //判断是否精确到分院
            if (null != OrganCourts && !"".Equals(OrganCourts))
            {
                var StudentIds = query.Where(p => p.GradeId.Equals(OrganCourts)).Select(p => p.Id).ToArray();
                SetVisitTimes(StudentIds, TimesOfWeek, AutoSet);
                return;
            }
            //判断是否精确到年级
            if (null != OrganGrade && !"".Equals(OrganGrade))
            {
                var StudentIds = query.Where(p => p.DivisId.Contains(OrganGrade)).Select(p => p.Id).ToArray();
                SetVisitTimes(StudentIds, TimesOfWeek, AutoSet);
                return;
            }
            //判断是否精确到学院
            if (null != Organ && !"".Equals(Organ))
            {
                var StudentIds = query.Select(p => p.Id).ToArray();
                SetVisitTimes(StudentIds, TimesOfWeek, AutoSet);
                return;
            }
        }

        //查询学院
        public object FindOrgan(string OrganName)
        {
            var query = Read((Organ p) => p.ParentId.Equals("2"));
            if (null != OrganName && !"".Equals(OrganName))
            {
                query.Where(p => p.Name.Contains(OrganName));
            }
            return query.Select(p => new { id = p.Id, text = p.Name }).ToList();
        }

        //查询年级
        public object FindOrganGrade(string OrganId, string GradeName)
        {
            var query = Read((Organ p) => p.ParentId.Equals(OrganId));
            if (null != GradeName && !"".Equals(GradeName))
            {
                query.Where(p => p.Name.Contains(GradeName));
            }
            return query.Select(p => new { id = p.Id, text = p.Name }).ToList();
        }

        //查询分院
        public object FindOrganCourts(string GradeId, string CourtName)
        {
            var query = Read((Organ p) => p.ParentId.Equals(GradeId));
            if (null != CourtName && !"".Equals(CourtName))
            {
                query.Where(p => p.Name.Contains(CourtName));
            }
            return query.Select(p => new { id = p.Id, text = p.Name }).ToList();
        }

        //查询班级
        public object FindOrganClass(string CourtsId, string ClassName)
        {
            var query = Read((Organ p) => p.ParentId.Equals(CourtsId));
            if (null != ClassName && !"".Equals(ClassName))
            {
                query.Where(p => p.Name.Contains(ClassName));
            }
            return query.Select(p => new { id = p.Id, text = p.Name }).ToList();
        }


        /// <summary>
        /// 工具方法 （批量设置学生每周访问额度）
        /// 已设置的学生则更新，没设置的则设置
        /// </summary>
        /// <param name="Ids"></param>
        /// <param name="TimesOfWeek"></param>
        /// <param name="AutoSet"></param>
        /// <returns></returns>
        public void SetVisitTimes(string[] Ids, int TimesOfWeek, int AutoSet)
        {
            var DormStudents = Read<DormVisitLimit>(p => p.Enabled == true).Select(p => p.StudentId).ToList();
            var InsertIds = Ids.Except(DormStudents); //需添加的数据
            var UpdateIds = Ids.Intersect(DormStudents); // 需修改的数据
            var DateTimeNow = DateTime.Now;

            ///添加
            AddRangeAndSave(InsertIds.Select(p => new DormVisitLimit
            {
                StudentId = p,
                TotalLimit = TimesOfWeek,
                UsableLimit = TimesOfWeek,
                IsAutoSet = AutoSet,
                CreatedTime = DateTimeNow,
                UpdateTime = DateTimeNow
            }));

            ///修改
            Query<DormVisitLimit>().Where(p => UpdateIds.Contains(p.StudentId)).ToList().ForEach(item =>
            {
                item.TotalLimit = TimesOfWeek;
                item.UsableLimit = TimesOfWeek;
                item.IsAutoSet = AutoSet;
                item.UpdateTime = DateTimeNow;
            });
            SaveChanges();
        }

        private class DormVisitLimitMoudle
        {
            public string F_Build { get; set; }
            public string F_Dorm_Num { get; set; }
            public string F_Student_Name { get; set; }
            public string F_College { get; set; }
            public int Total_Limit { get; set; }
            public int Usable_Limit { get; set; }
            public bool F_EnabledMark { get; set; }

        }
    }
}
