using System;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    /// <summary>
    /// 学校邮箱配置
    /// </summary>
    public class SchoolMailController : ControllerBase
    {
        private SchoolMailApp App { get; } = new SchoolMailApp();

        /// <summary>
        /// 修改学校邮件服务器配置
        /// </summary>
        public ActionResult Update(string smtpHost, string smtpPort, string pop3Host, string pop3Port)
        {
            try
            {
                App.Update(smtpHost, smtpPort, pop3Host, pop3Port);
                return Success();
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 获取学校邮件服务器配置
        /// </summary>
        public ActionResult Get()
        {
            try
            {
                var data = App.Get();
                return Data(data);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
    }
}