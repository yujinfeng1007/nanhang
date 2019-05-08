using System.Web.Http;
using ZHXY.Application;
using ZHXY.Assists.Application;

namespace ZHXY.Assits.Web
{
    /// <summary>
    /// 辅助屏
    /// </summary>

    public class AssistController : BaseAssistController
    {
        private AssistAppService App => new AssistAppService();

        /// <summary>
        /// 1.2.辅助屏绑定教室
        /// </summary>
        [HttpPost]
        public IHttpActionResult BindRoom(BindRoominput input)
        {
            var data = App.Bind(input);
            return SUCCESS(data);
        }

        /// <summary>
        /// 1.3.辅助屏教师登录
        /// </summary>
        [HttpPost]
        public IHttpActionResult TeacherLogin(TeacherLoginInput input)
        {
            var data = App.TeacherLogin(input);
            return SUCCESS(data);
        }

        /// <summary>
        /// 1.4.获取教室当天课程表
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetRoomCourseOfDay(GetRoomCourseOfDayInput input)
        {
            var data = App.GetRoomCourseOfDay(input);
            return SUCCESS(data);
        }

        /// <summary>
        /// 1.5.获取课程学生信息
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetCourseStudents(GetCourseStudentsInput input)
        {
            var data = App.GetCourseStudents(input);
            return SUCCESS(data);
        }

        /// <summary>
        /// 1.6.获取课堂最新问题
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetCourseLatestQuestions(GetCourseLatestQuestionsInput input)
        {
            var data = App.GetCourseLatestQuestions(input);
            return SUCCESS(data);
        }

        /// <summary>
        /// 1.7.下发课件
        /// </summary>
        [HttpPost]
        public IHttpActionResult DistributeCourseware(DistributeCoursewareInput input)
        {
            App.DistributeCourseware(input);
            return SUCCESS();
        }

        /// <summary>
        /// 1.9.获取未绑定辅助屏的教室信息
        /// </summary>
        [HttpPost]
        public IHttpActionResult GetUnBindRoomInfo(BaseApiInput input)
        {
            var data = App.GetUnboundRoom(input);
            return SUCCESS(data);
        }
    }
}