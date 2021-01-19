using APIProject.Controllers;
using Data.Model;
using Data.Model.APIWeb;
using Data.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketingSystem.Areas.FrontEnd.Controllers
{
    public class HomeFrontEndController : BaseController
    {
        // GET: FrontEnd/HomeFrontEnd
        public ActionResult Index()
        {
            HomeFrontEndOutputModel data = new HomeFrontEndOutputModel();
            data.ListNews = newsBusiness.ListNewsWeb();
            data.ListService = servicePlanBusiness.ListServiceHome();
            return View(data);
        }
        public ActionResult Register()
        {
            var listProvince = customerBusiness.GetListProvince(); 
            var listDistrict= customerBusiness.GetListDistrict(listProvince.FirstOrDefault().id);
            var listVillage= customerBusiness.GetListVillage(listDistrict.FirstOrDefault().id);
            ViewBag.ListDistrict = listDistrict;
            ViewBag.ListVillage = listVillage;
            return View(listProvince);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Login(string userName, string password)
        {
            return Json(customerBusiness.Login(userName,password),JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Register(RegisterCustomerInputModel input)
        {
            return Json(customerBusiness.Register(input), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetListDistrict(int province_id)
        {
            return Json(customerBusiness.GetListDistrict(province_id), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetListVillage(int district_id)
        {
            return Json(customerBusiness.GetListVillage(district_id), JsonRequestBehavior.AllowGet);
        }


        // chi tiết người dùng
        public PartialViewResult CusDetail()
        {
            try
            {
                if(client != null)
                {
                    var data = customerBusiness.CusDetail(client.Token);
                    return PartialView("CusDetail", data);
                }
                return PartialView("CusDetail");
            }
            catch
            {
                return PartialView("CusDetail");
            }
        }
        // cập nhập thông tin
        [HttpPost]
        public int UpdateCusInfor( string Name , string Address , string Email , int ProvinceID , int DistrictID , int VillageID)
        {
            try
            {
                if(client == null)
                {
                    return -2;
                }

                CustomerOutPutMode input = new CustomerOutPutMode();
                input.ID = client.Id;
                input.Name = Name;
                input.Address = Address;
                input.Email = Email;
                input.ProvinceID = ProvinceID;
                input.DistrictID = DistrictID;
                input.VillageID = VillageID;

                var data = customerBusiness.UpdateCusInfor(input);
                return data;
            }
            catch
            {
                return 0;
            }
        }

        //đăng xuất
        public int Logout()
        {
            try
            {
                Session[SystemParam.CLIENT] = null;
                return SystemParam.SUCCESS;
            }
            catch
            {
                return SystemParam.ERROR;
            }
        }
        public int ChangePassword(string CurrentPassword, string NewPassword)
        {
            try
            {
                LoginOutputModel cus = client;
                if(cus != null)
                {
                    return customerBusiness.ChangePassword(cus.Id, CurrentPassword, NewPassword);
                }
                return SystemParam.ERROR;
            }
            catch
            {
                return SystemParam.ERROR;
            }
        }
    }
}