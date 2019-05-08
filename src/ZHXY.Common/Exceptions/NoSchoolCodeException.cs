using System;

namespace ZHXY.Common
{
    public class NoLoggedInException : Exception
    {
        public NoLoggedInException() : base("未登录!")
        {
        }
    }
}