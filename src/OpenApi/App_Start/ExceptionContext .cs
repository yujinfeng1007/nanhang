using System;

namespace OpenApi
{
    public class ExceptionContext : SystemException
    {
        public ExceptionContext()
        {
        }
        public ExceptionContext(string errorCodeValue, string errorMsgInfo)
        {
            ErrorCodeValue = errorCodeValue;
            ErrorMsgInfo = errorMsgInfo;
            IsError = true;
        }
        public bool IsError { get; set; }
        public string ErrorCodeValue { get; set; }
        public string ErrorMsgInfo { get; set; }

        public string Body { get; set; }
    }
}