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
            if (client != null)
            {
                ViewBag.cusID = client.Id;
            }
            else
            {
                ViewBag.cusID = 0;
            }

            ViewBag.ListCategory = servicePlanBusiness.GetListCategory();
            return View();
        }

        public PartialViewResult SearchService(int Page = 1, string Name = "", int CateID = 0)
        {
            try
            {
                var data = servicePlanBusiness.SearchFontEnd(Page, Name, 1, CateID);
                return PartialView("_Service", data);
            }
            catch
            {
                return PartialView("_Service", new List<ListServicePlanOutputModel>().ToPagedList(1, 1));
            }
        }

        [HttpGet]
        public PartialViewResult ServiceDetail(int ID)
        {

            //ViewBag.LisProvince = customerBusiness.GetListProvince();
            //var data = servicePlanBusiness.ServiceDetail(ID);
            return PartialView("_ServiceDetail", null);
        }
        //public JsonResult ServiceDetail(int ID)
        //{
        //    try
        //    {
        //        ViewBag.LisProvince = customerBusiness.GetListProvince();
        //        var data = servicePlanBusiness.ServiceDetail(ID);
        //        return Json(data, JsonRequestBehavior.AllowGet);
        //    }
        //    catch
        //    {
        //        return Json(null, JsonRequestBehavior.AllowGet);
        //    }
        //}
        //Đăng ký dịch vụ
        [HttpPost]
        public JsonResult CreateOrder(int ServiceID, string Note, int ProvinceID, int DistrictID, int VillageID, string Address)
        {
            OrderInputModel input = new OrderInputModel();
            input.ServiceID = ServiceID;
            input.token = client.Token;
            input.Note = Note;
            input.ProvinceID = ProvinceID;
            input.DistrictID = DistrictID;
            input.VillageID = VillageID;
            input.Address = Address;

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