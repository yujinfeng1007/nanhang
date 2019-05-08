/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: NFine
 * Description: NFine快速开发平台
 * Website：http://www.nfine.cn
*********************************************************************************/

using System.Web.Mvc;

namespace NFine.Web.Areas.ExampleManage.Controllers
{
    public class SendMailController : ControllerBase
    {
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult SendMail(string account, string title, string content)
        {
            //MailHelper mail = new MailHelper();
            //mail.MailServer = Configs.GetValue("MailHost");
            //mail.MailUserName = Configs.GetValue("MailUserName");
            //mail.MailPassword = Configs.GetValue("MailPassword");
            //mail.MailName = "NFine快速开发平台";
            //mail.Send(account, title, content);
            return Success("发送成功。");
        }
    }
}