using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskApi.NanHang;
using System.Diagnostics;
//using Z.EntityFramework.Plus;

using ZHXY.Common;
using ZHXY.Dorm.Device.DH;
using ZHXY.Dorm.Device.tools;
using EntityFramework.Extensions;
using System.Data;

namespace TaskApi.Job
{
    public class NanHangJob : IJob
    {
        private ILog Logger { get; } = LogManager.GetLogger(typeof(NanHangJob));
        public void Execute(IJobExecutionContext context)
        {
            //downImage();
            //return;
            Console.WriteLine();
            Console.WriteLine("************************************        开始同步南航师生信息       ************************************");
            Console.WriteLine();
            ProcessOrgInfo(); //同步组织机构信息
            Console.WriteLine();
            ProcessOrgInfoStu(); //同步学生组织机构信息
            Console.WriteLine();
            ProcessSysOrgan();//处理sys_organization表的相关等级标识
            Console.WriteLine();
            ProcessTeacher(); //同步教师信息
            Console.WriteLine();
            ProcessStudent(); //同步学生信息
            Console.WriteLine("************************************        同步南航师生信息结束       ************************************");
            Console.WriteLine();
        }
        public static void downImage()
        {
            var model = new NHModel();
            var stuList = model.StudentInfoes.Where(p => p.studentBuildingId.Contains("12栋") || p.studentBuildingId.Contains("11栋")).ToList();
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
                string fileName = "E:\\img\\" + d.studentNo+".png";
                bool t= GetImageBase64Str.DownLoadPic(d.ImgUri, fileName);
                if (t)
                {
                    var dr = dt.NewRow();
                    dr["姓名"] = d.studentName;
                    dr["性别"] = d.studentSex=="0"?"女":"男";
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
            var stuList = model.StudentInfoes.Where(p => p.studentBuildingId.Contains("12栋") || p.studentBuildingId.Contains("11栋")).Select(p => new PersonMoudle
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
                }catch(Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            Console.WriteLine("-------------已退出");
        }
        /// <summary>
        /// 处理同步学生信息 学生信息同步6张表
        /// 分别为： School_Students，Sys_User，Sys_User_Role，Sys_UserLogOn，Dorm_DormStudent，Dorm_Dorm
        /// </summary>
        public void ProcessStudent()
        {
            Console.WriteLine("南航项目：开始同步学生信息 --> " + DateTime.Now.ToLocalTime());
            var sw = new Stopwatch();
            sw.Start();

            var newDb = new NHModel();
            var oldDb = new NanHangAccept();

            ///---------      Step1: 更新 School_Students 表数据      ---------///
            ProcessSchoolStudentInfo(oldDb, newDb);
            Console.WriteLine(" *** 同步 School_Students 结束 。");
            ///---------      Step2: 更新 Sys_User 表数据      ---------///
            ProcessSchoolStudentSysUser(oldDb, newDb);
            Console.WriteLine(" *** 同步 Sys_User 结束 。");
            ///---------      Step3: 更新 Sys_User_Role 表数据      ---------///
            ProcessSchoolStudentSysUserRole(oldDb, newDb);
            Console.WriteLine(" *** 同步 Sys_User_Role 结束 。");
            ///---------      Step4: 更新 Sys_UserLogOn 表数据      ---------///
            ProcessSchoolStudentSysUserLogOn(oldDb, newDb);
            Console.WriteLine(" *** 同步 Sys_UserLogOn 结束 。");
            ///---------      Step5: 更新 Dorm_Dorm 表数据      ---------///
            ProcessSchoolStudentDormInfo(oldDb, newDb);
            Console.WriteLine(" *** 同步 Dorm_Dorm 结束 。");
            ///---------      Step6: 更新 Dorm_DormStudent 表数据      ---------///
            ProcessSchoolStudentSysUserDormStudent(oldDb, newDb);
            Console.WriteLine(" *** 同步 Dorm_DormStudent 结束 。");

            //newDb.BulkDelete(newDb.StudentInfoes.ToList()); //批量删除中间表的所有数据
            newDb.Dispose();
            oldDb.Dispose();
            sw.Stop();
            Console.WriteLine("南航项目：同步学生信息结束 --> 合计耗时：" + sw.ElapsedMilliseconds / 1000 + "s");
        }

        /// <summary>
        /// 处理同步教师信息
        /// 此方法分别更新四张表，分别是：School_Teachers，Sys_User，Sys_User_Role，Sys_UserLogOn
        /// </summary>
        public void ProcessTeacher()
        {
            Console.WriteLine("南航项目：开始同步教师信息 --> " + DateTime.Now.ToLocalTime());
            var sw = new Stopwatch();
            sw.Start();

            var newDb = new NHModel();
            var oldDb = new NanHangAccept();

            ///---------      Step1: 更新 School_Teachers 表数据      ---------///
            ProcessSchoolTeacherInfo(oldDb, newDb);
            ///---------      Step2: 更新 Sys_User 表数据      ---------///
            ProcessSchoolTeacherSysUser(oldDb, newDb);
            ///---------      Step3: 更新 Sys_User_Role 表数据      ---------///
            ProcessSchoolTeacherSysUserRole(oldDb, newDb);
            ///---------      Step4: 更新 Sys_UserLogOn 表数据      ---------///
            ProcessSchoolTeacherSysUserLogON(oldDb, newDb);

            //newDb.BulkDelete(newDb.TeacherInfoes.ToList()); //删除中间表的所有教师数据
            newDb.Dispose();
            oldDb.Dispose();
            sw.Stop();
            Console.WriteLine("南航项目：同步教师信息结束 --> 合计耗时：" + sw.ElapsedMilliseconds / 1000 + "s");
        }

        /// <summary>
        /// 处理同步组织机构信息
        /// </summary>
        public void ProcessOrgInfo()
        {
            Console.WriteLine("开始同步组织机构信息: 南航项目 --> " + DateTime.Now.ToLocalTime());
            var sw = new Stopwatch();
            sw.Start();
            //校方数据集(分为两种：修改数据和新增数据)
            var newDb = new NHModel();
            var newData = newDb.OrganizationInfoes.Select(p => new Sys_Organize { F_Id = p.OrgId, F_FullName = p.OrgName, F_ParentId = p.ParentOrgId, F_CreatorTime = p.CreatedTime, F_LastModifyTime = p.LastUpdatedTime, F_EnCode = p.OrgId, F_DeleteMark=false }).ToList();

            //获取生产环境数据库数据集
            var oldDb = new NanHangAccept();
            //var oldData = oldDb.Sys_Organize.Select(p => new Sys_Organize { F_Id = p.F_Id, F_FullName = p.F_FullName, F_ParentId = p.F_ParentId, F_CreatorTime = p.F_CreatorTime, F_LastModifyTime = p.F_LastModifyTime }).ToList();
            var oldData = oldDb.Sys_Organize.ToList();
            var interList = newData.Intersect(oldData).ToList(); //取交集 （不作任何操作）
            oldData = oldData.Except(interList).ToList(); //将交集从内存删除，以节省内存空间并减少循环次数
            newData = newData.Except(interList).ToList();
            var addList = newData.Except(oldData).ToList(); //取差集 （新增和修改数据）

            var idList = oldData.Select(p => p.F_Id).ToList();
            var endList = new List<Sys_Organize>();
            foreach(var org in addList)
            {
                if (org.F_ParentId == null){org.F_ParentId = "3";}
                if (idList.Contains(org.F_Id))
                {
                    oldDb.Set<Sys_Organize>().Where(p => p.F_Id == org.F_Id).Update(p => new Sys_Organize
                    {
                        F_ParentId = org.F_ParentId,
                        F_FullName = org.F_FullName,
                        F_CreatorTime = org.F_CreatorTime,
                        F_LastModifyTime = org.F_LastModifyTime
                    });
                }
                else
                {
                    endList.Add(org);
                }
            }
            //newDb.BulkDelete(newDb.OrganizationInfoes.ToList()); //操作完成后，删除取出来的数据
            //oldDb.BulkInsert(endList);
            oldDb.Sys_Organize.AddRange(endList);
            oldDb.SaveChanges();
            oldDb.Dispose();
            newDb.Dispose();
            sw.Stop();            
            Console.WriteLine("南航项目：同步组织机构信息结束 --> 合计耗时：" + sw.ElapsedMilliseconds / 1000 + "s");
        }

        /// <summary>
        /// 处理同步组织机构信息（学生组织结构）
        /// </summary>
        public void ProcessOrgInfoStu()
        {
            Console.WriteLine("开始同步组织机构信息: 南航项目 --> " + DateTime.Now.ToLocalTime());
            var sw = new Stopwatch();
            sw.Start();
            //校方数据集(分为两种：修改数据和新增数据)
            var newDb = new NHModel();
            var newData = newDb.OrganizationInfo_stu.Select(p => new Sys_Organize { F_Id = p.OrgId, F_FullName = p.OrgName, F_ParentId = p.ParentOrgId, F_CreatorTime = p.CreatedTime, F_LastModifyTime = p.LastUpdatedTime, F_EnCode=p.OrgId, F_DeleteMark=false }).ToList();

            //获取生产环境数据库数据集
            var oldDb = new NanHangAccept();
            //var oldData = oldDb.Sys_Organize.Select(p => new Sys_Organize { F_Id = p.F_Id, F_FullName = p.F_FullName, F_ParentId = p.F_ParentId, F_CreatorTime = p.F_CreatorTime, F_LastModifyTime = p.F_LastModifyTime }).ToList();
            var oldData = oldDb.Sys_Organize.ToList();
            var interList = newData.Intersect(oldData).ToList(); //取交集 （不作任何操作）
            oldData = oldData.Except(interList).ToList(); //将交集从内存删除，以节省内存空间并减少循环次数
            newData = newData.Except(interList).ToList();
            var addList = newData.Except(oldData).ToList(); //取差集 （新增和修改数据）

            var idList = oldData.Select(p => p.F_Id).ToList();
            var endList = new List<Sys_Organize>();
            foreach (var org in addList)
            {
                if (org.F_ParentId == null) { org.F_ParentId = "2"; }
                if (idList.Contains(org.F_Id))
                {
                    oldDb.Set<Sys_Organize>().Where(p => p.F_Id == org.F_Id).Update(p => new Sys_Organize
                    {
                        F_ParentId = org.F_ParentId,
                        F_FullName = org.F_FullName,
                        F_CreatorTime = org.F_CreatorTime,
                        F_LastModifyTime = org.F_LastModifyTime
                    });
                }
                else
                {
                    endList.Add(org);
                }
            }
            //newDb.BulkDelete(newDb.OrganizationInfo_stu.ToList()); //操作完成后，删除取出来的数据
            //oldDb.BulkInsert(endList);
            oldDb.Sys_Organize.AddRange(endList);
            oldDb.SaveChanges();
            oldDb.Dispose();
            newDb.Dispose();
            sw.Stop();
            Console.WriteLine("南航项目：同步组织机构信息结束 --> 合计耗时：" + sw.ElapsedMilliseconds / 1000 + "s");
        }

        /// <summary>
        /// 同步之后的表，添加各个登记的标识：
        /// </summary>
        public void ProcessSysOrgan()
        {
            var oldDb = new NanHangAccept();
            //修改学生年级的 F_CategoryId 为 "Division"
            string UpdateDivSql = "UPDATE organ2 set organ2.F_CategoryId='Division' from [dbo].[Sys_Organize] organ1  left join Sys_Organize organ2 on organ2.F_ParentId=organ1.F_id where organ1.F_ParentId='2'";
            //修改学生的学部（）
            string UpdateGradeSql = "UPDATE organ3 set organ3.F_CategoryId='Grade' from[dbo].[Sys_Organize] organ1 left join Sys_Organize organ2 on organ2.F_ParentId = organ1.F_id left join Sys_Organize organ3 on organ3.F_ParentId = organ2.F_id where organ1.F_ParentId = '2'";
            //修改学生的班级
            string UpdateClassSql = "UPDATE organ4 set organ4.F_CategoryId='Class' from[dbo].[Sys_Organize] organ1 left join Sys_Organize organ2 on organ2.F_ParentId = organ1.F_id left join Sys_Organize organ3 on organ3.F_ParentId = organ2.F_id left join Sys_Organize organ4 on organ4.F_ParentId = organ3.F_id where organ1.F_ParentId = '2'";

            oldDb.Database.ExecuteSqlCommand(UpdateDivSql);
            oldDb.Database.ExecuteSqlCommand(UpdateGradeSql);
            oldDb.Database.ExecuteSqlCommand(UpdateClassSql);
        }

        /// <summary>
        /// 简化程序（把教师数据分为四张表，分为四个方法进行） School_Teachers
        /// </summary>
        /// <param name="oldDb"></param>
        /// <param name="newData"></param>
        public void ProcessSchoolTeacherInfo(NanHangAccept oldDb, NHModel newDb)
        {
            //获取学校的数据集
            var newData = newDb.TeacherInfoes.Select(p => new School_Teachers
            {
                F_Id = p.teacherId,
                F_Name = p.teacherName,
                F_User_ID = p.teacherId,
                F_Divis_ID = p.orgId,
                F_Num = p.teacherNo,
                F_MobilePhone = p.teacherPhone,
                F_CredType = p.certificateType,
                F_CredNum = p.certificateNo,
                F_FacePhoto = p.ImgUri,
                F_Gender = p.Sex.ToString()
            }).ToList();

            //获取本地生产环境数据集
            var oldData = oldDb.School_Teachers.ToList();
            var addList = newData.Except(oldData).ToList(); //取新数据对于生产环境数据的差集，这个结果就是添加或修改的数据集
            var ids = oldData.Select(p => p.F_Id).ToList();
            var InsertList = new List<School_Teachers>(); //批量新增数据集
            foreach (var tea in addList)
            {
                if (ids.Contains(tea.F_Id))
                {
                    oldDb.Set<School_Teachers>().Where(p => p.F_Id == tea.F_Id).Update(p => new School_Teachers
                    {
                        F_Name = tea.F_Name,
                        F_User_ID = tea.F_User_ID,
                        F_Divis_ID = tea.F_Divis_ID,
                        F_Num = tea.F_Num,
                        F_MobilePhone = tea.F_MobilePhone,
                        F_CredType = tea.F_CredType,
                        F_CredNum = tea.F_CredNum,
                        //F_FacePhoto = tea.F_FacePhoto,
                        F_LastModifyTime = DateTime.Now,
                        F_Gender = tea.F_Gender,
                        F_DeleteMark = false,
                        F_EnabledMark = true
                    });
                    //修改大华闸机学生数据
                    //PersonMoudle personMoudle = contackMoudleTeacher(oldDb, tea.F_Id);
                    //personMoudle.roleId = "teacher001";
                    //DHAccount.PUSH_DH_UPDATE_PERSON(personMoudle);
                }
                else
                {
                    tea.F_CreatorTime = DateTime.Now;
                    tea.F_LastModifyTime = DateTime.Now;
                    tea.F_DeleteMark = false;
                    tea.F_EnabledMark = true;
                    InsertList.Add(tea);
                }
            }
            //oldDb.BulkInsert(InsertList);
            oldDb.School_Teachers.AddRange(InsertList);
            oldDb.SaveChanges();

            //修改sys_user表的主机id为自动生成的ID，并修改teacher表的sys_userID为sys_user表生成的主机id
            //string updateSql = "update  teacher set teacher.F_User_ID = sysUser.F_ID from sys_user sysUser, school_teacher teacher where sysUser.F_Account = teacher.F_User_ID";
            //SqlHelper.ExecuteNonQuery(updateSql);  

            //开始增量数据至大华闸机
            //foreach (School_Teachers teacher in InsertList)
            //{
            //    PersonMoudle personMoudle = contackMoudleTeacher(oldDb, teacher.F_Id);
            //    personMoudle.roleId = "teacher001";
            //    DHAccount.PUSH_DH_ADD_PERSON(personMoudle);
            //}
        }

        /// <summary>
        /// 简化程序（把教师数据分为四张表，分为四个方法进行） Sys_User
        /// </summary>
        /// <param name="oldDb"></param>
        /// <param name="newData"></param>
        public void ProcessSchoolTeacherSysUser(NanHangAccept oldDb, NHModel newDb)
        {
            var newData = newDb.TeacherInfoes.Select(p => new Sys_User
            {
                F_Id = p.teacherId,
                F_RealName = p.teacherName,
                F_Account = p.LoginId,
                F_OrganizeId = p.orgId,
                F_MobilePhone = p.teacherPhone,
                F_CreatorTime = DateTime.Now,
                F_DepartmentId = p.orgId,
                F_HeadIcon = p.ImgUri
            });
            var oldData = oldDb.Sys_User.ToList();
            var addList = newData;
            var Ids = oldData.Select(p => p.F_Id).ToList();
            var InsertList = new List<Sys_User>();
            foreach(var tea in addList)
            {
                tea.F_RoleId = "teacher";
                tea.F_DutyId = "teacherDuty";
                tea.F_CreatorTime = DateTime.Now;
                tea.F_DeleteMark = false;
                tea.F_EnabledMark = true;
                if (Ids.Contains(tea.F_Id))
                {
                    oldDb.Set<Sys_User>().Where(p => p.F_Id == tea.F_Id).Update(p => new Sys_User
                    {
                        F_RealName = tea.F_RealName,
                        F_Account = tea.F_Account,
                        F_OrganizeId = tea.F_OrganizeId,
                        F_MobilePhone = tea.F_MobilePhone,
                        F_RoleId = tea.F_RoleId,
                        F_DutyId = tea.F_DutyId,
                        F_CreatorTime = tea.F_CreatorTime,
                        F_DeleteMark = false,
                        F_EnabledMark = true,
                        F_DepartmentId = tea.F_DepartmentId
                    });
                }
                else
                {
                    InsertList.Add(tea);
                }
            }
            //oldDb.BulkInsert(InsertList);
            oldDb.Sys_User.AddRange(InsertList);
            oldDb.SaveChanges();
        }

        /// <summary>
        /// 简化程序（把教师数据分为四张表，分为四个方法进行） Sys_User_Role
        /// </summary>
        /// <param name="oldDb"></param>
        /// <param name="newDb"></param>
        public void ProcessSchoolTeacherSysUserRole(NanHangAccept oldDb, NHModel newDb)
        {
            var newData = newDb.TeacherInfoes.Select(p => new Sys_User_Role
            {
                F_User = p.teacherId
            });

            var oldData = oldDb.Sys_User_Role.ToList();
            var addList = newData;
            var Ids = oldData.Select(p => p.F_User).ToList();
            var InsertList = new List<Sys_User_Role>();
            foreach(var userRole in addList)
            {
                if (!Ids.Contains(userRole.F_User))
                {
                    userRole.F_Role = "teacher";
                    userRole.F_Id = Guid.NewGuid().ToString();
                    userRole.F_CreatorTime = DateTime.Now;
                    userRole.F_DeleteMark = false;
                    userRole.F_EnabledMark = true;
                    InsertList.Add(userRole);
                }
            }
            //oldDb.BulkInsert(InsertList);
            oldDb.Sys_User_Role.AddRange(InsertList);
            oldDb.SaveChanges();
        }

        /// <summary>
        /// 简化程序（把教师数据分为四张表，分为四个方法进行） Sys_UserLogOn
        /// </summary>
        /// <param name="oldDb"></param>
        /// <param name="newDb"></param>
        public void ProcessSchoolTeacherSysUserLogON(NanHangAccept oldDb, NHModel newDb)
        {
            var newData = newDb.TeacherInfoes.Select(p => new Sys_UserLogOn
            {
                F_UserId = p.teacherId
            });
            var oldData = oldDb.Sys_UserLogOn.ToList();
            var addList = newData;
            var Ids = oldData.Select(p => p.F_UserId).ToList();
            var InsertList = new List<Sys_UserLogOn>();
            foreach(var tea in addList)
            {
                if (!Ids.Contains(tea.F_UserId))
                {
                    tea.F_Id = tea.F_UserId;
                    tea.F_UserSecretkey = Md5EncryptHelper.Encrypt("0000", 16).ToLower();
                    tea.F_UserPassword = Md5EncryptHelper.Encrypt(DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt("0000", 32).ToLower(), tea.F_UserSecretkey).ToLower(), 32).ToLower();
                    InsertList.Add(tea);
                }
            }
            //oldDb.BulkInsert(InsertList);
            oldDb.Sys_UserLogOn.AddRange(InsertList);
            oldDb.SaveChanges();
        }

        /// <summary>
        /// 简化程序（把学生数据分为六张表，分为六个方法进行） School_Students
        /// </summary>
        /// <param name="oldDb"></param>
        /// <param name="newDb"></param>
        public void ProcessSchoolStudentInfo(NanHangAccept oldDb, NHModel newDb)
        {
            var oldData = oldDb.School_Students.ToList();
            var newData = newDb.StudentInfoes.Select(p => new School_Students
            {
                F_Id = p.studentId,
                F_Name = p.studentName,
                F_Users_ID = p.studentId,
                F_StudentNum = p.studentNo,
                //F_Class_ID = p.orgId,
                F_Class_ID = p.studentClass,
                F_Gender = p.studentSex,
                F_CredType = p.certificateType,
                F_CredNum = p.certificateNo,
                F_Introduction = p.studentMeto,
                F_FacePic_File = p.ImgUri,
                F_Tel = p.studentPhone
                
            }).ToList();
            var Ids = oldData.Select(p => p.F_Id).ToList();
            var insertList = new List<School_Students>();
            foreach(var stu in newData)
            {
                string GradeID = oldDb.Sys_Organize.Where(p => p.F_Id == stu.F_Class_ID).Select(p => p.F_ParentId).ToList().FirstOrDefault();
                stu.F_Grade_ID = GradeID;
                stu.F_Divis_ID = oldDb.Sys_Organize.Where(p => p.F_Id == GradeID).Select(p => p.F_ParentId).ToList().FirstOrDefault();
                stu.F_DeleteMark = false;
                stu.F_EnabledMark = true;
                if (Ids.Contains(stu.F_Id))
                {
                    oldDb.Set<School_Students>().Where(p => p.F_Id == stu.F_Id).Update(p => new School_Students
                    {
                        F_Id = stu.F_Id,
                        F_Name = stu.F_Name,
                        F_Users_ID = stu.F_Users_ID,
                        F_StudentNum = stu.F_StudentNum,
                        F_Class_ID = stu.F_Class_ID,
                        F_Gender = stu.F_Gender,
                        F_CredType = stu.F_CredType,
                        F_CredNum = stu.F_CredNum,
                        F_Introduction = stu.F_Introduction,
                        //F_FacePic_File = stu.F_FacePic_File,
                        F_Tel = stu.F_Tel,
                        F_Grade_ID = stu.F_Grade_ID,
                        F_Divis_ID = stu.F_Divis_ID,
                        F_DeleteMark = false,
                        F_EnabledMark = true
                });
                    //修改大华闸机学生数据
                    //PersonMoudle personMoudle = contactMoudleStudent(oldDb, stu.F_Id);
                    //personMoudle.roleId = "student001";
                    //DHAccount.PUSH_DH_UPDATE_PERSON(personMoudle);
                }
                else
                {
                    insertList.Add(stu);
                }
            }
            //oldDb.BulkInsert(insertList);
            oldDb.School_Students.AddRange(insertList);
            oldDb.SaveChanges();

            //修改sys_user表的主机id为自动生成的ID，并修改student表的sys_userID为sys_user表生成的主机id
            //string updateSql = "update  student set student.F_User_ID = sysUser.F_ID from sys_user sysUser, school_student student where sysUser.F_Account = student.F_User_ID";
            //SqlHelper.ExecuteNonQuery(updateSql);

            //开始增量数据至大华闸机
            //foreach (School_Students stu in insertList)
            //{
            //    PersonMoudle personMoudle = contactMoudleStudent(oldDb, stu.F_Id);
            //    personMoudle.roleId = "student001";
            //    DHAccount.PUSH_DH_ADD_PERSON(personMoudle);
            //}
        }

        /// <summary>
        /// 简化程序（把学生数据分为六张表，分为六个方法进行） Sys_User
        /// </summary>
        /// <param name="oldDb"></param>
        /// <param name="newDb"></param>
        public void ProcessSchoolStudentSysUser(NanHangAccept oldDb, NHModel newDb)
        {
            var oldData = oldDb.Sys_User.ToList();
            var newData = newDb.StudentInfoes.Select(p => new Sys_User
            {
                F_Id = p.studentId,
                F_RealName = p.studentName,
                F_Account = p.LoginId,
                F_OrganizeId = p.orgId,
                F_MobilePhone = p.studentPhone,
                F_CreatorTime = DateTime.Now,
                F_DepartmentId = p.studentClass,
                F_HeadIcon = p.ImgUri
            });

            var Ids = oldData.Select(p => p.F_Id).ToList();
            var insertList = new List<Sys_User>();
            foreach(var sysUser in newData)
            {
                sysUser.F_RoleId = "student";
                sysUser.F_DutyId = "studentDuty";
                sysUser.F_CreatorTime = DateTime.Now;
                sysUser.F_DeleteMark = false;
                sysUser.F_EnabledMark = true;
                if (Ids.Contains(sysUser.F_Id))
                {
                    oldDb.Set<Sys_User>().Where(p => p.F_Id == sysUser.F_Id).Update(p => new Sys_User
                    {
                        F_RealName = sysUser.F_RealName,
                        F_Account = sysUser.F_Account,
                        F_OrganizeId = sysUser.F_OrganizeId,
                        F_MobilePhone = sysUser.F_MobilePhone,
                        F_RoleId = sysUser.F_RoleId,
                        F_DutyId = sysUser.F_DutyId,
                        F_CreatorTime = sysUser.F_CreatorTime,
                        F_DeleteMark = false,
                        F_EnabledMark = true,
                        F_DepartmentId=sysUser.F_DepartmentId
                    });
                }
                else
                {
                    insertList.Add(sysUser);
                }
            }
            //oldDb.BulkInsert(insertList);
            oldDb.Sys_User.AddRange(insertList);
            oldDb.SaveChanges();
        }

        /// <summary>
        /// 简化程序（把学生数据分为六张表，分为六个方法进行） Sys_User_Role
        /// </summary>
        /// <param name="oldDb"></param>
        /// <param name="newDb"></param>
        public void ProcessSchoolStudentSysUserRole(NanHangAccept oldDb, NHModel newDb)
        {
            var oldData = oldDb.Sys_User_Role.ToList();
            var newData = newDb.StudentInfoes.Select(p => new Sys_User_Role
            {
                F_User = p.studentId
            });
            var Ids = oldData.Select(p => p.F_User).ToList();
            var insertList = new List<Sys_User_Role>();
            foreach(var Rold in newData)
            {
                if (!Ids.Contains(Rold.F_User))
                {
                    Rold.F_Id = Guid.NewGuid().ToString();
                    Rold.F_Role = "student";
                    Rold.F_CreatorTime = DateTime.Now;
                    Rold.F_DeleteMark = false;
                    Rold.F_EnabledMark = true;
                    insertList.Add(Rold);
                }
            }
            //oldDb.BulkInsert(insertList);
            oldDb.Sys_User_Role.AddRange(insertList);
            oldDb.SaveChanges();
        }

        /// <summary>
        /// 简化程序（把学生数据分为六张表，分为六个方法进行） Sys_UserLogOn
        /// </summary>
        /// <param name="oldDb"></param>
        /// <param name="newDb"></param>
        public void ProcessSchoolStudentSysUserLogOn(NanHangAccept oldDb, NHModel newDb)
        {
            var newData = newDb.StudentInfoes.Select(p => new Sys_UserLogOn
            {
                F_UserId = p.studentId
            });
            var oldData = oldDb.Sys_UserLogOn.ToList();
            var Ids = oldData.Select(p => p.F_UserId).ToList();
            var InsertList = new List<Sys_UserLogOn>();
            foreach(var logOn in newData)
            {
                if (!Ids.Contains(logOn.F_UserId))
                {
                    logOn.F_Id = logOn.F_UserId;
                    logOn.F_UserSecretkey = Md5EncryptHelper.Encrypt("0000", 16).ToLower();
                    logOn.F_UserPassword = Md5EncryptHelper.Encrypt(DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt("0000", 32).ToLower(), logOn.F_UserSecretkey).ToLower(), 32).ToLower();
                    InsertList.Add(logOn);
                }
            }
            //oldDb.BulkInsert(InsertList);
            oldDb.Sys_UserLogOn.AddRange(InsertList);
            oldDb.SaveChanges();
        }

