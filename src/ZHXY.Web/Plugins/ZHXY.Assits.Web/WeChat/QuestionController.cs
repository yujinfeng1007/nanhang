using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Assists.Application;

namespace ZHXY.Assits.Web
{
    /// <summary>
    /// 互动问答,F_Course_PrepareID==备课ID，以备课ID为主键
    /// </summary>
    public class QuestionController : ZhxyWebControllerBase
    {
        private QuestionAppService App { get; }

        public QuestionController(QuestionAppService app)
        {
            App = app;
        }

        /// <summary>
        /// 提交问题
        /// </summary>
        public void Submit(SubmitQuestionDto input)
        {
            App.Submit(input);
        }

        /// <summary>
        /// 问题列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List(string courseId)
        {
            var data = App.GetByCourseId(courseId);
            return Result(data);
        }

        /// <summary>
        /// 标记
        /// </summary>
        /// <returns></returns>
        public void AddMark(CreateQuestionMarkDto input)
        {
            App.AddMark(input);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public void DeleteMark(string id)
        {
            App.DeleteMark(id);
        }

        public ActionResult GetSelfMarks(string userId)
        {
            var data = App.GetSelfMarks(userId);
            return Result(data);
        }

        public ActionResult GetMarks(string userId)
        {
            var data = App.GetMarks(userId);
            return Result(data);
        }
    }
}