using APIProject.Models;
using Data.DB;
using Data.Model;
using Data.Utils;
using OfficeOpenXml;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Data.Business
{
    public class OrderBusiness : GenericBusiness
    {
        ResponseBusiness rp = new ResponseBusiness();
        public OrderBusiness(NEXUS_SystemEntities context = null) : base()
        {

        }

        //Tìm kiếm thông tin đơn hàng
        public IPagedList<ListOrderOutputModel> Search(int page, string searchKey, string fromDate, string toDate, int? status)
        {
            try
            {
                DateTime? fd = Util.ConvertDate(fromDate);
                DateTime? td = Util.ConvertDate(toDate);
                if (td.HasValue)
                    td = td.Value.AddDays(1);
                var data = cnn.Orders.Where(o => o.IsActive.Equals(SystemParam.ACTIVE) && (!String.IsNullOrEmpty(searchKey) ? o.Code.Contains(searchKey) : true)
                && (fd.HasValue ? o.CreatedDate >= fd.Value : true) && (td.HasValue ? o.CreatedDate <= td.Value : true) && (status.HasValue && status.Value >= 0 ? o.Status.Equals(status.Value) : true))
                    .Select(o => new ListOrderOutputModel
                    {
                        ID = o.ID,
                        CusName = o.Customer.Name,
                        CusPhone = o.Customer.Phone,
                        CreatedDate = o.CreatedDate,
                        Status = o.Status,
                        Code = o.Code
                    }).OrderByDescending(o => o.ID).ToPagedList(page, SystemParam.MAX_ROW_IN_LIST);
                return data;
            }
            catch
            {
                return new List<ListOrderOutputModel>().ToPagedList(1, 1);
            }
        }

        //Lấy thông tin chi tiết đơn hàng
        public OrderDetailOuputModel GetOrderDetail(int id)
        {
            try
            {
                OrderDetailOuputModel data = new OrderDetailOuputModel();
                Order o = cnn.Orders.Find(id);
                data.ID = o.ID;
                data.CusName = o.Customer.Name;
                data.CusPhone = o.Customer.Phone;
                data.CusEmail = o.Customer.Email;
                data.Code = o.Code;
                data.TotalPrice = o.TotalPrice;
                data.Discount = o.Discount;
                data.CreatedDate = o.CreatedDate;
                data.Note = o.Note;
                data.AdminNote = !String.IsNullOrEmpty(o.AdminNote) ? o.AdminNote : "Chưa cập nhật";
                data.ServiceName = o.ServicePlan.Name;
                data.LocationRequest = !String.IsNullOrEmpty(o.Address) ? o.Village.Name + "-" + o.District.Name + "-" + o.Province.Name : o.Address;
                data.Status = o.Status;
                data.DiscountValue = o.DiscountValue.HasValue ? o.DiscountValue.Value : 0;
                data.UserHandleName = o.UserHandleID.HasValue ? o.User.Name : "Chưa cập nhật";
                return data;
            }
            catch
            {
                return new OrderDetailOuputModel();
            }

        }

        //Cập nhật đơn hàng
        public JsonResultModel UpdateBill(OrderDetailOuputModel input, int userID, int role)
        {
            try
            {
                EmailBusiness email = new EmailBusiness();
                if (role.Equals(SystemParam.ROLE_TECHNICAL_STAFF))
                    return rp.response(SystemParam.ERROR, SystemParam.FAIL, "Bạn không được phép sửa đơn hàng", null);
                Order o = cnn.Orders.Find(input.ID);

                switch (input.Status)
                {
                    case SystemParam.PENDING:

                        o.DiscountValue = input.DiscountValue;
                        o.Discount = input.Discount;
                        o.TotalPrice = input.TotalPrice;
                        o.AdminNote = input.AdminNote;
                        if (o.Status != input.Status)
                        {
                            email.configClient(o.Customer.Email, "[NEXUS SYSTEM THÔNG BÁO]", "Đơn hàng " + o.Code + " của bạn đang chờ xác nhận");
                        }
                        o.Status = input.Status;
                        o.UserHandleID = userID;
                        break;
                    case SystemParam.ACCEPT:
                        o.DiscountValue = input.DiscountValue;
                        o.Discount = input.Discount;
                        o.TotalPrice = input.TotalPrice;
                        o.AdminNote = input.AdminNote;
                        if (o.Status != input.Status)
                        {
                            email.configClient(o.Customer.Email, "[NEXUS SYSTEM THÔNG BÁO]", "Đơn hàng " + o.Code + " của bạn đã được xác nhận");
                        }
                        o.Status = input.Status;
                        o.UserHandleID = userID;
                        break;

                    case SystemParam.COMPLETE:

                        o.DiscountValue = input.DiscountValue;
                        o.Discount = input.Discount;
                        o.TotalPrice = input.TotalPrice;
                        o.AdminNote = input.AdminNote;


                        if (o.Status != input.Status)
                        {
                            //Lưu lịch sử
                            List<HistoryCustomerServicePlan> history = new List<HistoryCustomerServicePlan>();
                            HistoryCustomerServicePlan h = new HistoryCustomerServicePlan();

                            h.Note = "Dịch vụ dược kích hoạt";
                            h.UserID = userID;
                            h.IsActive = SystemParam.ACTIVE;
                            h.CreatedDate = DateTime.Now;
                            history.Add(h);

                            //Kích hoạt gói cước
                            CustomerServicePlan c = new CustomerServicePlan();
                            c.CustomerID = o.CustomerID;
                            c.ActiveDate = DateTime.Now;
                            c.ExtendDate = DateTime.Now;
                            c.ExpiryDate = DateTime.Now.AddMonths(o.ServicePlan.Value);
                            c.CreatedDate = DateTime.Now;
                            c.code = o.Code;
                            c.OrderID = o.ID;
                            c.Status = SystemParam.ACTIVE_STATUS;
                            c.IsActive = SystemParam.ACTIVE;
                            c.HistoryCustomerServicePlans = history;
                            c.Address = !String.IsNullOrEmpty(o.Address) ? o.Customer.Village.Name + " " + o.Customer.District.Name + " " + o.Customer.Province.Name : o.Address;
                            cnn.CustomerServicePlans.Add(c);


                            email.configClient(o.Customer.Email, "[NEXUS SYSTEM THÔNG BÁO]", "Đơn hàng " + o.Code + " của bạn đã được hoàn thành");
                        }

                        o.Status = input.Status;
                        o.UserHandleID = userID;

                        break;

                    case SystemParam.CANCEL:

                        o.DiscountValue = input.DiscountValue;
                        o.Discount = input.Discount;
                        o.TotalPrice = input.TotalPrice;
                        o.AdminNote = input.AdminNote;

                        if (o.Status != input.Status)
                        {
                            email.configClient(o.Customer.Email, "[NEXUS SYSTEM THÔNG BÁO]", "Đơn hàng " + o.Code + " của bạn bị trừ chối vì lý do: " + input.AdminNote);
                        }
                        o.Status = input.Status;
                        break;
                    default:
                        break;
                }
                cnn.SaveChanges();
                return rp.response(SystemParam.SUCCESS, SystemParam.SUCCESS_CODE, SystemParam.SUCCESS_MESSAGE, "");
            }
            catch (Exception e)
            {
                e.ToString();
                return rp.serverError();
            }
        }

        //Đăng ký dịch vụ
        public JsonResultModel CreateOrder(OrderInputModel input)
        {
            try
            {
                //Kiểm tra token 
                Customer cus = cnn.Customers.Where(c => c.IsActive.Equals(SystemParam.ACTIVE) && c.Token.Equals(input.token)).FirstOrDefault();
                if (cus == null)
                    return rp.response(SystemParam.ERROR, SystemParam.FAIL, SystemParam.TOKEN_NOT_FOUND, "");
                ServicePlan s = cnn.ServicePlans.Find(input.ServiceID);
                Order o = new Order();
                Random rd = new Random();
                string code = s.Category.Type + rd.Next(10000, 99999) + rd.Next(10000, 99999);

                o.Code = code;
                o.CustomerID = cus.ID;
                o.ProvinceID = input.ProvinceID;
                o.DistrictID = input.DistrictID;
                o.VillageID = input.VillageID;
                o.TotalPrice = s.Price;
                o.Discount = s.Price;
                o.CreatedDate = DateTime.Now;
                o.ServicePlanID = s.ID;
                o.Status = SystemParam.PENDING;
                o.Note = input.Note;
                o.IsActive = SystemParam.ACTIVE;
                o.Address = input.Address;
                cnn.Orders.Add(o);
                cnn.SaveChanges();
                return rp.response(SystemParam.SUCCESS, SystemParam.SUCCESS_CODE, SystemParam.SUCCESS_MESSAGE, "");
            }
            catch (Exception e)
            {
                e.ToString();
                return rp.serverError();
            }
        }

        //Xuất hóa đơn 
        public ExcelPackage ExportBill(int id)
        {
            try
            {
                OrderDetailOuputModel data = GetOrderDetail(id);
                string path = HttpContext.Current.Server.MapPath(@"/Template/Bill.xlsx");
                FileInfo file = new FileInfo(path);
                ExcelPackage pack = new ExcelPackage(file);

                ExcelWorksheet sheet = pack.Workbook.Worksheets[1];


                sheet.Cells[4, 2].Value = data.Code;
                sheet.Cells[5, 2].Value = data.CreatedDate.ToString(SystemParam.CONVERT_DATETIME_HAVE_HOUR);
                sheet.Cells[6, 2].Value = data.ServiceName;
                sheet.Cells[7, 2].Value = data.LocationRequest;
                sheet.Cells[8, 2].Value = String.Format("0n:0", data.TotalPrice);
                sheet.Cells[9, 2].Value = data.AdminNote;
                sheet.Cells[10, 2].Value = data.DiscountValue;
                sheet.Cells[11, 2].Value = data.Discount;
                sheet.Cells[12, 2].Value = data.UserHandleName;
                sheet.Cells[16, 2].Value = data.CusName;
                sheet.Cells[17, 2].Value = data.CusPhone;
                sheet.Cells[18, 2].Value = data.CusEmail;

                return pack;
            }
            catch (Exception e)
            {
                e.ToString();
                return null;
            }
        }
    }
}