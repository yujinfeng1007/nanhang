using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using EntityFramework.Extensions;
using System.Data;
using ZHXY.Domain;
using TaskApi.NH;
using ZHXY.Application;

namespace TaskApi.Job
{
    public class NanHangJob : IJob
    {
        private ILog Logger { get; } = LogManager.GetLogger(typeof(NanHangJob));
        public void Execute(IJobExecutionContext context)
        {
            var db = new EFContext();
            Console.WriteLine("************************************        开始同步南航师生信息       ************************************");
            ProcessOrgInfo(db); //同步教师组织机构信息
            ProcessOrgInfoStu(db); //同步学生组织机构信息
            ProcessSysOrgan(db);//处理sys_organization表的相关等级标识
            ProcessTeacher(db); //同步教师信息
            ProcessStudent(db); //同步学生信息
            Console.WriteLine("************************************        同步南航师生信息结束       ************************************");
        }


        /// <summary>
        /// 处理同步学生信息 学生信息同步6张表
        /// 分别为： School_Students，Sys_User，Sys_User_Role，Sys_UserLogOn，Dorm_DormStudent，Dorm_Dorm
        /// </summary>
        public void ProcessStudent(EFContext db)
        {
            Console.WriteLine("南航项目：开始同步学生信息 --> " + DateTime.Now.ToLocalTime());
            var sw = new Stopwatch();
            sw.Start();
            var newDb = new NHModel();
            ///---------      Step1: 更新 School_Students 表数据      ---------///
            ProcessSchoolStudentInfo(db, newDb);
            Console.WriteLine(" *** 同步 School_Students 结束 。");
            ///---------      Step2: 更新 Sys_User 表数据      ---------///
            ProcessSchoolStudentSysUser(db, newDb);
            Console.WriteLine(" *** 同步 Sys_User 结束 。");
            ///---------      Step3: 更新 Dorm_Dorm 表数据      ---------///
            ProcessSchoolStudentDormInfo(db, newDb);
            Console.WriteLine(" *** 同步 宿舍相关的表 结束 。");
            ///---------      Step5: 更新 Dorm_Dorm 表数据      ---------///
            ProcessStudentSysUserRole(db, newDb);
            Console.WriteLine(" *** 同步 用户角色表 结束 。");
            //newDb.BulkDelete(newDb.StudentInfoes.ToList()); //批量删除中间表的所有数据
            newDb.Dispose();
            db.Dispose();
            sw.Stop();
            Console.WriteLine("南航项目：同步学生信息结束 --> 合计耗时：" + sw.ElapsedMilliseconds / 1000 + "s");
        }

        /// <summary>
        /// 处理同步教师信息
        /// 此方法分别更新四张表，分别是：School_Teachers，Sys_User，Sys_User_Role，Sys_UserLogOn
        /// </summary>
        public void ProcessTeacher(EFContext db)
        {
            Console.WriteLine("南航项目：开始同步教师信息 --> " + DateTime.Now.ToLocalTime());
            var sw = new Stopwatch();
            sw.Start();
            var newDb = new NHModel();
            ///---------      Step1: 更新 School_Teachers 表数据      ---------///
            ProcessSchoolTeacherInfo(db, newDb);
            ///---------      Step2: 更新 Sys_User 表数据      ---------///
            ProcessSchoolTeacherSysUser(db, newDb);
            ///---------      Step3: 更新 Sys_User_Role 表数据      ---------///
            ProcessSchoolTeacherSysUserRole(db, newDb);
            //newDb.BulkDelete(newDb.TeacherInfoes.ToList()); //删除中间表的所有教师数据
            newDb.Dispose();
            sw.Stop();
            Console.WriteLine("南航项目：同步教师信息结束 --> 合计耗时：" + sw.ElapsedMilliseconds / 1000 + "s");
        }

        /// <summary>
        /// 处理同步组织机构信息
        /// </summary>
        public void ProcessOrgInfo(EFContext db)
        {
            Console.WriteLine("开始同步教师组织机构信息: 南航项目 --> " + DateTime.Now.ToLocalTime());
            var sw = new Stopwatch();
            sw.Start();
            //校方数据集(分为两种：修改数据和新增数据)
            var newDb = new NHModel();
            var newData = newDb.Set<Teacher_Organ>().AsNoTracking().Select(p => new OrganMoudle { Id = p.OrgId, Name = p.OrgName, ParentId = p.ParentOrgId, EnCode = p.OrgId }).ToList();
            //获取生产环境数据库数据集
            var oldData = db.Set<Org>().AsNoTracking().Select(p => new OrganMoudle { Id = p.Id, Name = p.Name, ParentId = p.ParentId, EnCode = p.Code }).ToList();
            var addList = newData.Except(oldData).ToList(); //取差集 （新增和修改数据）

            var idList = oldData.Select(p => p.Id).ToList();
            var endList = new List<Org>();
            foreach (var org in addList)
            {
                if (org.ParentId == null) { org.ParentId = "3"; }
                if (idList.Contains(org.Id))
                {
                    db.Set<Org>().Where(p => p.Id.Equals(org.Id)).Update(p => new Org
                    {
                        ParentId = org.ParentId,
                        Name = org.Name
                    });
                }
                else
                {
                    endList.Add(new Org()
                    {
                        Id = org.Id,
                        Name = org.Name,
                        ParentId = org.ParentId,
                        Code = org.EnCode
                    });
                }
            }
            //newDb.BulkDelete(newDb.OrganizationInfoes.ToList()); //操作完成后，删除取出来的数据
            //oldDb.BulkInsert(endList);
            db.Set<Org>().AddRange(endList);
            db.SaveChanges();
            newDb.Dispose();
            sw.Stop();
            Console.WriteLine("南航项目：同步组织机构信息结束 --> 合计耗时：" + sw.ElapsedMilliseconds / 1000 + "s");
        }