        /// <summary>
        /// 简化程序（把学生数据分为六张表，分为六个方法进行） Dorm_Dorm
        /// </summary>
        /// <param name="oldDb"></param>
        /// <param name="newDb"></param>
        public void ProcessSchoolStudentDormInfo(NanHangAccept oldDb, NHModel newDb)
        {
            var oldData = oldDb.dorm_dorm.Select(p => p.F_Title).ToList();
            var newData = newDb.StudentInfoes.Select(p => p.studentBuildingId).ToList();

            var InsertData = newData.Distinct().Except(oldData).ToList(); //取出新增的宿舍信息(先去重，然后再取差集)
            var BuildData = InsertData.Select(p => p.Trim().Replace("栋", "#").Split('#')[0]).Distinct().ToList(); //新增至宿舍楼栋表（dorm_building）
            List<dorm_building> BuildArr = BuildData.Select(p => new dorm_building
            {
                id = Guid.NewGuid().ToString(),
                building_no = p
            }).ToList();
            var InsertList = new List<dorm_dorm>();
            foreach(var data in InsertData)
            {
                string[] split = data.Trim().Replace("栋", "#").Split('#');
                if (split.Length == 2)
                {
                    var dorm = new dorm_dorm();
                    dorm.F_Id = Guid.NewGuid().ToString();
                    dorm.F_Memo = data;
                    dorm.F_CreatorTime = DateTime.Now;
                    dorm.F_Building_No = split[0]; //楼栋
                    dorm.F_Floor_No = split[1].Replace(split[1].Substring(1), ""); //楼层
                    dorm.F_Unit_No = BuildArr.Where(p => p.building_no.Equals(split[0])).Select(p => p.id).First(); //单元 （作为楼栋表 dorm_building 的关联ID）
                    dorm.F_Classroom_No = split[1]; //宿舍号
                    dorm.F_Title = data; //完整的宿舍编号
                    InsertList.Add(dorm);
                }
            }
            //oldDb.BulkInsert(InsertList);
            oldDb.dorm_dorm.AddRange(InsertList);
            oldDb.dorm_building.AddRange(BuildArr);
            oldDb.SaveChanges();
        }

