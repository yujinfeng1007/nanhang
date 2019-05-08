namespace OpenApi
{
    public class ResultContext
    {
        public ResultContext()
        {
        }
        public ResultContext(object body)
        {
            Body = body;
            IsError = false;
        }

        public ResultContext(string errorCodeValue, string errorMsgInfo)
        {
            ErrorCodeValue = errorCodeValue;
            ErrorMsgInfo = errorMsgInfo;
            IsError = true;
        }
        public bool IsError { get; set; }
        public string ErrorCodeValue { get; set; }
        public string ErrorMsgInfo { get; set; }

        public object Body { get; set; }
    }
}