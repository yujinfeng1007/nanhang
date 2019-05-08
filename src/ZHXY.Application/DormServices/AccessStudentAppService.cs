using ZHXY.Common;
using System.Collections.Generic;
using ZHXY.Domain;
using System;
using System.Linq;
using ZHXY.Dorm.Device.DH;

namespace ZHXY.Application
{
    public class AccessStudentAppService : AppService
    {
        public AccessStudentAppService(IZhxyRepository r) => R = r;

         public List<AccessStudent> GetList(Pagination pagination, string keyword,string F_DevId)
        {
            var expression = ExtLinq.True<AccessStudent>();
            if (!string.IsNullOrEmpty(keyword))
            {
                //请输入学号 / 院系 / 专业 / 班级
                expression = expression.And(t => t.Student.F_StudentNum.Contains(keyword));
                expression = expression.Or(t => t.Student.F_Divis_ID.Contains(keyword));
                expression = expression.Or(t => t.Student.F_Grade_ID.Contains(keyword));
                expression = expression.Or(t => t.Student.F_Class_ID.Contains(keyword));
            }
            if(!string.IsNullOrEmpty(F_DevId))
            {
                expression = expression.And(t => t.F_DeviceId==(F_DevId));
            }

            return Read(expression).Paging(pagination).ToList();
        }


        public void Delete(string id)
        {
            var ids = id.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            var removeList=Query<AccessStudent>(p => ids.Contains(p.F_Id)).ToList();
            DelAndSave<AccessStudent>(removeList);
            //删除设备数据
            DHAccount.PUSH_DH_DELETE_PERSON(ids);
        }

        public void AddAccessUser(string  deviceId,string userIds)
        {
            var ids = userIds.Split(',');

            var studentService = new StudentAppService();
            var teacherService = new TeacherAppService();
            var userService = new SysUserAppService();
            foreach (var id in ids)
            {
                var devData = new ZHXY.Dorm.Device.tools.PersonMoudle();

                var entity = new AccessStudent();
                entity.F_DeviceId = deviceId;

                var teacher= teacherService.GetByUser(id); 
                if(teacher!=null)
                {
                    entity.F_UserId = teacher.F_Id;
                    entity.F_UserName = teacher.F_Name;
                    entity.F_UserNum = teacher.F_Num;
                    entity.F_UserType = "2";
                    devData = getTeacherDevData(teacher);
                }
                else
                {
                    var student = studentService.Query<Student>(t=>t.F_Users_ID==id).FirstOrDefault();
                    if (student != null)
                    {
                        entity.F_UserId = student.F_Id;
                        entity.F_UserName = student.F_Name;
                        entity.F_UserNum = student.F_StudentNum;
                        entity.F_UserType = "1";
                        devData = getStudentDevData(student);
                    }
                }
                if (!Read<AccessStudent>(t=>t.F_UserId==entity.F_UserId).Any())
                {
                    //下发至设备
                    PUSH_DH(entity.F_DeviceId, devData);
                    // 添加记录
                    entity.Create();
                    AddAndSave(entity);
                }
            }
        }

        public object GetById(string id) => throw new NotImplementedException();

