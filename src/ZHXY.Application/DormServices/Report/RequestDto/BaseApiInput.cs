using System.ComponentModel.DataAnnotations;

namespace ZHXY.Application
{
    /// <summary>
    /// WebApi 输入数据模型
    /// </summary>
    public class BaseApiInput
    {
        /// <summary>
        /// 设备序列号
        /// </summary>
        [Required(ErrorMessage = "班牌序列号不能为空!")]
        public string F_Sn { get; set; }

        /// <summary>
        /// 学校id
        /// </summary>
        [Required(ErrorMessage = "学校代码不能为空!")]
        public string F_School_Id { get; set; }
    }
}