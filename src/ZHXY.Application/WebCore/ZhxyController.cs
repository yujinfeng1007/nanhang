using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using log4net;
using ZHXY.Common;

namespace ZHXY.Application
{
    //[LoginAuthentication]
    [ValidateParam]
    public abstract class ZhxyController : Controller
    {
        #region property

        protected ILog FileLog => Logger.GetLogger(GetType().ToString());

        #endregion property
        #region View

        [HttpGet]
        [HandlerAuthorize]
        public virtual async Task<ViewResult> Index() => await Task.Run(() => View());

        [HttpGet]
        public virtual async Task<ViewResult> Form() => await Task.Run(() => View());

        [HttpGet]
        public virtual async Task<ViewResult> Details() => await Task.Run(() => View());

        [HttpGet]
        public virtual async Task<ViewResult> Approve() => await Task.Run(() => View());

        [HttpGet]
        public virtual async Task<ViewResult> Return() => await Task.Run(() => View());

        [HttpGet]
        public virtual async Task<ViewResult> Import() => await Task.Run(() => View());

        #endregion View

        #region others
        protected DbParameter[] CreateParms(IDictionary<string, string> parms)
        {
            return parms.Select(kp => new SqlParameter("@" + kp.Key, kp.Value)).Cast<DbParameter>().ToArray();
        }
        #endregion others
    }
}