using NFine.Code;
using System;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    /// <summary>
    /// 用户邮箱配置
    /// </summary>
    public class UserEmailController : ControllerBase
    {
        /// <summary>
        /// 用户邮箱设置
        /// </summary>
        /// <returns></returns>
        public ActionResult SetEmail(string address, string password)
        {
            try
            {
                var userID = OperatorProvider.Provider.UserID();
                if (string.IsNullOrEmpty(userID)) throw new Exception("未登录用户!");
                new UserMailApp().SetEmail(userID, address, password);
                return Success();
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 获取用户邮箱配置
        /// </summary>
        public ActionResult GetEmailConfig()
        {
            try
            {
                var userID = OperatorProvider.Provider.UserID();
                if (string.IsNullOrEmpty(userID)) throw new Exception("未登录用户!");
                var data = new UserMailApp().Get(userID);
                return Data(data);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
    }
}