using APIProject.Models;
using Data.DB;
using Data.Model;
using Data.Utils;
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
                string code = s.Category.Type + rd.Next(10000,99999) + rd.Next(10000, 99999);

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
            catch(Exception e)
            {
                e.ToString();
                return rp.serverError();
            }
        }
    }
}