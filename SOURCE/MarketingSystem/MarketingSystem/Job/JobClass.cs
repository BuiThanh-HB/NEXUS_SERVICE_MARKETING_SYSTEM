using Quartz;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;

namespace MarketingSystem.Job
{
    public class JobClass : IJob
    {


        public async Task SendNoti()
        {
            System.Diagnostics.Debug.WriteLine("job start");
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await Task.WhenAll(SendNoti());
            }
            catch
            {
                return;
            }
        }
    }
}