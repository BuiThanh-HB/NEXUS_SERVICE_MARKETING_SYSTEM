using APIProject.Controllers;
using Data.Model;
using Data.Model.APIWeb;
using PagedList;
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
            ViewBag.ListCategory = servicePlanBusiness.GetListCategory();
            return View();
        }

        public PartialViewResult SearchService(int Page = 1, string Name = "" , int CateID = 0)
        {
            try
            {
                var data = servicePlanBusiness.SearchFontEnd(Page, Name, 1, CateID);
                return PartialView("_Service",data);
            }
            catch
            {
                return PartialView("_Service", new List<ListServicePlanOutputModel>().ToPagedList(1, 1));
            }
        }
        
        public PartialViewResult ServiceDetail(int ID)
        {
            try
            {
                var data = servicePlanBusiness.ServiceDetail(ID);
                return PartialView("_ServiceDetail",data);
            }
            catch
            {
                return PartialView("_ServiceDetail", new ListServicePlanOutputModel());
            }
        }
        //Đăng ký dịch vụ
        [HttpPost]
        public JsonResult CreateOrder(OrderInputModel input)
        {
            input.token = client.Token;
            return Json(orderBusiness.CreateOrder(input), JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult CusService()
        {
            try
            {
                var data = new List<ListServicePlanOutputModel>().ToPagedList(1, 1);
                return PartialView("CusService", data);
            }
            catch
            {
                return PartialView("CusService", new List<ListServicePlanOutputModel>().ToPagedList(1, 1));
            }
        }
    }
}