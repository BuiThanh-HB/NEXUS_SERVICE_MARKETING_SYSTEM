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
                return data;
            }
            catch
            {
                return new OrderDetailOuputModel();
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
    }
}