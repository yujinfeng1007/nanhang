using ZHXY.Common;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System;

namespace ZHXY.Application
{
    /// <summary>
    /// 学校代码和设备序列号过滤
    /// </summary>
    public class SchoolCodeAndSnFilter : ActionFilterAttribute
    {
        private bool CheckSchoolCode { get; }
        private bool CheckSn { get; }

        public SchoolCodeAndSnFilter(bool checkSchoolCode = true, bool checkSn = true)
        {
            CheckSchoolCode = checkSchoolCode;
            CheckSn = checkSn;
        }

        public SchoolCodeAndSnFilter()
        {
            CheckSchoolCode = true;
            CheckSn = true;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var args = actionContext.ActionArguments.Values;
            BaseApiInput inputArgs;
            foreach (var item in args)
            {
                if (!(item is BaseApiInput)) continue;
                inputArgs = item as BaseApiInput;
                if (CheckSchoolCode)
                {
                    if (string.IsNullOrEmpty(inputArgs.F_School_Id)) throw new Exception("学校代码不能为空!");
                }

                if (CheckSn)
                {
                    if (string.IsNullOrEmpty(inputArgs.F_Sn)) throw new Exception("序列号不能为空!");
                }

                OperatorProvider.Set(new OperatorModel { SchoolCode = inputArgs.F_School_Id });
                break;
            }
        }
    }
}