        /// <summary>
        /// 处理同步组织机构信息（学生组织结构）
        /// </summary>
        public void ProcessOrgInfoStu(EFContext db)
        {
            Console.WriteLine("开始同步学生组织机构信息: 南航项目 --> " + DateTime.Now.ToLocalTime());
            var sw = new Stopwatch();
            sw.Start();
            //校方数据集(分为两种：修改数据和新增数据)
            var newDb = new NHModel();
            var newData = newDb.Set<Student_Organ>().AsNoTracking().Select(p => new OrganMoudle { Id = p.OrgId, Name = p.OrgName, ParentId = p.ParentOrgId, EnCode = p.OrgId }).ToList();
            //获取生产环境数据库数据集
            var oldData = db.Set<Org>().AsNoTracking().Select(p => new OrganMoudle { Id = p.Id, Name = p.Name, ParentId = p.ParentId, EnCode = p.Code }).ToList();
            var addList = newData.Except(oldData).ToList(); //取差集 （新增和修改数据）
            var idList = oldData.Select(p => p.Id).ToList();
            var endList = new List<Org>();
            foreach (var org in addList)
            {
                if (org.ParentId == null) { org.ParentId = "2"; }
                if (idList.Contains(org.Id))
                {
                    db.Set<Org>().Where(p => p.Id.Equals(org.Id)).Update(p => new Org
                    {
                        ParentId = org.ParentId,
                        Name = org.Name
                    });
                }
                else
                {
                    endList.Add(new Org() {
                        Id = org.Id,
                        Name = org.Name,
                        ParentId = org.ParentId,
                        Code = org.EnCode
                    });
                }
            }
            //newDb.BulkDelete(newDb.OrganizationInfo_stu.ToList()); //操作完成后，删除取出来的数据
            //oldDb.BulkInsert(endList);
            db.Set<Org>().AddRange(endList);
            db.SaveChanges();
            newDb.Dispose();
            sw.Stop();
            Console.WriteLine("南航项目：同步组织机构信息结束 --> 合计耗时：" + sw.ElapsedMilliseconds / 1000 + "s");
        }

        /// <summary>
        /// 同步之后的表，添加各个登记的标识：
        /// </summary>
        public void ProcessSysOrgan(EFContext db)
        {
            //修改学生年级的 F_CategoryId 为 "Division"
            var UpdateDivSql = "UPDATE organ2 set organ2.category_id='Division' from [dbo].[zhxy_organ] organ1  left join zhxy_organ organ2 on organ2.p_id=organ1.id where organ1.p_id='2'";
            //修改学生的学部（）
            var UpdateGradeSql = "UPDATE organ3 set organ3.category_id='Grade' from[dbo].[zhxy_organ] organ1 left join zhxy_organ organ2 on organ2.p_id = organ1.id left join zhxy_organ organ3 on organ3.p_id = organ2.id where organ1.p_id = '2'";
            //修改学生的班级
            var UpdateClassSql = "UPDATE organ4 set organ4.category_id='Class' from[dbo].[zhxy_organ] organ1 left join zhxy_organ organ2 on organ2.p_id = organ1.id left join zhxy_organ organ3 on organ3.p_id = organ2.id left join zhxy_organ organ4 on organ4.p_id = organ3.id where organ1.p_id = '2'";

            db.Database.ExecuteSqlCommand(UpdateDivSql);
            db.Database.ExecuteSqlCommand(UpdateGradeSql);
            db.Database.ExecuteSqlCommand(UpdateClassSql);
        }