        /// <summary>
        /// 简化程序（把学生数据分为六张表，分为六个方法进行） Dorm_DormStudent
        /// </summary>
        /// <param name="oldDb"></param>
        /// <param name="newDb"></param>
        public void ProcessSchoolStudentSysUserDormStudent(NanHangAccept oldDb, NHModel newDb)
        {
            var oldData = oldDb.Dorm_DormStudent.ToList();
            var newData = newDb.StudentInfoes.Select(p => new Dorm_DormStudent
            {
                F_Student_ID = p.studentId,
                F_Memo = p.studentBuildingId,
                F_Sex = p.studentSex
            });

            var StudentIds = oldData.Select(p => p.F_Student_ID).ToList();
            var InsertStudent = new List<Dorm_DormStudent>();
            foreach(var info in newData)
            {
                if (StudentIds.Contains(info.F_Student_ID))
                {
                    string[] split = info.F_Memo.Trim().Replace("栋", "#").Split('#');
                    if(split.Length == 2)
                    {
                        string ClassRoomId = oldDb.dorm_dorm.Where(p => p.F_Title.Equals(info.F_Memo)).Select(p => p.F_Id).ToList().FirstOrDefault();
                        oldDb.Set<Dorm_DormStudent>().Where(p => p.F_Student_ID == info.F_Student_ID).Update(p => new Dorm_DormStudent
                        {
                            F_DormId = ClassRoomId,
                            F_Bed_ID = "",
                            F_Sex = info.F_Sex,
                            F_Memo = info.F_Memo
                        });
                    }
                }
                else
                {
                    try
                    {
                        string[] split = info.F_Memo.Trim().Replace("栋", "#").Split('#');
                        if (split.Length == 2)
                        {
                            string ClassRoomId = oldDb.dorm_dorm.Where(p => p.F_Title.Equals(info.F_Memo)).Select(p => p.F_Id).ToList().FirstOrDefault();
                            var student = new Dorm_DormStudent();
                            student.F_Id = Guid.NewGuid().ToString();
                            student.F_CreatorTime = DateTime.Now;
                            student.F_Student_ID = info.F_Student_ID;
                            student.F_DormId = ClassRoomId;
                            student.F_Bed_ID = "";
                            student.F_Sex = info.F_Sex;
                            student.F_Memo = info.F_Memo;
                            InsertStudent.Add(student);
                        }
                    }
                    catch
                    {

                    }
                }
            }
            //oldDb.BulkInsert(InsertStudent);
            oldDb.Dorm_DormStudent.AddRange(InsertStudent);
            oldDb.SaveChanges();
        }

        public static PersonMoudle contactMoudleStudent(NanHangAccept oldDb, string id)
        {
            string sql = "SELECT NULL\n" +
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

        public static PersonMoudle contackMoudleTeacher(NanHangAccept oldDb, string id)
        {
            string sql = "SELECT \n" +
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
                "        AND a.F_Id = '"+id+"'";
            return oldDb.Database.SqlQuery<PersonMoudle>(sql).FirstOrDefault();
        }
    }
}
