using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ZHXY.Api.moudle;
using ZHXY.Common;
using ZHXY.Domain;

namespace ZHXY.Api.Controllers
{
    public class PushImgDHController : ApiController
    {
        [HttpGet]
        public string TestPush(string name)
        {
            string MoudleFilePath = "C:\\人员信息Moudle.xlsx";
            string DataFilePath = "C:\\人员信息(学生).xlsx";
            var moudle = new ZhxyDbContext();
            List<DHStudentMoudle> MoudleList = new List<DHStudentMoudle>();
            var DormStudentInfos = moudle.Set<Student>().GroupJoin(moudle.Set<DormStudent>(), p => p.Id, s => s.StudentId, (stu, dorm) => dorm.DefaultIfEmpty().Select( o => new DHStudentMoudle
            {
                StudentNum = stu.StudentNumber,
                name = stu.Name,
                CredNum = stu.CredNumber,
                sex = stu.Gender.Equals("1") ? "男" : "女",
                DormId = dorm.FirstOrDefault().DormId
            })).SelectMany(x => x).ToList();
            var NoDormList = DormStudentInfos.Where(p => p.DormId == null).Select(p => new DHStudentMoudle {
                StudentNum = p.StudentNum,
                name = p.name,
                CredNum = p.CredNum,
                sex = p.sex,
                DormName = "未住楼栋未住楼层未知宿舍",
                BuildName = "未住楼栋",
                FloorName = "未住楼栋未住楼层"
            }).ToList();
            var HasDormList = DormStudentInfos.Where(p => p.DormId != null).Join(moudle.Set<DormRoom>(), s => s.DormId, p => p.Id, (temp, dorm) => new DHStudentMoudle
            {
                StudentNum = temp.StudentNum,
                name = temp.name,
                CredNum = temp.CredNum,
                sex = temp.sex,
                DormName = dorm.Title,
                BuildName = dorm.Title.Split('栋')[0] + "栋",
                FloorName = dorm.Title.Split('栋')[0] + "栋" + dorm.Title.Split('栋')[1].Replace(dorm.Title.Split('栋')[1].Substring(1), "") + "层"
            }).ToList();
            MoudleList.AddRange(NoDormList);
            MoudleList.AddRange(HasDormList);
            bool flag = NPOIExcelImport<DHStudentMoudle>.WriteExcel(MoudleFilePath, DataFilePath, MoudleList);
            Console.WriteLine(flag);
            return "success";
        }


        [HttpGet]
        public string TestPushTeacher(string name)
        {
            string MoudleFilePath = "C:\\人员信息Moudle.xlsx";
            string DataFilePath = "C:\\人员信息(教师).xlsx";
            var moudle = new ZhxyDbContext();
            var DormTeacherInfos = moudle.Set<Teacher>().AsNoTracking().Select(p => new DHTeacherMoudle
            {
                TeacherNo = p.JobNumber,
                name = p.Name,
                CredNum = p.CredNumber,
                sex = p.Gender.Equals("1") ? "男" : "女"
            }).ToList();
            bool flag = NPOIExcelImport<DHTeacherMoudle>.TeacherWriteExcel(MoudleFilePath, DataFilePath, DormTeacherInfos);
            Console.WriteLine(flag);
            return "success";
        }
    }
}