        /// <summary>
        /// 简化程序（把教师数据分为四张表，分为四个方法进行） School_Teachers
        /// </summary>
        /// <param name="oldDb"></param>
        /// <param name="newData"></param>
        public void ProcessSchoolTeacherInfo(EFContext db, NHModel newDb)
        {
            //获取学校的数据集
            var newData = newDb.Set<TeacherInfo>().AsNoTracking().Select(p => new TeacherMoudle
            {
                Id = p.teacherId,
                Name = p.teacherName,
                UserId = p.teacherId,
                OrganId = p.orgId,
                JobNumber = p.teacherNo,
                MobilePhone = p.teacherPhone,
                CredType = p.certificateType,
                CredNumber = p.certificateNo,
                FacePhoto = p.ImgUri,
                Gender = p.sex ? "1" : "0"
            }).ToList();

            //获取本地生产环境数据集
            var oldData = db.Set<Teacher>().AsNoTracking().Select(p => new TeacherMoudle
            {
                Id = p.Id,
                Name = p.Name,
                UserId = p.UserId,
                OrganId = p.OrganId,
                JobNumber = p.JobNumber,
                MobilePhone = p.MobilePhone,
                CredType = p.CredType,
                CredNumber = p.CredNumber,
                FacePhoto = p.FacePhoto,
                Gender = p.Gender
            }).ToList();
            var addList = newData.Except(oldData).ToList(); //取新数据对于生产环境数据的差集，这个结果就是添加或修改的数据集
            var ids = oldData.Select(p => p.Id).ToList();
            var InsertList = new List<Teacher>(); //批量新增数据集
            foreach (var tea in addList)
            {
                if (ids.Contains(tea.Id))
                {
                    db.Set<Teacher>().Where(p => p.Id.Equals(tea.Id)).Update(p => new Teacher
                    {
                        Name = tea.Name,
                        UserId = tea.UserId,
                        OrganId = tea.OrganId,
                        JobNumber = tea.JobNumber,
                        MobilePhone = tea.MobilePhone,
                        CredType = tea.CredType,
                        CredNumber = tea.CredNumber,
                        Gender = tea.Gender
                    });
                }
                else
                {
                    InsertList.Add(new Teacher() {
                        Id = tea.Id,
                        Name = tea.Name,
                        UserId = tea.UserId,
                        OrganId = tea.OrganId,
                        JobNumber = tea.JobNumber,
                        MobilePhone = tea.MobilePhone,
                        CredType = tea.CredType,
                        CredNumber = tea.CredNumber,
                        FacePhoto = tea.FacePhoto,
                        Gender = tea.Gender
                    });
                }
            }
            db.Set<Teacher>().AddRange(InsertList);
            db.SaveChanges();
        }

        /// <summary>
        /// 简化程序（把教师数据分为四张表，分为四个方法进行） Sys_User
        /// </summary>
        /// <param name="db"></param>
        /// <param name="newData"></param>
        public void ProcessSchoolTeacherSysUser(EFContext db, NHModel newDb)
        {
            var newData = newDb.Set<TeacherInfo>().AsNoTracking().Select(p => new UserMoudle
            {
                Id = p.teacherId,
                Name = p.teacherName,
                Account = p.LoginId,
                OrganId = p.orgId,
                MobilePhone = p.teacherPhone,
                HeadIcon = p.ImgUri,
                Gender = p.sex ? "1" : "0"
            }).ToList();
            var oldData = db.Set<User>().AsNoTracking().Select(p => new UserMoudle
            {
                Id = p.Id,
                Name = p.Name,
                Account = p.Account,
                OrganId = p.OrganId,
                MobilePhone = p.MobilePhone,
                HeadIcon = p.HeadIcon,
                Gender = p.Gender==true ? "1" : "0"
            }).ToList();
            var addList = newData.Except(oldData).ToList();
            var Ids = oldData.Select(p => p.Id).ToList();
            var InsertList = new List<User>();
            foreach (var tea in addList)
            {
                if (Ids.Contains(tea.Id))
                {
                    db.Set<User>().Where(p => p.Id.Equals(tea.Id)).Update(p => new User
                    {
                        Name = tea.Name,
                        Account = tea.Account,
                        OrganId = tea.OrganId,
                        MobilePhone = tea.MobilePhone,
                        DutyId = "teacherDuty",
                        Gender = tea.Gender == "1" ? true : false
                    });
                }
                else
                {
                    InsertList.Add(new User() {
                        DutyId = "teacherDuty",
                        Id = tea.Id,
                        Name = tea.Name,
                        Account = tea.Account,
                        OrganId = tea.OrganId,
                        MobilePhone = tea.MobilePhone,
                        HeadIcon = tea.HeadIcon,
                        Gender = tea.Gender == "1" ? true : false
                    });
                }
            }
            db.Set<User>().AddRange(InsertList);
            db.SaveChanges();
        }

        /// <summary>
        ///  教师角色表
        /// </summary>
        /// <param name="db"></param>
        /// <param name="newDb"></param>
        public void ProcessSchoolTeacherSysUserRole(EFContext db, NHModel newDb)
        {
            var newData = newDb.Set<TeacherInfo>().AsNoTracking().Select(p => p.teacherId).ToList();
            var oldData = db.Set<UserRole>().AsNoTracking().Select(p => p.UserId).ToList();
            var AddData = newData.Except(oldData).ToList();
            var ListData = new List<UserRole>();
            if(null != AddData && AddData.Count() > 0)
            {
                foreach(var s in AddData)
                {
                    var r = new UserRole();
                    r.UserId = s;
                    r.RoleId = "teacher";
                    ListData.Add(r);
                }
            }
            db.Set<UserRole>().AddRange(ListData);
            db.SaveChanges();
        }

