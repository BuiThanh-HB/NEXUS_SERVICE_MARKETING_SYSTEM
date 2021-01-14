using Quartz;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using Data.Business;

namespace MarketingSystem.Job
{
    public class JobClass : IJob
    {


        public async Task SendNoti()
        {
            CustomerServicePlanBusiness plan = new CustomerServicePlanBusiness();
            plan.ReportServiceStatusToCus();
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