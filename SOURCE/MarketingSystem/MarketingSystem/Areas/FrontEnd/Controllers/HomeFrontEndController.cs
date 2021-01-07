using APIProject.Controllers;
using Data.Model;
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
            return View();
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
    }
}