        /// <summary>
        /// 简化程序（把学生数据分为六张表，分为六个方法进行） School_Students
        /// </summary>
        /// <param name="oldDb"></param>
        /// <param name="newDb"></param>
        public void ProcessSchoolStudentInfo(EFContext oldDb, NHModel newDb)
        {
            var newData = newDb.Set<StudentInfo>().AsNoTracking().Select(p => new StudentMoudle
            {
                Id = p.studentId,
                Name = p.studentName,
                UserId = p.studentId,
                StudentNumber = p.studentNo,
                ClassId = p.studentClass,
                Gender = p.studentSex,
                CredType = p.certificateType,
                CredNumber = p.certificateNo,
                FacePic = p.ImgUri,
                MobilePhone = p.studentPhone
            }).ToList();
            var oldData = oldDb.Set<Student>().Select(p => new StudentMoudle
            {
                Id = p.Id,
                Name = p.Name,
                UserId = p.UserId,
                StudentNumber = p.StudentNumber,
                ClassId = p.ClassId,
                Gender = p.Gender,
                CredType = p.CredType,
                CredNumber = p.CredNumber,
                FacePic = p.FacePic,
                MobilePhone = p.MobilePhone
            }).ToList();
            var Ids = oldData.Select(p => p.Id).ToList();
            var DataList = newData.Except(oldData).ToList();
            var insertList = new List<Student>();
            var SqlUpdateList = oldDb.Set<Student>().ToList();
            var ProUpdateList = DataList.Where(p => Ids.Contains(p.Id)).ToList();
            var i = 0;
            foreach (var student in SqlUpdateList)
            {
                i++;
                var stu = ProUpdateList.Where(s => s.Id.Equals(student.Id)).FirstOrDefault();
                if(stu == null)
                {
                    continue;
                }
                var bbb = i;
                student.Id = stu.Id;
                student.Name = stu.Name;
                student.UserId = stu.UserId;
                student.StudentNumber = stu.StudentNumber;
                student.ClassId = stu.ClassId;
                student.Gender = stu.Gender;
                student.CredType = stu.CredType;
                student.CredNumber = stu.CredNumber;
                student.MobilePhone = stu.MobilePhone;
                student.GradeId = oldDb.Set<Org>().AsNoTracking().Where(p => p.Id.Equals(stu.ClassId)).Select(p => p.ParentId).FirstOrDefault();
                student.DivisId = oldDb.Set<Org>().AsNoTracking().Where(p => p.Id.Equals(stu.GradeId)).Select(p => p.ParentId).FirstOrDefault();
            }
            oldDb.SaveChanges();
            DataList = DataList.Where(p => !Ids.Contains(p.Id)).ToList();
            foreach (var stu in DataList)
            {
                //if (Ids.Contains(stu.Id))
                //{
                //    stu.GradeId = oldDb.Set<Organ>().AsNoTracking().Where(p => p.Id.Equals(stu.ClassId)).Select(p => p.ParentId).ToList().FirstOrDefault();
                //    stu.DivisId = oldDb.Set<Organ>().AsNoTracking().Where(p => p.Id.Equals(stu.GradeId)).Select(p => p.ParentId).ToList().FirstOrDefault();
                //    UpdateList.Add(new Student() {
                //        Id = stu.Id,
                //        Name = stu.Name,
                //        UserId = stu.UserId,
                //        StudentNumber = stu.StudentNumber,
                //        ClassId = stu.ClassId,
                //        Gender = stu.Gender,
                //        CredType = stu.CredType,
                //        CredNumber = stu.CredNumber,
                //        MobilePhone = stu.MobilePhone,
                //        GradeId = stu.GradeId,
                //        DivisId = stu.DivisId
                //    });
                    //oldDb.Set<Student>().Where(p => p.Id.Equals(stu.Id)).UpdateAsync(p => new Student
                    //{
                    //    Id = stu.Id,
                    //    Name = stu.Name,
                    //    UserId = stu.UserId,
                    //    StudentNumber = stu.StudentNumber,
                    //    ClassId = stu.ClassId,
                    //    Gender = stu.Gender,
                    //    CredType = stu.CredType,
                    //    CredNumber = stu.CredNumber,
                    //    MobilePhone = stu.MobilePhone,
                    //    GradeId = stu.GradeId,
                    //    DivisId = stu.DivisId
                    //});

                    //var data = oldDb.Set<Student>().Where(p => p.Id.Equals(stu.Id)).ToList();
                    //foreach(var student in data)
                    //{
                    //    student.Id = stu.Id;
                    //    student.Name = stu.Name;
                    //    student.UserId = stu.UserId;
                    //    student.StudentNumber = stu.StudentNumber;
                    //    student.ClassId = stu.ClassId;
                    //    student.Gender = stu.Gender;
                    //    student.CredType = stu.CredType;
                    //    student.CredNumber = stu.CredNumber;
                    //    student.MobilePhone = stu.MobilePhone;
                    //    student.GradeId = stu.GradeId;
                    //    student.DivisId = stu.DivisId;
                    //}
                    //oldDb.SaveChanges();
                //}
                //else
                //{
                    insertList.Add(new Student() {
                        Id = stu.Id,
                        Name = stu.Name,
                        UserId = stu.UserId,
                        StudentNumber = stu.StudentNumber,
                        ClassId = stu.ClassId,
                        Gender = stu.Gender,
                        CredType = stu.CredType,
                        CredNumber = stu.CredNumber,
                        MobilePhone = stu.MobilePhone,
                        FacePic = stu.FacePic,
                        GradeId = oldDb.Set<Org>().AsNoTracking().Where(p => p.Id.Equals(stu.ClassId)).Select(p => p.ParentId).FirstOrDefault(),
                        DivisId =  oldDb.Set<Org>().AsNoTracking().Where(p => p.Id.Equals(stu.GradeId)).Select(p => p.ParentId).FirstOrDefault()
            });
                //}
            }
            oldDb.Set<Student>().AddRange(insertList);
            oldDb.SaveChanges();
        }

