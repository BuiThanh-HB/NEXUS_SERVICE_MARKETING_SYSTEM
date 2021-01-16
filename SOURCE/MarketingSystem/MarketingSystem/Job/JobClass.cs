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


        public async Task ReportServiceStatusToCus()
        {
            CustomerServicePlanBusiness plan = new CustomerServicePlanBusiness();
            //Gọi hàm gửi thông báo gói cước sắp hết hạn 
            plan.ReportServiceStatusToCus();
            //Gọi hàm cập nhật trạng thái gói cước khi đã hết hạn
            plan.UpdateServiceOfCus();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await Task.WhenAll(ReportServiceStatusToCus());
            }
            catch
            {
                return;
            }
        }
    }
}