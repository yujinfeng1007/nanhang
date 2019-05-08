using OpenApi.Parms.School;
using System.Linq;
using System.Web.Http;
using ZHXY.Application;
namespace OpenApi.Controllers.Sys
{
    [RoutePrefix("api/Student/Student")]
    public class StudentController : ApiControllerBase
    {
        private StudentAppService StudentApp => new StudentAppService();

        [Route("GetAllStudent")]
        public string PostGetAllStudent()
        {
            try
            {
                var objs = StudentApp.GetList().Select(t => new
                {
                    t.F_Id,//学生ID
                    t.F_Divis_ID,//学部ID
                    t.F_Grade_ID,//年级ID
                    t.F_Class_ID,//班级ID
                    t.F_StudentNum, //学号
                    t.F_NationNum,//全国学籍号
                    t.F_InitDTM,//入学时间
                    t.F_Name, //姓名
                    t.F_Gender,//性别
                    t.F_Birthday,//出生日期
                    t.F_FacePic_File,//照片
                    t.F_Nation,//国籍
                    t.F_CredType, //证件类型
                    t.F_CredNum,//证件号码
                    t.F_CredPhoto_Obve,//证件正面照片
                    t.F_CredPhoto_Rever,//证件反面照片
                    t.F_Native,//籍贯
                    t.F_Volk,//民族
                    t.F_PolitStatu,//政治面貌
                    t.F_Home_Addr,//家庭住址
                    t.F_Tel,//联系电话
                    t.F_Father,//父亲姓名
                    t.F_FatherTel,//父亲联系电话
                    t.F_Mother,//母亲姓名
                    t.F_MotherTel, //母亲联系电话
                    //t.F_Guarder,//监护人姓名
                    //t.F_Guarder_Relation,//监护关系
                    //t.F_Guarder_Tel,//监护人联系电话
                    //t.F_Guarder_CredNum,//监护人证件号
                    //t.F_Guarder_CredType,//监护人证件类型
                    //t.F_Guarder_CredPhoto_Obve,//监护人证件正面
                    //t.F_Guarder_CredPhoto_Rever,//监护人证件反面
                    t.F_In_Memo//入学信息备注
                });
                return Success(objs);
            }
            catch (System.Exception ex)
            {
                throw new ExceptionContext("1101", ex.Message);
            }
        }

        [Route("GetStudentByNum")]
        public string PostGetStudentByNum(GetStudentByIdParm param)
        {
            try
            {
                var data = StudentApp.GetList(t => t.F_StudentNum == param.StudentNum).Select(t => new
                {
                    t.F_Id,//学生ID
                    t.F_Divis_ID,//学部ID
                    t.F_Grade_ID,//年级ID
                    t.F_Class_ID,//班级ID
                    t.F_StudentNum, //学号
                    t.F_NationNum,//全国学籍号
                    t.F_InitDTM,//入学时间
                    t.F_Name, //姓名
                    t.F_Gender,//性别
                    t.F_Birthday,//出生日期
                    t.F_FacePic_File,//照片
                    t.F_Nation,//国籍
                    t.F_CredType, //证件类型
                    t.F_CredNum,//证件号码
                    t.F_CredPhoto_Obve,//证件正面照片
                    t.F_CredPhoto_Rever,//证件反面照片
                    t.F_Native,//籍贯
                    t.F_Volk,//民族
                    t.F_PolitStatu,//政治面貌
                    t.F_Home_Addr,//家庭住址
                    t.F_Tel,//联系电话
                    t.F_Father,//父亲姓名
                    t.F_FatherTel,//父亲联系电话
                    t.F_Mother,//母亲姓名
                    t.F_MotherTel, //母亲联系电话
                    //t.F_Guarder,//监护人姓名
                    //t.F_Guarder_Relation,//监护关系
                    //t.F_Guarder_Tel,//监护人联系电话
                    //t.F_Guarder_CredNum,//监护人证件号
                    //t.F_Guarder_CredType,//监护人证件类型
                    //t.F_Guarder_CredPhoto_Obve,//监护人证件正面
                    //t.F_Guarder_CredPhoto_Rever,//监护人证件反面
                    t.F_In_Memo//入学信息备注
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