using NFine.Code;
using System;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    /// <summary>
    /// 邮件管理
    /// </summary>
    public class MailController : ControllerBase
    {
        private MailApp App { get; } = new MailApp();

        /// <summary>
        /// 删除邮件
        /// </summary>
        public ActionResult DeleteMail(string idArr)
        {
            try
            {
                var userID = OperatorProvider.Provider.UserID();
                if (string.IsNullOrEmpty(userID)) throw new Exception("用户未登录!");
                var arr = idArr.Split(new char[] { ',', ';' });
                App.Delete(arr);
                return Success();
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 获取邮件
        /// </summary>
        public ActionResult Get(string id)
        {
            try
            {
                var userID = OperatorProvider.Provider.UserID();
                if (string.IsNullOrEmpty(userID)) throw new Exception("用户未登录!");
                var data = App.Get(id);
                return Content(data.ToJson());
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 全部标记为已读
        /// </summary>
        /// <returns></returns>
        public ActionResult MarkAllRead()
        {
            try
            {
                var userID = OperatorProvider.Provider.UserID();
                if (string.IsNullOrEmpty(userID)) throw new Exception("用户未登录!");
                App.MarkAllRead();
                return Success();
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 标记为已读
        /// </summary>
        public ActionResult MarkRead(string id)
        {
            try
            {
                var userID = OperatorProvider.Provider.UserID();
                if (string.IsNullOrEmpty(userID)) throw new Exception("用户未登录!");
                var arr = id.Split(new char[] { ',', ';' });
                App.MarkRead(arr);
                return Success();
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 标记为未读
        /// </summary>
        public ActionResult MarkUnread(string id)
        {
            try
            {
                var userID = OperatorProvider.Provider.UserID();
                if (string.IsNullOrEmpty(userID)) throw new Exception("用户未登录!");
                var arr = id.Split(new char[] { ',', ';' });
                App.MarkUnread(arr);
                return Success();
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 移动到回收站
        /// </summary>
        public ActionResult MoveToTrashBox(string idArr)
        {
            try
            {
                var userID = OperatorProvider.Provider.UserID();
                if (string.IsNullOrEmpty(userID)) throw new Exception("用户未登录!");
                var arr = idArr.Split(new char[] { ',', ';' });
                App.MoveToTrashBox(arr);
                return Success();
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 移动到草稿箱
        /// </summary>
        [HttpPost]
        public ActionResult MoveDraftBox(string id)
        {
            try
            {
                var userID = OperatorProvider.Provider.UserID();
                if (string.IsNullOrEmpty(userID)) throw new Exception("用户未登录!");
                var arr = id.Split(new char[] { ',', ';' });
                App.MoveDraftBox(arr);
                return Success();
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 保存到草稿
        /// </summary>
        [HttpPost]
        public ActionResult SaveToDraftBox(string id, string to, string cc, string bcc, string subject, string content, string attachments)
        {
            try
            {
                var userID = OperatorProvider.Provider.UserID();
                if (string.IsNullOrEmpty(userID)) throw new Exception("用户未登录!");
                App.SaveToDraftBox(userID, id, to, cc, bcc, subject, content, attachments);
                return Success();
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        [HttpPost]
        public ActionResult Send(string to, string cc, string bcc, string subject, string content, string attachments)
        {
            try
            {
                var userID = OperatorProvider.Provider.UserID();
                if (string.IsNullOrEmpty(userID)) throw new Exception("用户未登录!");
                App.SendMail(userID, to, cc, bcc, subject, content, attachments);
                return Success();
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 草稿箱
        /// </summary>
        public ActionResult DraftBox(Pagination pagination, string keyword)
        {
            try
            {
                var userID = OperatorProvider.Provider.UserID();
                if (string.IsNullOrEmpty(userID)) throw new Exception("用户未登录!");
                var data = new
                {
                    rows = App.DraftBox(pagination, userID, keyword),
                    pagination.Total,
                    pagination.Page,
                    pagination.Records,
                };
                return Content(data.ToJson());
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 收件箱
        /// </summary>
        public ActionResult InBox(Pagination pagination, string keyword)
        {
            try
            {
                var userID = OperatorProvider.Provider.UserID();
                if (string.IsNullOrEmpty(userID)) throw new Exception("用户未登录!");
                var data = new
                {
                    rows = App.InBox(pagination, userID, keyword),
                    pagination.Total,
                    pagination.Page,
                    pagination.Records,
                };
                return Content(data.ToJson());
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 发件箱
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public ActionResult OutBox(Pagination pagination, string keyword)
        {
            try
            {
                var userID = OperatorProvider.Provider.UserID();
                if (string.IsNullOrEmpty(userID)) throw new Exception("用户未登录!");
                var data = new
                {
                    rows = App.OutBox(pagination, userID, keyword),
                    pagination.Total,
                    pagination.Page,
                    pagination.Records,
                };
                return Content(data.ToJson());
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 回收站
        /// </summary>
        public ActionResult TrashBox(Pagination pagination, string keyword)
        {
            try
            {
                var userID = OperatorProvider.Provider.UserID();
                if (string.IsNullOrEmpty(userID)) throw new Exception("用户未登录!");
                var data = new
                {
                    rows = App.TrashBox(pagination, userID, keyword),
                    pagination.Total,
                    pagination.Page,
                    pagination.Records,
                };
                return Content(data.ToJson());
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 收件
        /// </summary>
        public ActionResult ReceiveMail()
        {
            try
            {
                var userID = OperatorProvider.Provider.UserID();
                if (string.IsNullOrEmpty(userID)) throw new Exception("用户未登录!");
                App.ReceiveMail(userID);
                return Success();
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult ReceiveMailTest(string userID)
        {
            try
            {
                App.ReceiveMail(userID);
                return Success();
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
    }
}