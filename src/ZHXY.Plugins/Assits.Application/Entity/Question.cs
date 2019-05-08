using ZHXY.Domain;

namespace ZHXY.Assists.Entity
{
    /// <summary>
    /// 课堂问题
    /// </summary>
    public class Question : CompleteEntity
    {
        /// <summary>
        /// 发问者Id
        /// </summary>
        public string AskerId { get; set; }

        /// <summary>
        /// 课堂Id
        /// </summary>
        public string CourseId { get; set; }

        /// <summary>
        /// 问题标记Id
        /// </summary>
        public string MarkId { get; set; }

        /// <summary>
        /// 问题内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 发问者
        /// </summary>
        public virtual User Asker { get; set; }

        /// <summary>
        /// 备课ID
        /// </summary>
        public virtual PrepareLesson Course { get; set; }

        /// <summary>
        /// 标记
        /// </summary>
        public virtual QuestionMark Mark { get; set; }
    }
}