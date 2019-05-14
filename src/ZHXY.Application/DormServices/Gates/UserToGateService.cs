using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZHXY.Common;
using ZHXY.Domain;
using ZHXY.Dorm.Device.DH;
using ZHXY.Dorm.Device.tools;

namespace ZHXY.Application.DormServices.Gates
{
    /// <summary>
    /// 闸机人员信息
    /// </summary>
    public class UserToGateService : AppService
    {
        public UserToGateService(IZhxyRepository r) : base(r) { }

        public void SendUserHeadIco(string[] userId)
        {
            var stuList = Read<User>().Where(t => userId.Contains(t.Id)).ToList().Select(d =>
            {
                string studentNo = d.Name;
                string imgUri = d.HeadIcon;
                int gender = 0;
                string certificateNo = "";
                string userType = "访客";
                string ss = "";
                string lc = "";
                string ld = "";
                if (d.RoleId.Contains("student"))
                {
                    var stu = Read<Student>(p => p.F_Users_ID == d.Id).FirstOrDefault();
                    studentNo = stu?.F_StudentNum;
                    imgUri = stu?.F_FacePic_File;
                    gender = stu?.F_Gender == "0" ? 2 : 1;
                    certificateNo = stu?.F_CredNum;
                    userType = "学生";
                    var ssdata = Query<DormStudent>(p => p.StudentId == stu.F_Id).FirstOrDefault();
                    ss = ssdata?.DormInfo?.Title;
                    lc = ssdata?.DormInfo?.FloorNumber;
                    var ldid = ssdata?.DormInfo?.BuildingId;
                    var lddata = Read<Building>(p => p.Id == ldid).FirstOrDefault();
                    ld = lddata?.BuildingNo;
                }
                if (d.RoleId.Contains("teacher"))
                {
                    var tea = Read<Teacher>(p => p.UserId == d.Id).FirstOrDefault();
                    studentNo = tea?.JobNumber;
                    imgUri = tea?.FacePhoto;
                    gender = tea?.Gender == "0" ? 2 : 1;
                    certificateNo = tea?.CredNum;
                    userType = "教职工";
                }
                return new PersonMoudle
                {
                    orgId = "org001",
                    code = studentNo,
                    idCode = certificateNo,
                    name = d.Name,
                    roleId = userType,
                    sex = gender,
                    //colleageCode = "55f67dcc42a5426fb0670d58dda22a5b", //默认分院
                    dormitoryCode = ld,// "fe8a5225be5f43478d0dd0c85da5dd1d",//楼栋  例如： 11栋
                    dormitoryFloor = lc,// "8e447843bc8c4e92b9ffdf777047d20d", //楼层  例如：3楼
                    dormitoryRoom = ss,// "20c70f65b54b4f96851e26343678c4ec", //宿舍号  例如：312
                    photoUrl = imgUri
                };
            }).ToList();
            string result = null;
            foreach (var person in stuList)
            {
                try
                {
                    var d = DHAccount.PUSH_DH_ADD_PERSON(person);
                }
                catch (Exception e)
                {
                    result += person.name + ",";
                }
            }
            if (result != null)
                throw new Exception("以下用户下载失败!"+result);
        }

    }

}
