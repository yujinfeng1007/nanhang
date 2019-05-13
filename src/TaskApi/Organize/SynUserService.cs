using System;
using System.Linq;
using ZHXY.Common;

using ZHXY.Domain;

namespace TaskApi
{
    public class SynUserService
    {
        protected WebHelper WebHelp { get { return new WebHelper(); } }
        private readonly string CallUrlAdd = Configs.GetValue("CallUrlAdd");
        protected string AppId { get { return Configs.GetValue("appid"); } }
        private string SchoolCode { get; }

        public SynUserService(string schoolCode) => SchoolCode = schoolCode;

        public void UpdOrAdd(UserModel data, string catetoryId)// string loginId, string mobilePhone, string orgId, string passWord, string telephone, string userId, string userName, string userStatus, string userType, string catetoryId, string gw,string num,string sex)
        {
            using (var db = new UnitWork().BeginTrans())
            {
                string gw = data.post_Description;
                if (data.isHeadmaster == "1")
                {
                    gw = "班主任";
                }
                if (data.isSubHeadmaster == "1")
                {
                    gw = "副班主任";
                }
                var userType = data.UserType;
                string num = data.user_Num;
                string loginId = data.LoginId;
                string mobilePhone = data.MOBILE;
                string orgId = data.OrgId;
                string passWord = data.PassWord;
                string telephone = data.TELEPHONE;
                string userId = data.UserId;
                string userName = data.UserName;
                string userStatus = data.UserStatus;
                string sex = data.sex;
                DateTime? inDate = null;
                DateTime tmpDate;
                if (DateTime.TryParse(data.join_school_time, out tmpDate))
                    inDate = tmpDate;
                var entity = AddOrUpdUser(db, loginId, mobilePhone, orgId, passWord, telephone, userId, userName, userStatus, userType, catetoryId, gw, sex, data.picture, data.has_school_admin, data.IS_DELETED);
                if (userType == "1")
                {
                    AddOrUpdTeacher(db, entity, catetoryId, gw, num, data.identity_num, data.major, data.political, data.identity_type, inDate);
                }
                if (userType == "6")
                {
                    AddOrUpdStudent(db, entity, catetoryId, num, data.identity_num, data.political, data.identity_type, inDate, data.GuardianId, data.GuardianName);
                }
                if (userType == "5")
                {
                }
                db.Commit();
            }
        }

