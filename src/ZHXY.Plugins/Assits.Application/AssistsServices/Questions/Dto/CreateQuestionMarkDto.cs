using ZHXY.Assists.Entity;
using ZHXY.Common;

namespace ZHXY.Assists.Application
{
    /// <summary>
    /// 创建问题标记
    /// </summary>
    public class CreateQuestionMarkDto
    {
        public string Name { get; set; }

        public static implicit operator QuestionMark(CreateQuestionMarkDto input)
        {
            return input.MapTo<QuestionMark>();
        }
    }
}