        /// <summary>
        /// 简化程序（把学生数据分为六张表，分为六个方法进行） Sys_User
        /// </summary>
        /// <param name="db"></param>
        /// <param name="newDb"></param>
        public void ProcessSchoolStudentSysUser(EFContext db, NHModel newDb)
        {
            var newData = newDb.Set<StudentInfo>().AsNoTracking().Select(p => new UserMoudle
            {
                Id = p.studentId,
                Name = p.studentName,
                Account = p.LoginId,
                OrganId = p.orgId,
                MobilePhone = p.studentPhone,
                HeadIcon = p.ImgUri
            }).ToList();
            var oldData = db.Set<User>().AsNoTracking().Select(p => new UserMoudle
            {
                Id = p.Id,
                Name = p.Name,
                Account = p.Account,
                OrganId = p.OrganId,
                MobilePhone = p.MobilePhone,
                HeadIcon = p.HeadIcon
            }).ToList();
            var Ids = oldData.Select(p => p.Id).ToList();
            var DataList = newData.Except(oldData).ToList();
            var insertList = new List<User>();

            var SqlUpdateList = db.Set<User>().ToList();
            foreach(var sql in SqlUpdateList)
            {
                var stu = DataList.Where(p => p.Id.Equals(sql.Id)).FirstOrDefault();
                if(stu == null)
                {
                    continue;
                }
                sql.Name = sql.Name;
                sql.Account = sql.Account;
                sql.OrganId = sql.OrganId;
                sql.MobilePhone = sql.MobilePhone;
                sql.DutyId = "studentDuty";
            }

            DataList = DataList.Where(p => !Ids.Contains(p.Id)).ToList();
            foreach (var sysUser in DataList)
            {
                //if (Ids.Contains(sysUser.Id))
                //{
                //    db.Set<User>().Where(p => p.Id.Equals(sysUser.Id)).Update(p => new User
                //    {
                //        Name = sysUser.Name,
                //        Account = sysUser.Account,
                //        OrganId = sysUser.OrganId,
                //        MobilePhone = sysUser.MobilePhone,
                //        DutyId = "studentDuty"
                //    });
                //}
                //else
                //{
                    insertList.Add(new User() {
                        Id = sysUser.Id,
                        Name = sysUser.Name,
                        Account = sysUser.Account,
                        OrganId = sysUser.OrganId,
                        MobilePhone = sysUser.MobilePhone,
                        HeadIcon = sysUser.HeadIcon,
                        DutyId = "studentDuty"
                    });
                //}
            }
            db.Set<User>().AddRange(insertList);
            db.SaveChanges();
        }

