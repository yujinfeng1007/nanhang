using log4net;
using System.Web.Http;
using ZHXY.Common;

namespace OpenApi
{
    [HandlerAuthorize]
    public abstract class ApiControllerBase : ApiController
    {
        public ILog FileLog
        {
            get { return Logger.GetLogger(GetType().ToString()); }
        }

        public string Success(object data)
        {
            return new ResultContext(data).ToJson();
        }

        public string Error(string errorCodeValue, string errorMsgInfo)
        {
            return new ResultContext(errorCodeValue, errorMsgInfo).ToJson();
        }
    }
}