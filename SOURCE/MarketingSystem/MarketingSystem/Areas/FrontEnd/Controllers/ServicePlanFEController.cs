using APIProject.Controllers;
using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketingSystem.Areas.FrontEnd.Controllers
{
    public class ServicePlanFEController : BaseController
    {
        // GET: FrontEnd/ServicePlanFE
        public ActionResult Index()
        {
            return View();
        }

        //Đăng ký dịch vụ
        public JsonResult CreateOrder(OrderInputModel input)
        {
            return Json(orderBusiness.CreateOrder(input), JsonRequestBehavior.AllowGet);
        }
    }
}