        /// <summary>
        /// 简化程序（把学生数据分为六张表，分为六个方法进行） Dorm_Dorm
        /// </summary>
        /// <param name="db"></param>
        /// <param name="newDb"></param>
        public void ProcessSchoolStudentDormInfo(EFContext db, NHModel newDb)
        {
            var newData = newDb.Set<StudentInfo>().AsNoTracking().Select(p => new DormStudentMoudle {
                StudentId = p.studentId,
                Description = p.studentBuildingId,
                Gender = p.studentSex
            }).ToList();
           
            //添加到 zhxy_Building表
            var BuildData = newData.Where(p => p.Description != null && p.Description.Contains("栋")).Select(p => p.Description.Split('栋')[0].Replace(" ", "").Replace("海院A", "海A").Replace("海院B", "海B")).Distinct().ToList(); //新增至宿舍楼栋表（dorm_building）
            var oldBuildData = db.Set<Building>().AsNoTracking().Select(p => p.BuildingNo).ToList();
            var AddBuild = BuildData.Except(oldBuildData).ToList();
            if(null != AddBuild && AddBuild.Count() > 0)
            {
                var add = new List<Building>();
                foreach(var s in AddBuild)
                {
                    var b = new Building();
                    b.BuildingNo = s;
                    add.Add(b);
                }
                db.Set<Building>().AddRange(add);
                db.SaveChanges();
            }

            //添加到 zhxy_dorm 表
            var NewRoomTitle = newData.Where(p => p.Description != null && p.Description.Contains("栋")).Select(p => p.Description).Distinct().ToList();
            var RoomTitle = db.Set<DormRoom>().AsNoTracking().Select(p => p.Building.BuildingNo + "栋" + p.RoomNumber).ToList();
            var AddRoomInfo = NewRoomTitle.Except(RoomTitle).ToList();
            if(AddRoomInfo != null && AddRoomInfo.Count() > 0)
            {
                var AddDorm = new List<DormRoom>();
                foreach(var title in AddRoomInfo)
                {
                    var BuildNo = title.Split('栋');
                    if(BuildNo != null && BuildNo.Count() == 2)
                    {
                        var d = new DormRoom();
                        var build = BuildNo[0];
                        d.BuildingId = db.Set<Building>().AsNoTracking().Where(p => p.BuildingNo.Equals(build)).Select(s => s.Id).FirstOrDefault();
                        d.FloorNumber = BuildNo[1].Replace(BuildNo[1].Substring(1), "");
                        d.RoomNumber = BuildNo[1];
                        d.Title = title;
                        AddDorm.Add(d);
                    }
                }
                db.Set<DormRoom>().AddRange(AddDorm);
                db.SaveChanges();
            }

            //添加到zhxy_dorm_student 表
            var ListDataNew = new List<DormStudentMoudle>();
            foreach(var n in newData)
            {
                var D = new DormStudentMoudle();
                D.DormId = db.Set<DormRoom>().AsNoTracking().Where(p => p.Title.Equals(n.Description)).Select(p => p.Id).FirstOrDefault();
                D.StudentId = n.StudentId;
                D.Gender = n.Gender;
                ListDataNew.Add(D);
            }
            var ListDataOld = db.Set<DormStudent>().AsNoTracking().Select(p => new DormStudentMoudle { DormId = p.DormId, StudentId = p.StudentId, Gender = p.Gender}).ToList();
            var AddList = ListDataNew.Except(ListDataOld).ToList();
            var FinalAddList = new List<DormStudent>();
            foreach(var s in AddList)
            {
                var StudentIds = ListDataOld.Select(p => p.StudentId).ToList();
                if (StudentIds.Contains(s.StudentId))
                {
                    db.Set<DormStudent>().Where(p => p.StudentId.Equals(s.StudentId)).Update(p => new DormStudent
                    {
                        DormId = s.DormId,
                        Gender = s.Gender
                    });
                }
                else
                {
                    FinalAddList.Add(new DormStudent() {
                        DormId = s.DormId,
                        Gender = s.Gender,
                        StudentId = s.StudentId
                    });
                }
            }
            db.Set<DormStudent>().AddRange(FinalAddList);
            db.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="newDb"></param>
        public void ProcessStudentSysUserRole(EFContext db, NHModel newDb)
        {
            var newData = newDb.Set<StudentInfo>().AsNoTracking().Select(p => p.studentId).ToList();
            var oldData = db.Set<UserRole>().AsNoTracking().Select(p => p.UserId).ToList();
            var AddData = newData.Except(oldData).ToList();
            var ListData = new List<UserRole>();
            if (null != AddData && AddData.Count() > 0)
            {
                foreach (var s in AddData)
                {
                    var r = new UserRole();
                    r.UserId = s;
                    r.RoleId = "student";
                    ListData.Add(r);
                }
            }
            db.Set<UserRole>().AddRange(ListData);
            db.SaveChanges();
        }

        public static PersonMoudle contactMoudleStudent(EFContext oldDb, string id)
        {
            var sql = "SELECT NULL\n" +
                "\taccessCardsn,\n" +
                "\ta.F_StudentNum code,\n" +
                "\ta.F_Class_ID colleageClass,\n" +
                "\ta.F_Divis_ID colleageCode,\n" +
                "\tNULL colleageGrade,\n" +
                "\ta.F_Grade_ID colleageMajor,\n" +
                "\ta.F_Tel contactNum,\n" +
                "\tb.F_Bed_ID dormitoryBed,\n" +
                "\tc.F_Building_No dormitoryCode,\n" +
                "\tc.F_Floor_No dormitoryFloor,\n" +
                "\tc.F_Title dormitoryRoom,\n" +
                "\ta.F_CredNum idCode,\n" +
                "\ta.F_Name name,\n" +
                "\tNULL orgId,\n" +
                "\tNULL pageNum,\n" +
                "\tNULL pageSize,\n" +
                "\tNULL param,\n" +
                "\tNULL rfidCardsn,\n" +
                "\tNULL roleId,\n" +
                "\ta.F_Gender sex,\n" +
                "\tNULL colleageArea,\n" +
                "\tNULL dormitoryArea,\n" +
                "\tc.F_Leader_Name dormitoryLeader,\n" +
                "\td.F_Tel dormitoryLeaderPhone,\n" +
                "\tNULL emergencyPerson,\n" +
                "\tNULL emergencyPersonPhone,\n" +
                "\tNULL instructorCode,\n" +
                "\tNULL instructorName,\n" +
                "\tNULL instructorPhone,\n" +
                "\tNULL managementClassList,\n" +
                "\tNULL photoBase64,\n" +
                "\ta.F_FacePic_File photoUrl,\n" +
                "\tNULL resume \n" +
                "FROM\n" +
                "\t[dbo].[School_Students] a\n" +
                "\tLEFT JOIN Dorm_DormStudent b ON a.F_Id = b.F_Student_ID\n" +
                "\tLEFT JOIN Dorm_Dorm c ON b.F_DormId = c.F_Id\n" +
                "\tLEFT JOIN School_Students d ON c.F_Leader_ID = d.F_Id \n" +
                "WHERE\n" +
                "\ta.F_DeleteMark IS NULL \n" +
                "\tAND c.F_DeleteMark IS NULL \n" +
                "\tAND a.F_Id = '" + id + "'";
            return oldDb.Database.SqlQuery<PersonMoudle>(sql).FirstOrDefault();
        }

        public static PersonMoudle contackMoudleTeacher(EFContext oldDb, string id)
        {
            var sql = "SELECT \n" +
                "       NULL accessCardsn,\n" +
                "        a.F_Num code,\n" +
                "        NULL colleageClass,\n" +
                "        NULL colleageCode,\n" +
                "        NULL colleageGrade,\n" +
                "        NULL colleageMajor,\n" +
                "        a.F_MobilePhone contactNum,\n" +
                "        NULL dormitoryBed,\n" +
                "        NULL dormitoryCode,\n" +
                "        NULL dormitoryFloor,\n" +
                "        NULL dormitoryRoom,\n" +
                "        a.F_CredNum idCode,\n" +
                "        a.F_Name name,\n" +
                "        a.F_Divis_ID orgId,\n" +
                "        NULL pageNum,\n" +
                "        NULL pageSize,\n" +
                "        NULL param,\n" +
                "        NULL rfidCardsn,\n" +
                "        NULL roleId,\n" +
                "        a.F_Gender sex,\n" +
                "        NULL colleageArea,\n" +
                "        NULL dormitoryArea,\n" +
                "        NULL dormitoryLeader,\n" +
                "        NULL dormitoryLeaderPhone,\n" +
                "        NULL emergencyPerson,\n" +
                "        NULL emergencyPersonPhone,\n" +
                "        NULL instructorCode,\n" +
                "        NULL instructorName,\n" +
                "        NULL instructorPhone,\n" +
                "        NULL managementClassList,\n" +
                "        NULL photoBase64,\n" +
                "        a.F_FacePhoto photoUrl,\n" +
                "        NULL resume\n" +
                "FROM\n" +
                "        [dbo].[School_Teachers] a\n" +
                "WHERE\n" +
                "        a.F_DeleteMark is null\n" +
                "        AND a.F_Id = '" + id + "'";
            return oldDb.Database.SqlQuery<PersonMoudle>(sql).FirstOrDefault();
        }

        public static void downImage()
        {
            var model = new NHModel();
            var stuList = model.StudentInfo.Where(p => p.studentBuildingId.Contains("12栋") || p.studentBuildingId.Contains("11栋")).ToList();
            var dt = new DataTable();
            var dc1 = new DataColumn("姓名", Type.GetType("System.String"));
            var dc2 = new DataColumn("性别", Type.GetType("System.String"));
            var dc3 = new DataColumn("学工号", Type.GetType("System.String"));
            var dc4 = new DataColumn("所属组织", Type.GetType("System.String"));
            var dc5 = new DataColumn("身份证号", Type.GetType("System.String"));
            var dc6 = new DataColumn("类别", Type.GetType("System.String"));
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc3);
            dt.Columns.Add(dc4);
            dt.Columns.Add(dc5);
            dt.Columns.Add(dc6);
            foreach (var d in stuList)
            {
                //string filepath = System.AppDomain.CurrentDomain.BaseDirectory;
                var fileName = "E:\\img\\" + d.studentNo + ".png";
                var t = GetImageBase64Str.DownLoadPic(d.ImgUri, fileName);
                if (t)
                {
                    var dr = dt.NewRow();
                    dr["姓名"] = d.studentName;
                    dr["性别"] = d.studentSex == "0" ? "女" : "男";
                    dr["学工号"] = d.studentNo;
                    dr["所属组织"] = "南昌航空大学";
                    dr["身份证号"] = d.certificateNo;
                    dr["类别"] = "学生";
                    dt.Rows.Add(dr);
                }
            }
        }

