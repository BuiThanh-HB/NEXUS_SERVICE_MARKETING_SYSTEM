using APIProject.Models;
using Data.DB;
using Data.Model;
using Data.Utils;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Data.Business
{
    public class CustomerServicePlanBusiness : GenericBusiness
    {
        ResponseBusiness rp = new ResponseBusiness();
        EmailBusiness email = new EmailBusiness();
        public CustomerServicePlanBusiness(NEXUS_SystemEntities context = null) : base()
        {

        }


        //Tìm kiếm thông tin gói cước của khách hàng
        public IPagedList<ListServicePlanOfCus> Search(int page, string searchKey, string code, int? status, string fromDate, string toDate)
        {
            try
            {
                DateTime? fd = Util.ConvertDate(fromDate);
                DateTime? td = Util.ConvertDate(toDate);
                if (td.HasValue)
                    td = td.Value.AddDays(1);
                var data = cnn.CustomerServicePlans.Where(c => c.IsActive.Equals(SystemParam.ACTIVE) && (!String.IsNullOrEmpty(searchKey) ? c.Customer.Name.Contains(searchKey) || c.Customer.Phone.Contains(searchKey) : true)
                && (!String.IsNullOrEmpty(code) ? c.code.Equals(code) : true) && (fd.HasValue ? c.CreatedDate >= fd.Value : true) && (td.HasValue ? c.CreatedDate <= td.Value : true) && (status.HasValue && status.Value >= 0 ? c.Status.Equals(status) : true))
                    .Select(c => new ListServicePlanOfCus
                    {
                        ID = c.ID,
                        Code = c.code,
                        ServiceName = c.Order.ServicePlan.Name,
                        CusName = c.Customer.Name,
                        CreatedDate = c.CreatedDate,
                        ActiveDate = c.ActiveDate,
                        Status = c.Status.HasValue ? c.Status.Value : 0,
                        LocaRequest = !String.IsNullOrEmpty(c.Address) ? c.Customer.Village.Name + " " + c.Customer.District.Name + " " + c.Customer.Province.Name : c.Address,

                    }).OrderByDescending(c => c.ID).ToPagedList(page, SystemParam.MAX_ROW_IN_LIST);
                return data;
            }
            catch
            {
                return new List<ListServicePlanOfCus>().ToPagedList(1, 1);
            }

        }


        //Lấy chi tiết gói cước của khách hàng
        public CustomerServicePlanDetailModel GetCustomerServicePlanDetail(int id)
        {
            try
            {
                CustomerServicePlanDetailModel data = new CustomerServicePlanDetailModel();
                CustomerServicePlan c = cnn.CustomerServicePlans.Find(id);
                data.ID = c.ID;
                data.Code = c.code;
                data.CusName = c.Customer.Name;
                data.ServiceName = c.Order.ServicePlan.Name;
                data.LocaRequest = !String.IsNullOrEmpty(c.Address) ? c.Customer.Village.Name + " " + c.Customer.District.Name + " " + c.Customer.Province.Name : c.Address;
                data.ActiveDate = c.ActiveDate;
                data.Status = c.Status.Value;
                data.histories = c.HistoryCustomerServicePlans.Where(h => h.IsActive.Equals(SystemParam.ACTIVE)).Select(h => new HistoryCustomerServicePlan
                {
                    Note = h.Note,
                    Status = h.Status,
                    CreatedDate = h.CreatedDate
                }).OrderByDescending(h => h.ID).ToList();
                return data;
            }
            catch
            {
                return new CustomerServicePlanDetailModel();
            }


        }



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
                foreach (var dt in sv)
                {
                    email.configClient(dt.Customer.Email, "[NEXUS SYSTEM THÔNG BÁO]", "Gói cước " + dt.Order.ServicePlan.Name + " của bạn đã không được gia hạn và đã bị dừng hoạt động vào ngày " + dt.ExpiryDate.Value.ToString(SystemParam.CONVERT_DATETIME));
                    dt.Status = SystemParam.NO_ACTIVE_STATUS;

                    //Lưu lại lịch sử gói cước
                    HistoryCustomerServicePlan h = new HistoryCustomerServicePlan();
                    h.Note = "Gói cước " + dt.Order.ServicePlan.Name + " của bạn đã không được gia hạn và đã bị dừng hoạt động vào ngày " + dt.ExpiryDate.Value.ToString(SystemParam.CONVERT_DATETIME);
                    h.UserID = 1;
                    h.IsActive = SystemParam.ACTIVE;
                    h.CreatedDate = DateTime.Now;
                    histories.Add(h);
                }

                cnn.HistoryCustomerServicePlans.AddRange(histories);
                cnn.SaveChanges();
            }
        }

        //Cập nhật thông tin gói cước của khách hàng
        public JsonResultModel UpdateCustomerServicePlan(CustomerServicePlanDetailModel input)
        {
            try
            {
                CustomerServicePlan c = cnn.CustomerServicePlans.Find(input.ID);
                HistoryCustomerServicePlan h = new HistoryCustomerServicePlan();
                string content = "";

                switch (input.Type)
                {
                    //Cập nhật trạng thái của gói cước
                    case 1:
                        if (!input.Status.Equals(c.Status.Value))
                        {
                            switch (input.Status)
                            {
                                case SystemParam.NO_ACTIVE_STATUS:

                                    //Lưu lại lịch sử gói cước
                                    h.UserID = input.UserID;
                                    h.Note = !String.IsNullOrEmpty(input.Note) ? input.Note : "";
                                    h.IsActive = SystemParam.ACTIVE;
                                    h.CustomerServicePlanID = c.ID;
                                    h.Status = input.Status;
                                    h.CreatedDate = DateTime.Now;
                                    cnn.HistoryCustomerServicePlans.Add(h);
                                    content = "Gói cước " + c.Order.ServicePlan.Name + " của bạn đã bị ngừng hoạt động";
                                    break;

                                case SystemParam.ACTIVE_STATUS:

                                    h.UserID = input.UserID;
                                    h.Note = !String.IsNullOrEmpty(input.Note) ? input.Note : "";
                                    h.CustomerServicePlanID = c.ID;
                                    h.IsActive = SystemParam.ACTIVE;
                                    h.Status = input.Status;
                                    h.CreatedDate = DateTime.Now;
                                    cnn.HistoryCustomerServicePlans.Add(h);
                                    content = "Gói cước " + c.Order.ServicePlan.Name + " của bạn đã được hoạt động trở lại";
                                    break;
                                default: break;
                            }
                        }
                        c.Status = input.Status;
                        break;

                    //Gia gạn thêm cho gói cước
                    case 2:
                        c.Status = SystemParam.ACTIVE_STATUS;

                        h.UserID = input.UserID;
                        h.Note = !String.IsNullOrEmpty(input.Note) ? input.Note : "";
                        h.IsActive = SystemParam.ACTIVE;
                        h.CreatedDate = DateTime.Now;
                        cnn.HistoryCustomerServicePlans.Add(h);
                        c.ExtendDate = DateTime.Now;
                        h.CustomerServicePlanID = c.ID;
                        h.Status = SystemParam.ACTIVE_STATUS;
                        c.ExpiryDate = DateTime.Now.AddMonths(c.Order.ServicePlan.Value);
                        h.Status = SystemParam.ACTIVE_STATUS;
                        content = "Gói cước " + c.Order.ServicePlan.Name + " của bạn đã được gia hạn thêm";

                        break;
                    default:
                        break;
                }
                cnn.SaveChanges();
                if (!String.IsNullOrEmpty(content))
                    email.configClient(c.Customer.Email, "[NEXUS SYSTEM THÔNG BÁO]", content);
                return rp.response(SystemParam.SUCCESS, SystemParam.SUCCESS_CODE, "Thành công", "");

            }
            catch(Exception e)
            {
                e.ToString();
                return rp.serverError();
            }
        }


        public IPagedList<CustomerService> Myservice(int cusID, int page , string searchKey , string code, int? status, string fromDate, string toDate )
        {
            try
            {
                DateTime? fd = Util.ConvertDate(fromDate);
                DateTime? td = Util.ConvertDate(toDate);
                if (td.HasValue)
                    td = td.Value.AddDays(1);
                var data = cnn.CustomerServicePlans.Where(c => c.IsActive.Equals(SystemParam.ACTIVE)
                        && c.CustomerID.Equals(cusID)
                        && (!String.IsNullOrEmpty(searchKey) ? c.Order.ServicePlan.Name.Contains(searchKey) : true)
                        && (!String.IsNullOrEmpty(code) ? c.code.Equals(code) : true)
                        && (fd.HasValue ? c.CreatedDate >= fd.Value : true)
                        && (td.HasValue ? c.CreatedDate <= td.Value : true)
                        && (status.HasValue && status.Value >= 0 ? c.Status == status : true)
                        )
                    .Select(c => new CustomerService
                    {
                        ID = c.ID,
                        Code = c.code,
                        ServiceName = c.Order.ServicePlan.Name,
                        ActiveDate = c.ActiveDate,
                        ExpiryDate = c.ExpiryDate,
                        ExtendDate = c.ExtendDate,
                        ImageUrl = c.Order.ServicePlan.ImageUrl,
                        OrderID = c.OrderID,
                        ServiceID = c.Order.ServicePlanID,
                        Value = c.Order.ServicePlan.Value,
                        Price = c.Order.TotalPrice,
                        Status = c.Status.HasValue ? c.Status.Value : 0,
                    }).OrderByDescending(cd => cd.ID).ToPagedList(page, SystemParam.COUNT_LIST_WEB);


                return data;
            }
            catch(Exception e)
            {
                e.ToString();
                return new List<CustomerService>().ToPagedList(1, 1);
            }

        }

        public JsonResultModel CusDelSercicePlan(int id)
        {
            try
            {
                CustomerServicePlan s = cnn.CustomerServicePlans.Find(id);
                s.Status = SystemParam.NO_ACTIVE_STATUS;
                cnn.SaveChanges();
                return rp.response(SystemParam.SUCCESS, SystemParam.SUCCESS_CODE, SystemParam.SUCCESS_MESSAGE, "");
            }
            catch
            {
                return rp.serverError();
            }
        }
    }
}