        private Dorm.Device.tools.PersonMoudle getStudentDevData(Student student)
        {
            
            var dormStudent = Read<DormStudent>(t => t.F_Student_ID == student.F_Id);
            var data = new Dorm.Device.tools.PersonMoudle
            {
                //accessCardsn,
                code = student.F_StudentNum,
                //colleageClass = student.F_Class_ID,
                //colleageCode = student.F_Divis_ID,
                colleageCode= "55f67dcc42a5426fb0670d58dda22a5b",
                //colleageGrade,
                //colleageMajor = student.F_Grade_ID,
                //contactNum = student.F_Tel,
                //dormitoryBed = dormStudent?.F_Bed_ID,
                dormitoryCode = "ab3238e2f28948e8afb461f6f6df953f",// dormStudent?.DormInfo?.F_Building_No,
                dormitoryFloor = "ddf6df9c74124f00b36bd4d915a4853c",//dormStudent?.DormInfo?.F_Floor_No,
                dormitoryRoom = "879f179da29446dba72e1547037512ad",// dormStudent?.DormInfo?.F_Title,
                idCode = student.F_CredNum,
                name = student.F_Name,
                //orgId,
                //pageNum,
                //pageSize,
                //param,
                //rfidCardsn,
                //roleId,
                sex = student.F_Gender == "1" ? 1 : 2,
                //colleageArea,
                //dormitoryArea,
                //dormitoryLeader = dormStudent?.DormInfo?.F_Leader_Name,
                //dormitoryLeaderPhone = student.F_Tel,
                //emergencyPerson,
                //emergencyPersonPhone,
                //instructorCode,
                //instructorName,
                //instructorPhone,
                //managementClassList,
                photoUrl = student.F_FacePic_File,
                //photoBase64 = GetImageBase64Str.ImageBase64Str(student.F_FacePic_File),
                //photoUrl = "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1554801343&di=34a439fb6146c14b733929b5ff14e04b&imgtype=jpg&er=1&src=http%3A%2F%2Fwww.xixi173.com%2Fsoft%2FUploadPic%2F2013-2%2F201322114482529746.jpg",
                //photoBase64 = GetImageBase64Str.ImageBase64Str("https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1554801343&di=34a439fb6146c14b733929b5ff14e04b&imgtype=jpg&er=1&src=http%3A%2F%2Fwww.xixi173.com%2Fsoft%2FUploadPic%2F2013-2%2F201322114482529746.jpg"),
                roleId = "student001"
                //resume
            };
            return data;
        }

        private ZHXY.Dorm.Device.tools.PersonMoudle getTeacherDevData(Teacher teacher)
        {
            var data = new ZHXY.Dorm.Device.tools.PersonMoudle
            {
                //accessCardsn,
                code = teacher.F_Num,
                //colleageClass = student.F_Class_ID,
                //colleageCode = student.F_Divis_ID,
                ////colleageGrade,
                //colleageMajor = student.F_Grade_ID,
                contactNum = teacher.F_MobilePhone,
                //dormitoryBed = dormStudent?.F_Bed_ID,
                //dormitoryCode = dormStudent?.DormInfo?.F_Building_No,
                //dormitoryFloor = dormStudent?.DormInfo?.F_Floor_No,
                //dormitoryRoom = dormStudent?.DormInfo?.F_Title,
                idCode = teacher.F_CredNum,
                name = teacher.F_Name,
                //orgId,
                //pageNum,
                //pageSize,
                //param,
                //rfidCardsn,
                //roleId,
                sex = teacher.F_Gender == "1" ? 1 : 2,
                //colleageArea,
                //dormitoryArea,
                //dormitoryLeader = dormStudent?.DormInfo?.F_Leader_Name,
                dormitoryLeaderPhone = teacher.F_MobilePhone,
                //emergencyPerson,
                //emergencyPersonPhone,
                //instructorCode,
                //instructorName,
                //instructorPhone,
                //managementClassList,
                photoUrl = teacher.F_FacePhoto,
                //photoBase64 = GetImageBase64Str.ImageBase64Str(student.F_FacePic_File),
                //photoUrl = "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1554801343&di=34a439fb6146c14b733929b5ff14e04b&imgtype=jpg&er=1&src=http%3A%2F%2Fwww.xixi173.com%2Fsoft%2FUploadPic%2F2013-2%2F201322114482529746.jpg",
                //photoBase64 = GetImageBase64Str.ImageBase64Str("https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1554801343&di=34a439fb6146c14b733929b5ff14e04b&imgtype=jpg&er=1&src=http%3A%2F%2Fwww.xixi173.com%2Fsoft%2FUploadPic%2F2013-2%2F201322114482529746.jpg"),
                roleId = "teacher001"
                //resume
            };
            return data;
        }

        private void PUSH_DH(string deviceId, ZHXY.Dorm.Device.tools.PersonMoudle data)
        {
            var r = DHAccount.PUSH_DH_ADD_PERSON(data);
            var result=Json.ToJObject(r.ToString());
            var te = result["success"];
            if (result["success"]?.ToString().ToLower() == "false")
            {
                throw new Exception("下发闸机失败!"+r+ data.photoUrl+data.photoUrl);
            }
        }
    }
}

