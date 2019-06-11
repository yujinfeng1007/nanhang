using System.Web.Http;
using ZHXY.Application;

namespace ZHXY.Api.Controllers
{
    public class DHController : ApiController
    {
        [HttpGet]
        public object F_DH_TEST(string name, string pname, int level)
        {

           
            //推送人员信息至大华
            var personMoudle = new PersonMoudle();
            personMoudle.id = 0;
            personMoudle.orgId = "org001";
            personMoudle.code = "888888";
            personMoudle.idCode = "360421199311144020";
            personMoudle.name = "Refuse12";
            personMoudle.roleId = "student001"; //teacher001
            personMoudle.sex = 1;
            personMoudle.dormitoryName = "1栋"; //默认分院
            personMoudle.dormitoryFloorName = "1栋1层"; //楼层  例如：3层
            personMoudle.dormitoryRoomName = "1栋104"; //宿舍号  例如：312
            personMoudle.photoUrl = "http://localhost:8080/file/5.jpg";
            return DHAccount.PUSH_DH_ADD_PERSON(personMoudle);



        }

        [HttpGet]
        public object DeletePerson()
        {
            return DHAccount.PUSH_DH_DELETE_PERSON(new string[] { "133475" });
        }

        [HttpGet]
        public object FindPerson(string idCode)
        {
            //查询人员信息  大华
            var personMoudleTest = new PersonMoudle();
            personMoudleTest.idCode = idCode;
            personMoudleTest.roleId = "temp";
            personMoudleTest.code = null;
            return DHAccount.SELECT_DH_PERSON(personMoudleTest);
        }

        /// <summary>
        /// 布控访客相关
        /// </summary>
        /// <param name="PicUrl"></param>
        /// <param name="idCode"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        public string TempSurvey(string PicUrl, string idCode, string name)
        {
            string[] str = { "1000004$7$0$0", "1000009$7$0$0", "1000013$7$0$0", "1000002$7$0$0", "1000010$7$0$0", "1000000$7$0$0", "1000012$7$0$0", "1000008$7$0$0", "1000011$7$0$0", "1000003$7$0$0" };
            var survey = new SurveyMoudle();
            survey.channelId = str;
            survey.code = "";
            survey.name = name;
            survey.sex =1;
            survey.idCode = idCode;
            survey.photoBase64 = GetImageBase64Str.ImageBase64Str(PicUrl); ;
            survey.initialTime = "2019-05-30 00:00:00";
            survey.expireTime = "2019-05-30 23:59:59";
            return DHAccount.TempSurvey(survey);
        }

        /// <summary>
        /// 撤控 
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet]
        public string CancelSurvey(string personId)
        {
            string[] str = { "1000004$7$0$0", "1000009$7$0$0", "1000013$7$0$0", "1000002$7$0$0", "1000010$7$0$0", "1000000$7$0$0", "1000012$7$0$0", "1000008$7$0$0", "1000011$7$0$0", "1000003$7$0$0" };
            return DHAccount.CancelSurvey(str, personId);
        }

        /// <summary>
        /// 一键常开、常闭
        /// </summary>
        /// <param name="type">开门的动作（int类型，必填）：1-开门，2关门，3-常开门，4常关门</param>  
        /// <param name="channelId">开门的通道Id(String类型，必填)</param>
        /// <returns></returns>
        [HttpGet]
        public string OpenDoor(int type, string channelId)
        {
            return DHAccount.OpenDoor(type, channelId);
        }
    }
}
