using APIProject.Controllers;
using Data.Model;
using Data.Model.APIWeb;
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

        public PartialViewResult SearchService(int Page = 1, string Name = "")
        {
            try
            {
                var data = servicePlanBusiness.GetListCategory();
                return PartialView("_Service",data);
            }
            catch
            {
                return PartialView("_Service"/*new List<ListPostCategory>().ToPagedList(1, 1)*/ );
            }
        }
        //Đăng ký dịch vụ
        [HttpPost]
        public JsonResult CreateOrder(OrderInputModel input)
        {
            input.token = client.Token;
            return Json(orderBusiness.CreateOrder(input), JsonRequestBehavior.AllowGet);
        }
    }
}