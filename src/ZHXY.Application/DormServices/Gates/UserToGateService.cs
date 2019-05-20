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

        public UserToGateService() => R = new ZhxyRepository();

        public void SendUserHeadIco(string[] userId)
        {
            var stuList = Read<User>().Where(t => userId.Contains(t.Id)).ToList().Select(d =>
            {
                string studentNo = d.Name;
                string imgUri = d.HeadIcon;
                int gender = 0;
                string certificateNo = "";
                string userType = "teacher001";
                string ss = "";
                string lc = "";
                string ld = "";
                if (d.DutyId.Contains("student"))
                {
                    var stu = Read<Student>(p => p.UserId == d.Id).FirstOrDefault();
                    studentNo = stu?.StudentNumber;
                    gender = stu?.Gender == "0" ? 2 : 1;
                    certificateNo = stu?.CredNumber;
                    userType = "student001";// "学生";

                    var ssdata = Query<DormStudent>(p => p.StudentId == stu.Id).FirstOrDefault();
                    ss = ssdata?.DormInfo?.Title;
                    lc = ssdata?.DormInfo?.FloorNumber;
                    var ldid = ssdata?.DormInfo?.BuildingId;
                    // 闸机Id列表
                    var zjids = Read<Relevance>(p => p.SecondKey == ldid && p.Name == "Gate_Building").Select(p => p.FirstKey).ToList();
                    // 楼栋Id列表
                    var lds = Read<Relevance>(p => p.Name == "Gate_Building" && zjids.Contains(p.FirstKey)).Select(p => p.SecondKey).ToList();

                    var lddatas = Read<Building>(p => lds.Contains(p.Id)).ToList();
                    ld = lddatas.Count <= 0 ? "未绑定楼栋" : string.Join("_", lddatas);
                }
                if (d.DutyId.Contains("teacher"))
                {
                    var tea = Read<Teacher>(p => p.UserId == d.Id).FirstOrDefault();
                    studentNo = tea?.JobNumber;
                    //imgUri = tea?.FacePhoto;
                    gender = tea?.Gender == "0" ? 2 : 1;
                    certificateNo = tea?.CredNumber;
                    userType = "teacher001";// "教职工";
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
                    var dhUserstr = DHAccount.SELECT_DH_PERSON(new PersonMoudle {  code = person.code});
                    if (dhUserstr != null)
                    {
                        //var jo = dhUserstr.ToString().ToJObject();
                        //int code = jo.Value<int>("code"); //返回码
                        //if (code == 200)
                        //{
                        //    var datas = (List<object>)jo["data"]["list"].ToObject(typeof(List<object>));
                        //    for (int i = 0; i < datas.Count; i++)
                        //    {
                        //        string id = jo["data"]["list"][i]["id"]?.ToString();
                        //        DHAccount.PUSH_DH_DELETE_PERSON(new string[] { id });
                        //    }
                        //}
                        try
                        {
                            DHAccount.PUSH_DH_UPDATE_PERSON(person);
                        }
                        catch(Exception e)
                        {
                            result += person.name + ",";
                        }
                    }
                    else
                    {
                        try
                        {
                            DHAccount.PUSH_DH_ADD_PERSON(person);
                        }
                        catch (Exception e)
                        {
                            result += person.name + ",";
                        }
                    }
                }
                catch
                {
                }
            }
            if (result != null)
                throw new Exception("以下用户下载失败!"+result);
        }
    }
}
