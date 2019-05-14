using System.Web.Http;
using ZHXY.Application;
using ZHXY.Common;

namespace ZHXY.Api.Controllers
{
    /// <summary>
    /// 微信
    /// </summary>
    [RoutePrefix("Api/Wechat")]
    public class WechatController : BaseApiController
    {
        ///// <summary>
        ///// 微信端登录绑定学生
        ///// </summary>
        ///// <param name="F_StudentNo">学号</param>
        ///// <param name="F_CheckCode">身份证后6位</param>
        ///// <returns></returns>
        //[Route("Post")]
        //[HttpPost]
        //public IHttpActionResult Post(string F_StudentNo, string F_CheckCode)
        //{
        //    var student = new StudentService().GetBykeyValue(F_StudentNo);
        //    var pwd = "000000";
        //    if (!student.CredNumber.IsEmpty() && student.CredNumber.Length >= 6)
        //    {
        //        pwd = student.CredNumber.Substring(student.CredNumber.Length - 6, 6);
        //        pwd = pwd.ToLower().Replace("x", "0");//密码x变为0
        //    }

        //    if (!pwd.ToLower().Equals(F_CheckCode.ToLower()))
        //    {
        //        return BadRequest("验证码错误");
        //    }

        //    return Ok("success");
        //}
    }
}