        private void AddOrUpdStudent(IUnitWork db, User entity, string catetoryId, string num,
            string credNum, string politstatu, string credType, DateTime? entryTime, string parentId, string parentName)
        {
            var student = new Student();
            if (catetoryId == "Class")
            {
                var org = db.FindEntity<Organ>(p => p.Id == entity.OrgId);
                student.F_Class_ID = entity.OrgId;
                student.F_Grade_ID = org?.ParentId;
                student.F_Divis_ID = org?.Parent?.ParentId;
            }
            if (catetoryId == "Grade")
            {
                var org = db.FindEntity<Organ>(p => p.Id == entity.OrgId);
                student.F_Grade_ID = entity.OrgId;
                student.F_Divis_ID = org?.ParentId;
            }
            if (catetoryId == "Division")
                student.F_Divis_ID = entity.OrgId;
            student.F_Name = entity.F_RealName;
            student.F_StudentNum = num;
            student.F_CredNum = credNum;
            student.F_CredType = credType;
            student.F_Gender = entity.F_Gender != false ? "1" : "0";
            student.F_PolitStatu = politstatu;
            student.F_InitDTM = entryTime;
            //student.F_Users_ID = entity.F_Id;
            student.F_Tel = entity.F_MobilePhone;
            student.F_DepartmentId = entity.OrgId;

            var s = db.FindEntity<Student>(p => p.F_Users_ID == entity.F_Id);
            if (s != null)
            {
                student.Modify(s.F_Id);
                db.Update(student);
            }
            else
            {
                //student.F_StudentNum = entity.F_Id;
                student.F_Users_ID = entity.F_Id;
                student.F_CurStatu = "1";
                student.Create();
                db.Insert(student);
            }

            //AddStudentAsParent(db, student.F_Id, student.F_StudentNum, parentId, parentName);
        }
        //// 创建学生和家长关联表
        //private void AddStudentAsParent(IUnitWork db, string studentId, string studentNum, string parentId
        //    , string parentName)
        //{
        //    if (string.IsNullOrEmpty(parentId))
        //        return;
        //    var data = db.FindEntity<StuParent>(t => t.F_Stu_Id == studentId && t.F_ParentId == parentId);
        //    if (data != null)
        //        return;
        //    var e = new StuParent();
        //    e.F_ParentId = parentId;
        //    e.F_ParentName = parentName;
        //    e.F_Parent_CardNo = string.Empty;
        //    e.F_Parent_Phone = "";
        //    e.F_Stu_Id = studentId;
        //    //e.F_Stu_Name = student.F_Name;
        //    e.F_Stu_Num = studentNum;
        //    e.Create();
        //    db.Insert(e);
        //}
        private void AddOrUpdTeacher(IUnitWork db, User entity, string catetoryId, string gw, string num,
            string credNum, string profession, string politstatu, string credType, DateTime? entryTime)
        {
            var teacher = new Teacher();
            if (catetoryId == "Class")
            {
                var org = db.FindEntity<Organ>(p => p.Id == entity.OrgId);
                teacher.F_Divis_ID = org?.Parent?.ParentId;
            }
            if (catetoryId == "Grade")
            {
                var org = db.FindEntity<Organ>(p => p.Id == entity.OrgId);
                teacher.F_Divis_ID = org?.ParentId;
            }
            if (catetoryId == "Division")
                teacher.F_Divis_ID = entity.OrgId;
            teacher.F_Name = entity.F_RealName;
            teacher.F_Num = num;
            //student.F_Users_ID = entity.F_Id;
            teacher.F_MobilePhone = entity.F_MobilePhone;
            teacher.F_DepartmentId = entity.OrgId;
            teacher.F_CredNum = credNum;
            teacher.F_Profession = profession;
            teacher.F_PolitStatu = politstatu;
            teacher.F_CredType = credType;
            teacher.F_EntryTime = entryTime;
            var t = db.FindEntity<Teacher>(p => p.F_User_ID == entity.F_Id);
            if (t != null)
            {
                teacher.Modify(t.F_Id);
                db.Update(teacher);
            }
            else
            {
                teacher.F_User_ID = entity.F_Id;
                teacher.Create();
                db.Insert(teacher);
            }

            if (catetoryId == "Class" && (gw == "班主任" || gw == "副班主任"))
            {
               // AddOrUpdClassTeacher(db, entity.F_DepartmentId, teacher.F_Id, gw);
            }
        }

        //private void AddOrUpdClassTeacher(IUnitWork db, string classId, string teacherId, string gw)
        //{
        //    var datas = db.QueryAsNoTracking<ClassTeacher>(t => t.F_ClassID == classId).ToList();
        //    if (datas.Count > 0)
        //    {
        //        foreach (var data in datas)
        //        {
        //            if (gw == "班主任")
        //                data.F_Leader_Tea = teacherId;
        //            if (gw == "副班主任")
        //                data.F_Leader_Tea2 = teacherId;
        //            db.Update(data);
        //        }
        //    }
        //    else
        //    {
        //        var ent = new ClassTeacher();
        //        ent.Create();
        //        ent.F_ClassID = classId;
        //        if (gw == "班主任")
        //            ent.F_Leader_Tea = teacherId;
        //        if (gw == "副班主任")
        //            ent.F_Leader_Tea2 = teacherId;
        //        db.Insert(ent);
        //    }
        //}

