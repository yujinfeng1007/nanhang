using ZHXY.Assists.Entity;
using ZHXY.Common;

namespace ZHXY.Assists.Application
{
    /// <summary>
    /// 创建课堂问题
    /// </summary>
    public class SubmitQuestionDto
    {
        /// <summary>
        /// 科目Id
        /// </summary>
        public string CourseId { get; set; }

        /// <summary>
        /// 问题
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 问题标记
        /// </summary>
        public string MarkId { get; set; }

        public static implicit operator Question(SubmitQuestionDto input)
        {
            return input.MapTo<Question>();
        }
    }
}