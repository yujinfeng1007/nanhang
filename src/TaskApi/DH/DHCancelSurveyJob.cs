using Quartz;
using System;

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
