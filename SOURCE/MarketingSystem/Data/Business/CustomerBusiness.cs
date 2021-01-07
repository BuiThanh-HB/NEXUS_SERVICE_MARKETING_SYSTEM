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
    public class CustomerBusiness : GenericBusiness
    {
        ResponseBusiness rp = new ResponseBusiness();
        public CustomerBusiness(NEXUS_SystemEntities context = null) : base()
        {

        }
        public JsonResultModel Register(RegisterCustomerInputModel input)
        {
            try
            {
                if (cnn.Customers.Any(x => (x.Phone == input.phone || x.Email == input.email) && x.IsActive))
                {
                  return  rp.response(0, 0, "Email hoặc số điện thoại đã tồn tại", null);
     
                }
                else
                {
                    Customer cus = new Customer();
                    cus.Address = input.address;
                    cus.CreatedDate = DateTime.Now;
                    cus.Email = input.email;
                    cus.DistrictID = input.district_id;
                    cus.ProvinceID = input.province_id;
                    cus.Phone = input.phone;
                    cus.Password = Util.GenPass(input.password);
                    cus.VillageID = input.village_id;
                    cus.Name = input.name;
                    cus.Token = Util.CreateMD5(DateTime.Now.ToString());
                    cus.IsActive = true;
                    cnn.Customers.Add(cus);
                    cnn.SaveChanges();
                  return  rp.response(1, 1, "Thêm khách hàng thành công", null);
              
                }
            }catch(Exception ex)
            {
                return rp.serverError();
            }
        }


        public List<SelectOptionModel> GetListProvince()
        {
            return cnn.Provinces.Select(x => new SelectOptionModel
            {

                id = x.ID,
                value = x.Name
            }).ToList();
        }
        public List<SelectOptionModel> GetListDistrict(int province_id)
        {
            return cnn.Districts.Where(c=>c.ProvinceID==province_id).Select(x => new SelectOptionModel
            {

                id = x.ID,
                value = x.Name
            }).ToList();
        }
        public List<SelectOptionModel> GetListVillage(int district_id)
        {
            return cnn.Villages.Where(c=>c.DistrictID==district_id).Select(x => new SelectOptionModel
            {

                id = x.ID,
                value = x.Name
            }).ToList();
        }

        public JsonResultModel Login(string userName,string password)
        {
            try
            {
         
                var data = cnn.Customers.Where(x => x.IsActive && (x.Phone == userName || x.Email == userName)).FirstOrDefault();
                if (data == null)
                    return rp.response(0, 0, "Tài khoản hoặc mật khẩu không lợp lệ", null);
                if(!Util.CheckPass(password,data.Password))
                    return rp.response(0, 0, "Tài khoản hoặc mật khẩu không lợp lệ", null);
                else
                {
                    HttpContext.Current.Session["Client"] = data;/// chỗ này thích lưu cái gì thì tự lưu nhé
                     return rp.response(1, 1, "Thành coong", null);

                }    
            }
            catch
            {
                return rp.serverError();
            }
        }
    }
}