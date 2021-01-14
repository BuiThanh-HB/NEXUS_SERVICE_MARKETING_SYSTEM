using Data.DB;
using Data.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Business
{
    public class CustomerServicePlanBusiness : GenericBusiness
    {
        public CustomerServicePlanBusiness(NEXUS_SystemEntities context = null) : base()
        {

        }

        EmailBusiness email = new EmailBusiness();
        //Tiến trình tự động gửi mail thông báo gói cước sắp hết hạn tới khách hàng
        public void ReportServiceStatusToCus()
        {
            try
            {
                DateTime now = Util.ConvertDate(DateTime.Now.ToString(SystemParam.CONVERT_DATETIME)).Value;
                DateTime date = now.AddDays(7);
                List<CustomerServicePlan> sv = cnn.CustomerServicePlans.Where(c => c.IsActive.Equals(SystemParam.ACTIVE) && c.Status.Equals(SystemParam.ACTIVE) && c.ExpiryDate.HasValue && c.ExpiryDate <= date && (c.ExpiryDate.Value >= now ? true : false)).ToList();

                if (sv != null && sv.Count() > 0)
                {
                    foreach (var dt in sv)
                    {
                        email.configClient(dt.Customer.Email, "[NEXUS SYSTEM THÔNG BÁO]", "Gói cước " + dt.Order.ServicePlan.Name + " của bạn sẽ hết hạn trong ngày " + dt.ExpiryDate.Value.ToString(SystemParam.CONVERT_DATETIME) + ", vui lòng gia hạn để tiếp tục sử dụng dịch vụ");
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }



        }
    }
}