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

        //Cập nhật trạng thái các gói cước về ngừng hoạt động khi gói cước đã hết hạn
        public void UpdateServiceOfCus()
        {
            DateTime now = Util.ConvertDate(DateTime.Now.ToString(SystemParam.CONVERT_DATETIME)).Value;
            List<CustomerServicePlan> sv = cnn.CustomerServicePlans.Where(c => c.IsActive.Equals(SystemParam.ACTIVE) && c.Status.Equals(SystemParam.ACTIVE) && c.ExpiryDate.HasValue && c.ExpiryDate.Value.Day == now.Day && c.ExpiryDate.Value.Month == now.Month).ToList();
            List<HistoryCustomerServicePlan> histories = new List<HistoryCustomerServicePlan>();

            if (sv != null && sv.Count() > 0)
            {
                foreach(var dt in sv)
                {
                    email.configClient(dt.Customer.Email, "[NEXUS SYSTEM THÔNG BÁO]", "Gói cước " + dt.Order.ServicePlan.Name + " của bạn đã không được gia hạn và đã bị dừng hoạt động và ngày " + dt.ExpiryDate.Value.ToString(SystemParam.CONVERT_DATETIME));
                    dt.Status = SystemParam.NO_ACTIVE;

                    //Lưu lại lịch sử gói cước
                    HistoryCustomerServicePlan h = new HistoryCustomerServicePlan();
                    h.Note = "Gói cước " + dt.Order.ServicePlan.Name + " của bạn đã không được gia hạn và đã bị dừng hoạt động và ngày " + dt.ExpiryDate.Value.ToString(SystemParam.CONVERT_DATETIME);
                    h.UserID = 1;
                    h.IsActive = SystemParam.ACTIVE;
                    h.CreatedDate = DateTime.Now;
                    histories.Add(h);
                }

                cnn.HistoryCustomerServicePlans.AddRange(histories);
                cnn.SaveChanges();
            }
        }
    }
}