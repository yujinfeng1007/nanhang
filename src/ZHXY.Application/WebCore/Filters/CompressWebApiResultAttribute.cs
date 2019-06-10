using ZHXY.Common;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Linq;

namespace ZHXY.Application
{
    /// <summary>
    /// 对结果进行压缩处理
    /// </summary>
    public class CompressWebApiResultAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actContext)
        {
            var content = actContext.Response?.Content;
            var acceptEncodings = actContext.Request.Headers.AcceptEncoding.Where(p => p.Value == "gzip" || p.Value == "deflate").ToList();
            if (!acceptEncodings.Any() || null == content || actContext.Request.Method == HttpMethod.Options) return;
            var first = acceptEncodings.FirstOrDefault();
            if (null == first) return;
            var bytes = content.ReadAsByteArrayAsync().Result;
            switch (first.Value)
            {
                case "gzip":
                    actContext.Response.Content = new ByteArrayContent(CompressionHelper.GZipBytes(bytes));
                    actContext.Response.Content.Headers.Add("Content-Encoding", "gzip");
                    break;

                case "deflate":
                    actContext.Response.Content = new ByteArrayContent(CompressionHelper.DeflateBytes(bytes));
                    actContext.Response.Content.Headers.Add("Content-encoding", "deflate");
                    break;
            }
        }
    }

}