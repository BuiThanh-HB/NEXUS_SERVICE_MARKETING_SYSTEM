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
                    cus.Status = SystemParam.ACTIVE;
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
                if (user.Status.Value.Equals(SystemParam.NO_ACTIVE))
                    return rp.response(0, 0, "Tài khoản của bạn tạm thời bị khóa", null);
                else
                {
                    LoginOutputModel data = new LoginOutputModel();
                    string token = Util.CreateMD5(DateTime.Now.ToString());
                    data.Id = user.ID;
                    data.Token = token;
                    data.Name = user.Name;
                    user.Token = token;
                    cnn.SaveChanges();
                    HttpContext.Current.Session["Client"] = data;
                    return rp.response(1, 1, "Thành công", null);

                }
            }
            catch
            {
                return rp.serverError();
            }
        }

        //Tìm kiếm thông tin khách hàng
        public IPagedList<ListCustomerOutputModel> Search(int page, string searchKey, string fromDate, string toDate, Boolean? status)
        {
            try
            {
                DateTime? fd = Util.ConvertDate(fromDate);
                DateTime? td = Util.ConvertDate(toDate);
                if (td.HasValue)
                    td = td.Value.AddDays(1);
                var data = cnn.Customers.Where(c => c.IsActive.Equals(SystemParam.ACTIVE) && (!String.IsNullOrEmpty(searchKey) ? c.Name.Contains(searchKey) || c.Phone.Contains(searchKey) : true)
                && (fd.HasValue ? c.CreatedDate >= fd.Value : true) && (td.HasValue ? c.CreatedDate <= td.Value : true) && (status.HasValue ? c.Status == status : true))
                    .Select(c => new ListCustomerOutputModel
                    {
                        ID = c.ID,
                        Name = c.Name,
                        Phone = c.Phone,
                        CreatedDate = c.CreatedDate,
                        StatusStr = c.Status.Value.Equals(SystemParam.ACTIVE) ? "Hoạt động" : "Tạm khóa",
                        Address = String.IsNullOrEmpty(c.Address) ? c.Village.Name + "-" + c.District.Name + "-" + c.Province.Name : c.Address
                    }).OrderByDescending(c => c.ID).ToPagedList(page, SystemParam.MAX_ROW_IN_LIST);
                return data;
            }
            catch
            {
                return new List<ListCustomerOutputModel>().ToPagedList(1, 1);
            }
        }

        //Khóa tài khoản khách hàng
        public JsonResultModel ChangeStatus(int id)
        {
            try
            {
                EmailBusiness email = new EmailBusiness();
                Customer cus = cnn.Customers.Find(id);
                cus.Status = !cus.Status.Value;
                cnn.SaveChanges();
                if (cus.Status.Equals(SystemParam.NO_ACTIVE))
                    email.configClient(cus.Email, "[NEXUS SYSTEM THÔNG BÁO]", "Tài khoản của bạn tạm thời bị khóa");
                return rp.response(SystemParam.SUCCESS, SystemParam.SUCCESS_CODE, "Thành công", null);
            }
            catch
            {
                return rp.serverError();
            }
        }
        public int ChangePassword(int ID, string CurrentPassword, string NewPassword)
        {
            try
            {
                var passUser = cnn.Customers.Where(u => u.IsActive == SystemParam.ACTIVE && u.ID.Equals(ID)).FirstOrDefault();

                if (passUser != null && !Util.CheckPass(CurrentPassword, passUser.Password))
                {
                    return SystemParam.WRONG_PASSWORD;
                }

                Customer cus = cnn.Customers.Find(ID);
                cus.Password = Util.GenPass(NewPassword);
                cnn.SaveChanges();
                return SystemParam.SUCCESS;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return SystemParam.ERROR;
            }
        }

        // 
        public CustomerOutPutMode CusDetail(string token)
        {
            try
            {
                var data = cnn.Customers.Where(c => c.Token.Equals(token) && c.IsActive.Equals(SystemParam.ACTIVE))
                    .Select(cus => new CustomerOutPutMode
                    {
                        ID = cus.ID,
                        Name = cus.Name,
                        Address = cus.Address,
                        ProvinceID = cus.ProvinceID,
                        Province = cus.Province.Name,
                        DistrictID = cus.DistrictID,
                        District = cus.District.Name,
                        VillageID = cus.VillageID,
                        Village = cus.Village.Name,
                        Email = cus.Email
                    }).FirstOrDefault();
                if(data!= null)
                {
                    return data;
                }
                return new CustomerOutPutMode();
            }
            catch
            {
                return new CustomerOutPutMode();
            }
        }
        // cập nhâp thông tin
        public int UpdateCusInfor (CustomerOutPutMode input)
        {
            try
            {
                return 1;
            }
            catch
            {
                return 0;
            }
        }
    }
}