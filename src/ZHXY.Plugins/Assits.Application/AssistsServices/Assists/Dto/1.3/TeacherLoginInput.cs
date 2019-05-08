using ZHXY.Application;
namespace ZHXY.Assists.Application
{
    /// <summary>
    /// 1.3.辅助屏教师登录
    /// </summary>
    public class TeacherLoginInput : BaseApiInput
    {
        /// <summary>
        /// 登录账户
        /// </summary>
        public string F_User { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string F_Pwd { get; set; }
    }
}