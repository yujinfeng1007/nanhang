using ZHXY.Assists.Entity;
using ZHXY.Common;

namespace ZHXY.Assists.Application
{
    /// <summary>
    /// 标记改名
    /// </summary>

    public class RenameMarkDto
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public static implicit operator QuestionMark(RenameMarkDto input)
        {
            return input.MapTo<QuestionMark>();
        }
    }
}