        private User AddOrUpdUser(IUnitWork db, string loginId, string mobilePhone, string orgId, string passWord, string telephone, string userId, string userName, string userStatus, string userType, string catetoryId, string gw, string sex, string ico, string isAdmin, string isDelete)
        {
            var entity = new User();
            entity.F_EnabledMark = true;
            entity.F_Id = userId;
            entity.EmailPassword = passWord;
            entity.F_Account = loginId;
            entity.F_MobilePhone = mobilePhone;
            entity.F_NickName = userName;
            entity.F_RealName = userName;
            entity.OrgId = orgId;
            entity.F_Gender = sex == "1" ? true : false;
            entity.F_HeadIcon = ico;
            entity.F_DeleteMark = isDelete == "1" ? true : false;
            if (catetoryId == "Class")
            {
                var org = db.FindEntity<Organ>(p => p.Id == orgId);
                entity.F_OrganizeId = org?.Parent?.ParentId;
            }
            if (catetoryId == "Grade")
            {
                var org = db.FindEntity<Organ>(p => p.Id == orgId);
                entity.F_OrganizeId = org?.ParentId;
            }
            if (catetoryId == "Division")
                entity.F_OrganizeId = orgId;
            if (userType == "6")
            {
                entity.F_DutyId = null;
                entity.F_RoleId = "student";
            }
            if (userType == "1")
            {
                entity.F_DutyId = "teacherDuty";
                entity.F_RoleId = "teacher";
                if ((gw == "班主任" || gw == "副班主任"))//catetoryId == "Class" &&
                {
                    entity.F_RoleId = "6D8BC58FF1F24924A73F6B86A718BD6C";
                }
            }
            if (userType == "5")
            {
                entity.F_DutyId = "parentDuty";
                entity.F_RoleId = "parent";
                entity.OrgId = "parent";
            }
            if (isAdmin == "1")
            {
                entity.F_RoleId = "7A6C0ECA17B9433DBD3A0C127E35A696";
            }
            var data = db.FindEntity<User>(t => t.F_Id == userId);
            if (data != null)
            {
                db.Update(entity);
            }
            else
            {
                entity.F_Id = userId;
                db.Insert(entity);

                setUserInfo(db, entity);
            }
            // 插入角色
            if (entity.F_RoleId != null)
            {
                var role = db.FindEntity<UserRole>(t => t.F_User == entity.F_Id);
                if (role != null)
                {
                    role.F_Role = entity.F_RoleId;
                    db.Update(role);
                }
                else
                {
                    var e = new UserRole();
                    e.F_Id = Guid.NewGuid().ToString();
                    e.F_Role = entity.F_RoleId;
                    e.F_User = entity.F_Id;
                    db.Insert(e);
                }
            }
            return entity;
        }

        private void setUserInfo(IUnitWork db, User entity)
        {
            if (db.FindEntity<UserLogin>(t => t.F_Id == entity.F_Id) != null)
                return;
            var userLogOnEntity = new UserLogin
            {
                F_Id = entity.F_Id,
                F_UserId = entity.F_Id,
                F_UserSecretkey = Md5EncryptHelper.Encrypt("0000", 16).ToLower()
            };
            userLogOnEntity.F_UserPassword = Md5EncryptHelper.Encrypt(DESEncryptHelper.Encrypt(Md5EncryptHelper.Encrypt("0000", 32).ToLower(), userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();
            db.Insert(userLogOnEntity);

            var isPost = true;
            var parameters = "sysid=" + entity.F_Id + "&amp;appid=" + AppId + "&amp;nickname=" +
                             entity.F_RealName + "&amp;username=" + entity.F_Account +
                             "&amp;password=" + userLogOnEntity.F_UserPassword +
                             "&amp;sysgroupid=" + entity.OrgId + "&amp;headicon=" +
                             entity.F_HeadIcon + "&amp;birthday=" + entity.F_Birthday + "";
            WebHelper.SendRequest(CallUrlAdd, parameters, isPost, "application/json");
        }
    }
}