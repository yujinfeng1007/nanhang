using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using ZHXY.Common;

namespace ZHXY.Application
{
    //移动端接口包装类
    [Serializable]
    public class ApiResult
    {
        public ApiResult()
        {
        }
        public ApiResult(object body)
        {
            Body = body == null ? "[]" : body.ToJson();
            IsError = false;
        }
        public ApiResult(string errorCodeValue, string errorMsgInfo)
        {
            ErrorCodeValue = errorCodeValue;
            ErrorMsgInfo = errorMsgInfo;
            Body = "[]";
        }
        public bool IsError { get; set; }

        //错误代码
        //0000 正确
        //0001 错误
        public string ErrorCodeValue { get; set; }

        //错误描述
        public string ErrorMsgInfo { get; set; }

        //消息体 json 串
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

        public static ActionResult PagingRst<T>(this List<T> rows, int records, int total)
        {
            return new ContentResult { Content = new { rows, records, total, state = ResultState.Success }.ToCamelJson(), ContentEncoding = Encoding.UTF8, ContentType = "application/json" };
        }

        public static ActionResult PagingRst<T>(this List<T> rows)
        {
            return new ContentResult { Content = new { rows, state = ResultState.Success }.ToCamelJson(), ContentEncoding = Encoding.UTF8, ContentType = "application/json" };
        }
    }
}