        public static void process()
        {
            Console.WriteLine("------------批量同步11栋和12栋学生数据，到12栋1楼101室");
            //批量同步11栋和12栋学生数据，到12栋1楼101室
            var model = new NHModel();
            var stuList = model.StudentInfo.Where(p => p.studentBuildingId.Contains("12栋") || p.studentBuildingId.Contains("11栋")).Select(p => new PersonMoudle
            {
                orgId = "org001",
                code = p.studentNo,
                idCode = p.certificateNo,
                name = p.studentName,
                roleId = "student001", //teacher001
                sex = 1,
                colleageCode = "55f67dcc42a5426fb0670d58dda22a5b", //默认分院
                dormitoryCode = "fe8a5225be5f43478d0dd0c85da5dd1d",//楼栋  例如： 11栋
                dormitoryFloor = "8e447843bc8c4e92b9ffdf777047d20d", //楼层  例如：3楼
                dormitoryRoom = "20c70f65b54b4f96851e26343678c4ec", //宿舍号  例如：312
                photoUrl = p.ImgUri
            }).ToList();

            foreach (var person in stuList)
            {
                try
                {
                    Console.WriteLine(person.name);
                    var d = DHAccount.PUSH_DH_ADD_PERSON(person);
                    Console.WriteLine(d.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            Console.WriteLine("-------------已退出");
        }
    }

    public class OrganMoudle : IEquatable<OrganMoudle>
    {
        public string Id { get; internal set; }
        public string Name { get; internal set; }
        public string ParentId { get; internal set; }
        public string EnCode { get; internal set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as OrganMoudle);
        }

        public bool Equals(OrganMoudle other)
        {
            return other != null &&
                   Id == other.Id &&
                   Name == other.Name &&
                   ParentId == other.ParentId &&
                   EnCode == other.EnCode;
        }

        public override int GetHashCode()
        {
            var hashCode = -1783685579;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ParentId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EnCode);
            return hashCode;
        }

        public static bool operator ==(OrganMoudle moudle1, OrganMoudle moudle2)
        {
            return EqualityComparer<OrganMoudle>.Default.Equals(moudle1, moudle2);
        }

        public static bool operator !=(OrganMoudle moudle1, OrganMoudle moudle2)
        {
            return !(moudle1 == moudle2);
        }
    }

