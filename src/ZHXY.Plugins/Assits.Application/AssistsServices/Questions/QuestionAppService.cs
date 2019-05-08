using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ZHXY.Application;
using ZHXY.Domain;
using ZHXY.Assists.Entity;
using ZHXY.Common;
using ZHXY.Repository;

namespace ZHXY.Assists.Application
{
    /// <summary>
    /// 课堂问题服务
    /// </summary>
    public class QuestionAppService : AppService
    {
        public QuestionAppService(IAssistManageRepository repos) => R = repos;

        public QuestionAppService() => R = new AssistManageRepository();

        #region question

        public void Submit(SubmitQuestionDto input)
        {
            AddAndSave<Question>(input);
        }

        public void Update(UpdateQuestionDto input)
        {
            var question = Get<Question>(input.Id);
            input.MapTo(question);
            SaveChanges();
        }

        public void Delete(string id)
        {
            DelAndSave<Question>(id);
        }

        public List<Question> GetByCourseId(string courseId)
        {
            return Read<Question>(p => p.CourseId.Equals(courseId)).ToListAsync().Result;
        }

        #endregion question

        #region question mark

        public void AddMark(CreateQuestionMarkDto input)
        {
            AddAndSave<QuestionMark>(input);
        }

        public void DeleteMark(string id)
        {
            DelAndSave<QuestionMark>(id);
        }

        public void RenameMark(RenameMarkDto input)
        {
            var mark = Get<QuestionMark>(input.Id);
            input.MapTo(mark);
            SaveChanges();
        }

        public List<QuestionMarkView> GetSelfMarks(string userId)
        {
            return Read<QuestionMark>(p => p.CreatedByUserId.Equals(userId))
                .Select(p => new QuestionMarkView
                {
                    Id = p.Id,
                    Name = p.Name
                }).ToList();
        }

        public List<QuestionMarkView> GetMarks(string userId)
        {
            return Read<QuestionMark>()
                .Select(p => new QuestionMarkView
                {
                    Id = p.Id,
                    Name = p.Name
                }).ToList();
        }

        #endregion question mark
    }
}