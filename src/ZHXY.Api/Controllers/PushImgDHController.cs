using System;
using System.Linq;
using System.Web.Http;
using TaskApi.NanHang;
using ZHXY.Common;

namespace ZHXY.Api.Controllers
{
    public class PushImgDHController : ApiController
    {
        [HttpGet]
        public string TestPush(string name)
        {

            string MoudleFilePath = "C:\\人员信息Moudle.xlsx";
            string DataFilePath = "C:\\人员信息.xlsx";
            var moudle = new NanHangAccept();
            //var DormStudentInfos = moudle.Dorm_DormStudent.Where(s => s.F_Student_ID == s.Student.F_Id && s.Student.F_DeleteMark == false && !s.F_Memo.Contains("11栋") && !s.F_Memo.Contains("12栋")).Select(p => new DHStudentMoudle
            var DormStudentInfos = moudle.Dorm_DormStudent.Where(s => s.F_Student_ID == s.Student.F_Id && s.Student.F_DeleteMark == false).Select(p => new DHStudentMoudle
            {
                DormName = p.F_Memo,
                StudentNum = p.Student.F_StudentNum,
                name = p.Student.F_Name.Replace("·", "-").Replace("", "yǎn"),
                CredNum = p.Student.F_CredNum,
                sex = p.Student.F_Gender.Equals("1") ?"女":"男"
            }).ToList();
            foreach (var info in DormStudentInfos)
            {
                var Split = info.DormName.Split('栋');
                info.BuildName = Split[0] + "栋"; //楼栋   XX栋
                info.FloorName = info.BuildName + Split[1].Replace(Split[1].Substring(1), "") + "层"; //楼层   XX栋XX层
            }
            bool flag = NPOIExcelImport<DHStudentMoudle>.WriteExcel(MoudleFilePath, DataFilePath , DormStudentInfos);
            Console.WriteLine(flag);
            return "success";
        }
    }
}