    public class TeacherMoudle : IEquatable<TeacherMoudle>
    {
        public string Id { get; internal set; }
        public string Name { get; internal set; }
        public string UserId { get; internal set; }
        public string OrganId { get; internal set; }
        public string JobNumber { get; internal set; }
        public string MobilePhone { get; internal set; }
        public string CredType { get; internal set; }
        public string CredNumber { get; internal set; }
        public string FacePhoto { get; internal set; }
        public string Gender { get; internal set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as TeacherMoudle);
        }

        public bool Equals(TeacherMoudle other)
        {
            return other != null &&
                   Id == other.Id &&
                   Name == other.Name &&
                   UserId == other.UserId &&
                   OrganId == other.OrganId &&
                   JobNumber == other.JobNumber &&
                   MobilePhone == other.MobilePhone &&
                   CredType == other.CredType &&
                   CredNumber == other.CredNumber &&
                   FacePhoto == other.FacePhoto &&
                   Gender == other.Gender;
        }

        public override int GetHashCode()
        {
            var hashCode = -1613071421;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(UserId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(OrganId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(JobNumber);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(MobilePhone);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CredType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CredNumber);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FacePhoto);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Gender);
            return hashCode;
        }

        public static bool operator ==(TeacherMoudle moudle1, TeacherMoudle moudle2)
        {
            return EqualityComparer<TeacherMoudle>.Default.Equals(moudle1, moudle2);
        }

        public static bool operator !=(TeacherMoudle moudle1, TeacherMoudle moudle2)
        {
            return !(moudle1 == moudle2);
        }
    }

    public class UserMoudle : IEquatable<UserMoudle>
    {
        public string Id { get; internal set; }
        public string Name { get; internal set; }
        public string Account { get; internal set; }
        public string OrganId { get; internal set; }
        public string MobilePhone { get; internal set; }
        public string HeadIcon { get; internal set; }
        public string Gender { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as UserMoudle);
        }

        public bool Equals(UserMoudle other)
        {
            return other != null &&
                   Id == other.Id &&
                   Name == other.Name &&
                   Account == other.Account &&
                   OrganId == other.OrganId &&
                   MobilePhone == other.MobilePhone &&
                   HeadIcon == other.HeadIcon &&
                   Gender == other.Gender;
        }

        public override int GetHashCode()
        {
            var hashCode = -679124400;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Account);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(OrganId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(MobilePhone);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(HeadIcon);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Gender);
            return hashCode;
        }

        public static bool operator ==(UserMoudle moudle1, UserMoudle moudle2)
        {
            return EqualityComparer<UserMoudle>.Default.Equals(moudle1, moudle2);
        }

        public static bool operator !=(UserMoudle moudle1, UserMoudle moudle2)
        {
            return !(moudle1 == moudle2);
        }
    }

    public class StudentMoudle : IEquatable<StudentMoudle>
    {
        public string Id { get; internal set; }
        public string Name { get; internal set; }
        public string UserId { get; internal set; }
        public string StudentNumber { get; internal set; }
        public string ClassId { get; internal set; }
        public string Gender { get; internal set; }
        public string CredType { get; internal set; }
        public string CredNumber { get; internal set; }
        public string FacePic { get; internal set; }
        public string MobilePhone { get; internal set; }
        public string GradeId { get; set; }
        public string DivisId { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as StudentMoudle);
        }

        public bool Equals(StudentMoudle other)
        {
            return other != null &&
                   Id == other.Id &&
                   Name == other.Name &&
                   UserId == other.UserId &&
                   StudentNumber == other.StudentNumber &&
                   ClassId == other.ClassId &&
                   Gender == other.Gender &&
                   CredType == other.CredType &&
                   CredNumber == other.CredNumber &&
                   FacePic == other.FacePic &&
                   MobilePhone == other.MobilePhone &&
                   GradeId == other.GradeId &&
                   DivisId == other.DivisId;
        }

        public override int GetHashCode()
        {
            var hashCode = -1882642292;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(UserId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(StudentNumber);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ClassId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Gender);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CredType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CredNumber);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FacePic);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(MobilePhone);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(GradeId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DivisId);
            return hashCode;
        }

        public static bool operator ==(StudentMoudle moudle1, StudentMoudle moudle2)
        {
            return EqualityComparer<StudentMoudle>.Default.Equals(moudle1, moudle2);
        }

        public static bool operator !=(StudentMoudle moudle1, StudentMoudle moudle2)
        {
            return !(moudle1 == moudle2);
        }
    }

    public class DormStudentMoudle : IEquatable<DormStudentMoudle>
    {
        public string DormId { get; internal set; }
        public string StudentId { get; internal set; }
        public string Gender { get; internal set; }
        public string Description { get; internal set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as DormStudentMoudle);
        }

        public bool Equals(DormStudentMoudle other)
        {
            return other != null &&
                   DormId == other.DormId &&
                   StudentId == other.StudentId &&
                   Gender == other.Gender &&
                   Description == other.Description;
        }

        public override int GetHashCode()
        {
            var hashCode = -427861170;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DormId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(StudentId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Gender);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            return hashCode;
        }

        public static bool operator ==(DormStudentMoudle moudle1, DormStudentMoudle moudle2)
        {
            return EqualityComparer<DormStudentMoudle>.Default.Equals(moudle1, moudle2);
        }

        public static bool operator !=(DormStudentMoudle moudle1, DormStudentMoudle moudle2)
        {
            return !(moudle1 == moudle2);
        }
    }
}
