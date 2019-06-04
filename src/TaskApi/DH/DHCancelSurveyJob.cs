using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using ZHXY.Common;
using ZHXY.Domain;

namespace TaskApi.DH
{
    public class DHCancelSurveyJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("date " + DateTime.Now);
        }
    }
}
