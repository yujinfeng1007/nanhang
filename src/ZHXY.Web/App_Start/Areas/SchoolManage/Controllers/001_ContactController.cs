using NFine.Code;
using NFine.Domain.Entity.MailManage;
using System;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    /// <summary>
    /// 个人通讯录管理
    /// </summary>
    public class ContactController : ControllerBase
    {
        private ContactApp App { get; } = new ContactApp();

        /// <summary>
        /// 添加联系人
        /// </summary>
        /// <returns></returns>
        public ActionResult Add(Contact contact)
        {
            try
            {
                var userID = OperatorProvider.Provider.UserID();
                if (string.IsNullOrEmpty(userID)) throw new Exception("未登录用户!");
                contact.User = userID;
                App.Add(contact);
                return Success();
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 修改联系人
        /// </summary>
        /// <returns></returns>
        public ActionResult Update(Contact contact)
        {
            try
            {
                var userID = OperatorProvider.Provider.UserID();
                if (string.IsNullOrEmpty(userID)) throw new Exception("未登录用户!");
                if (contact.User != userID) throw new Exception("该联系人不属于你!");
                App.Update(contact);
                return Success();
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 删除联系人
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete(string id)
        {
            try
            {
                var userID = OperatorProvider.Provider.UserID();
                if (string.IsNullOrEmpty(userID)) throw new Exception("未登录用户!");
                var arr = id.Split(new char[] { ',', ';' });
                App.Delete(arr, userID);
                return Success();
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 联系人列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List(Pagination pagination, string keyword)
        {
            try
            {
                var userID = OperatorProvider.Provider.UserID();
                if (string.IsNullOrEmpty(userID)) throw new Exception("未登录用户!");
                var list = App.List(pagination, userID, keyword);
                var data = new
                {
                    pagination.Total,
                    pagination.Page,
                    pagination.Records,
                    rows = list
                };
                return Data(data);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 获取联系人
        /// </summary>
        public ActionResult Get(string id)
        {
            try
            {
                var userID = OperatorProvider.Provider.UserID();
                if (string.IsNullOrEmpty(userID)) throw new Exception("未登录用户!");
                var data = App.Get(id);
                return Data(data);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
    }
}