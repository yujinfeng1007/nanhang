using System;
using System.Linq;
using System.Web.Mvc;
using ZHXY.Application;

namespace ZHXY.Assits.Web.WeChat
{
    public class CourseController : ZhxyWebControllerBase
    {
        TeacherPrepareLessonAppService App { get; }
        public CourseController(TeacherPrepareLessonAppService app)
        {
            App = app;
        }

        /// <summary>
        /// 随堂课件
        /// </summary>
        /// <param name="F_Course_PrepareID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CourseBookList(string F_Course_PrepareID)
        {
            var entity = App.Get(F_Course_PrepareID);
            if (entity == null) throw new Exception("未备课");
            var list = entity.F_attachments.Split(',');
            var attachments = App.Read<SchAttachment>(p => list.Contains(p.F_Id));
            return Result(attachments);
        }

        /// <summary>
        /// 课件重命名
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdBookName(string bookId)
        {
            var entity = App.Read<SchAttachment>(p => p.F_Id.Equals(bookId)).FirstOrDefault();
            if (entity == null) throw new Exception("未找到该课件");
            return Result(entity.F_Name);
        }
        /// <summary>
        /// 删除课件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteBooks(string[] bookIds)
        {
            App.DeleteBooks(bookIds);
            return AjaxResult();
        }
    }
}