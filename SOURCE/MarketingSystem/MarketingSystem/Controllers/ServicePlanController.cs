using APIProject.App_Start;
using APIProject.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketingSystem.Controllers
{
    public class ServicePlanController : BaseController
    {
        // GET: ServicePlan
        [UserAuthenticationFilter]
        public ActionResult Index()
        {
            //Lấy danh sách danh mục
            ViewBag.listCate = servicePlanBusiness.GetListCategory();
            return View();
        }
    }
}