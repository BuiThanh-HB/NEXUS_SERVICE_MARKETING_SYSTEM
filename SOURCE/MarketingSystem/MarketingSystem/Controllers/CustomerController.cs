using APIProject.App_Start;
using APIProject.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketingSystem.Controllers
{
    public class CustomerController : BaseController
    {
        // GET: Customer
        [UserAuthenticationFilter]
        public ActionResult Index()
        {
            return View();
        }

        //Tìm kiếm thông tin khách hàng
        public PartialViewResult Search(int page, string searchKey, string fromDate, string toDate,Boolean? status)
        {
            ViewBag.searchKey = searchKey;
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            ViewBag.status = status;
            return PartialView("_TableCustomer", customerBusiness.Search(page, searchKey, fromDate, toDate,status));
        }

        //Khóa tài khoản của khách hàng
        public JsonResult BlockAccount(int id)
        {
            return Json(customerBusiness.BlockAccount(id), JsonRequestBehavior.AllowGet);
        }
    }
}