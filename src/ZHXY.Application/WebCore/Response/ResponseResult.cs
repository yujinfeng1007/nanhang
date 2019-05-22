using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using ZHXY.Common;

namespace ZHXY.Application
{
    //移动端接口包装类
    public class ApiResult
    {
        public ApiResult()
        {
        }
       
        public bool IsError { get; set; }

        public string ErrorCodeValue { get; set; }

        public string ErrorMsgInfo { get; set; }

        public object Body { get; set; }
    }


    public static class ResultState
    {
        public const string Error = "error";
        public const string Success = "success";
        public const string Info = "info";
        public const string Warning = "warning";
    }

    public static class Result
    {
        public static ActionResult Success(object data = null)
        {
            return new ContentResult { Content = new { state = ResultState.Success,data }.ToCamelJson(), ContentEncoding = Encoding.UTF8, ContentType = "application/json" };
        }

        public static ActionResult PagingRst(this object rows, int records, int total)
        {
            return new ContentResult { Content = new { rows, records, total, state = ResultState.Success }.ToCamelJson(), ContentEncoding = Encoding.UTF8, ContentType = "application/json" };
        }

        public static ActionResult PagingRst<T>(this List<T> rows)
        {
            return new ContentResult { Content = new { rows, state = ResultState.Success }.ToCamelJson(), ContentEncoding = Encoding.UTF8, ContentType = "application/json" };
        }
    }
}