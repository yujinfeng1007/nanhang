using OpenApi.Parms.School;
using System.Linq;
using System.Web.Http;
using ZHXY.Application;
namespace OpenApi.Controllers.Sys
{
    [RoutePrefix("api/Teacher/Teacher")]
    public class TeacherController : ApiControllerBase
    {
        private TeacherAppService  teacherApp => new TeacherAppService();

        [Route("GetAllTeacher")]
        public string PostGetAllTeacher()
        {
            try
            {
                var datas = teacherApp.GetList().Select(t => new
                {
                    t.F_Id,//教师ID
                    t.F_User_ID,//用户ID
                    t.F_Divis_ID,//隶属学部
                    t.F_Name,//教师姓名
                    t.F_Gender,//性别
                    t.F_Num,//教师工号
                    t.F_Nation,//国籍/地区
                    t.F_FacePhoto,//电子照片
                    t.F_CredType,//证件类型
                    t.F_CredNum,//证件号码
                    t.F_CredPhoto_Obve,//证件正面照片
                    t.F_CredPhoto_Rever,//证件反面照片
                    t.F_Birthday,//出生日期
                    t.F_Native,//籍贯
                    t.F_RegAddr,//出生地
                    t.F_Volk,//民族
                    t.F_PolitStatu,//政治面貌
                    t.F_Marrige,//婚姻状况
                    t.F_InWork_Date,//参加工作时间
                    t.F_EntryTime,//进校时间
                    t.F_Type_Teacher,//教职工类别
                    t.F_Profession,//科别
                    t.F_Academy,//毕业院校
                    t.F_Education,//最高学历
                    t.F_Duties,//职务
                    t.F_Titles,//职称
                });
                return Success(datas);
            }
            catch (System.Exception ex)
            {
                throw new ExceptionContext("1101", ex.Message);
            }
        }

        [Route("GetTeacherByNum")]
        public string PostGetTeacherByNum(GetTeacherByIdParm param)
        {
            try
            {
                var data = teacherApp.GetList(t => t.F_Num == param.TeacherNum)
                    .Select(t => new
                    {
                        t.F_Id,//教师ID
                        t.F_User_ID,//用户ID
                        t.F_Divis_ID,//隶属学部
                        t.F_Name,//教师姓名
                        t.F_Gender,//性别
                        t.F_Num,//教师工号
                        t.F_Nation,//国籍/地区
                        t.F_FacePhoto,//电子照片
                        t.F_CredType,//证件类型
                        t.F_CredNum,//证件号码
                        t.F_CredPhoto_Obve,//证件正面照片
                        t.F_CredPhoto_Rever,//证件反面照片
                        t.F_Birthday,//出生日期
                        t.F_Native,//籍贯
                        t.F_RegAddr,//出生地
                        t.F_Volk,//民族
                        t.F_PolitStatu,//政治面貌
                        t.F_Marrige,//婚姻状况
                        t.F_InWork_Date,//参加工作时间
                        t.F_EntryTime,//进校时间
                        t.F_Type_Teacher,//教职工类别
                        t.F_Profession,//科别
                        t.F_Academy,//毕业院校
                        t.F_Education,//最高学历
                        t.F_Duties,//职务
                        t.F_Titles,//职称
                    });
                return Success(data);
            }
            catch (System.Exception ex)
            {
                throw new ExceptionContext("1101", ex.Message);
            }
        }
    }
}