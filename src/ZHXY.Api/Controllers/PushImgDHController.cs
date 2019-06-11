using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ZHXY.Common;
using ZHXY.Domain;

namespace ZHXY.Api.Controllers
{
    public class PushImgDHController : ApiController
    {

        /// <summary>
        /// 生成学生信息报表，下发大华U8000
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string TestPushStudent()
        {
            string MoudleFilePath = "C:\\人员信息Moudle.xlsx";
            string DataFilePath = "C:\\人员信息(学生).xlsx";
            var moudle = new EFContext();
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


        /// <summary>
        ///  生成学生数据报表，处理宿舍相关并下发U8000
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string PushStudent()
        {
            string MoudleFilePath = "C:\\人员信息Moudle.xlsx";
            string DataFilePath = "C:\\人员信息(学生)规范楼栋.xlsx";
            var moudle = new EFContext();
            List<DHStudentMoudle> MoudleList = new List<DHStudentMoudle>();
            var DormStudentInfos = moudle.Set<Student>().GroupJoin(moudle.Set<DormStudent>(), p => p.Id, s => s.StudentId, (stu, dorm) => dorm.DefaultIfEmpty().Select(o => new DHStudentMoudle
            {
                StudentNum = stu.StudentNumber,
                name = stu.Name,
                CredNum = stu.CredNumber,
                sex = stu.Gender.Equals("1") ? "男" : "女",
                DormId = dorm.FirstOrDefault().DormId
            })).SelectMany(x => x).ToList();
            var NoDormList = DormStudentInfos.Where(p => p.DormId == null).Select(p => new DHStudentMoudle
            {
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
                DormName = dorm.Title.Trim(),
                BuildName = dorm.Title.Trim().Split('栋')[0] + "栋",
                FloorName = dorm.Title.Trim().Split('栋')[0] + "栋" + dorm.Title.Trim().Split('栋')[1].Replace(dorm.Title.Trim().Split('栋')[1].Substring(1), "") + "层"
            }).ToList();
            MoudleList.AddRange(NoDormList);
            MoudleList.AddRange(HasDormList);

            foreach(var StudentMoudle in MoudleList)
            {
                switch (StudentMoudle.BuildName)
                {
                    case "1栋" : StudentMoudle.BuildName = "1栋2栋"; break;
                    case "2栋": StudentMoudle.BuildName = "1栋2栋"; break;
                    case "3栋": StudentMoudle.BuildName = "3栋4栋"; break;
                    case "4栋": StudentMoudle.BuildName = "3栋4栋"; break;
                    case "5栋": StudentMoudle.BuildName = "5栋6栋"; break;
                    case "6栋": StudentMoudle.BuildName = "5栋6栋"; break;
                    case "7栋": StudentMoudle.BuildName = "7栋8栋"; break;
                    case "8栋": StudentMoudle.BuildName = "7栋8栋"; break;
                    case "9栋": StudentMoudle.BuildName = "9栋10栋"; break;
                    case "10栋": StudentMoudle.BuildName = "9栋10栋"; break;
                    case "11栋": StudentMoudle.BuildName = "11栋12栋"; break;
                    case "12栋": StudentMoudle.BuildName = "11栋12栋"; break;
                    case "15栋": StudentMoudle.BuildName = "15栋16栋"; break;
                    case "16栋": StudentMoudle.BuildName = "15栋16栋"; break;
                    case "17栋": StudentMoudle.BuildName = "17栋18栋"; break;
                    case "18栋": StudentMoudle.BuildName = "17栋18栋"; break;
                    case "19栋": StudentMoudle.BuildName = "19栋20栋"; break;
                    case "20栋": StudentMoudle.BuildName = "19栋20栋"; break;
                    case "21栋": StudentMoudle.BuildName = "21栋22栋"; break;
                    case "22栋": StudentMoudle.BuildName = "21栋22栋"; break;
                    case "23栋": StudentMoudle.BuildName = "23栋24栋"; break;
                    case "24栋": StudentMoudle.BuildName = "23栋24栋"; break;
                    case "25栋": StudentMoudle.BuildName = "25栋26栋"; break;
                    case "26栋": StudentMoudle.BuildName = "25栋26栋"; break;
                    case "27栋": StudentMoudle.BuildName = "27栋28栋"; break;
                    case "28栋": StudentMoudle.BuildName = "27栋28栋"; break;
                    case "海院A栋": StudentMoudle.BuildName = "海A栋海B栋"; break;
                    case "海院B栋": StudentMoudle.BuildName = "海A栋海B栋"; break;
                    default:break;
                }
            }

            bool flag = NPOIExcelImport<DHStudentMoudle>.WriteExcel(MoudleFilePath, DataFilePath, MoudleList);
            Console.WriteLine(flag);
            return "success";
        }

        /// <summary>
        /// 生成教师信息报表，并下发 U8000
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string TestPushTeacher()
        {
            string MoudleFilePath = "C:\\人员信息Moudle.xlsx";
            string DataFilePath = "C:\\人员信息(教师).xlsx";
            var moudle = new EFContext();
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
