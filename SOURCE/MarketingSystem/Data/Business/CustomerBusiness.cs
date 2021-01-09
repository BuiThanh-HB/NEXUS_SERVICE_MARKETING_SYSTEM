using APIProject.Models;
using Data.DB;
using Data.Model;
using Data.Model.APIWeb;
using Data.Utils;
using PagedList;
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
                    return rp.response(0, 0, "Email hoặc số điện thoại đã tồn tại", null);

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
                    return rp.response(1, 1, "Thêm khách hàng thành công", null);

                }
            }
            catch (Exception ex)
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
            return cnn.Districts.Where(c => c.ProvinceID == province_id).Select(x => new SelectOptionModel
            {

                id = x.ID,
                value = x.Name
            }).ToList();
        }
        public List<SelectOptionModel> GetListVillage(int district_id)
        {
            return cnn.Villages.Where(c => c.DistrictID == district_id).Select(x => new SelectOptionModel
            {

                id = x.ID,
                value = x.Name
            }).ToList();
        }

        public JsonResultModel Login(string userName, string password)
        {
            try
            {

                var user = cnn.Customers.Where(x => x.IsActive && (x.Phone == userName || x.Email == userName)).FirstOrDefault();
                if (user == null)
                    return rp.response(0, 0, "Tài khoản hoặc mật khẩu không lợp lệ", null);
                if (!Util.CheckPass(password, user.Password))
                    return rp.response(0, 0, "Tài khoản hoặc mật khẩu không lợp lệ", null);
                else
                {
                    LoginOutputModel data = new LoginOutputModel();
                    string token = Util.CreateMD5(DateTime.Now.ToString());
                    data.Id = user.ID;
                    data.Token = user.Token;
                    data.Name = user.Name;
                    user.Token = token;
                    cnn.SaveChanges();
                    HttpContext.Current.Session["Client"] = data;/// chỗ này thích lưu cái gì thì tự lưu nhé
                    return rp.response(1, 1, "Thành công", null);

                }
            }
            catch
            {
                return rp.serverError();
            }
        }

        //Tìm kiếm thông tin khách hàng
        public IPagedList<ListCustomerOutputModel> Search(int page, string searchKey, string fromDate, string toDate)
        {
            try
            {
                DateTime? fd = Util.ConvertDate(fromDate);
                DateTime? td = Util.ConvertDate(toDate);
                if (td.HasValue)
                    td = td.Value.AddDays(1);
                var data = cnn.Customers.Where(c => c.IsActive.Equals(SystemParam.ACTIVE) && (!String.IsNullOrEmpty(searchKey) ? c.Name.Contains(searchKey) || c.Phone.Contains(searchKey) : true)
                && (fd.HasValue ? c.CreatedDate >= fd.Value : true) && (td.HasValue ? c.CreatedDate <= td.Value : true))
                    .Select(c => new ListCustomerOutputModel
                    {
                        ID = c.ID,
                        Name = c.Name,
                        Phone = c.Phone,
                        CreatedDate = c.CreatedDate,
                        Address = String.IsNullOrEmpty(c.Address) ? c.Village.Name + " " + c.District.Name + " Tỉnh " + c.Province.Name : c.Address
                    }).OrderByDescending(c => c.ID).ToPagedList(page, SystemParam.MAX_ROW_IN_LIST);
                return data;
            }
            catch
            {
                return new List<ListCustomerOutputModel>().ToPagedList(1, 1);
            }
        }
    }
}