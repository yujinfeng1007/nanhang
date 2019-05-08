using ZHXY.Assists.Entity;
using ZHXY.Common;

namespace ZHXY.Assists.Application
{
    /// <summary>
    /// 修改课堂问题
    /// </summary>
    public class UpdateQuestionDto : SubmitQuestionDto
    {
        public string Id { get; set; }

        public static implicit operator Question(UpdateQuestionDto input)
        {
            return input.MapTo<Question>();
        }
    }
}