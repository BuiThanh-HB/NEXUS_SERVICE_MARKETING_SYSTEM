using APIProject.Controllers;
using Data.Business;
using Data.DB;
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

        public PartialViewResult SearchService(int Page = 1, string Name = "", int CateID = 0)
        {
            try
            {
                ViewBag.ListCategory = servicePlanBusiness.GetListCategory();
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

            var listProvince = customerBusiness.GetListProvince();
            var listDistrict = customerBusiness.GetListDistrict(listProvince.FirstOrDefault().id);
            var listVillage = customerBusiness.GetListVillage(listDistrict.FirstOrDefault().id);
            ViewBag.ListDistrict = listDistrict;
            ViewBag.ListVillage = listVillage;
            ViewBag.LisProvince = listProvince;
            var data = servicePlanBusiness.ServiceDetail(ID);
            return PartialView("_ServiceDetail", data);
        }

        //Đăng ký dịch vụ
        [HttpPost]
        public int CreateOrder(int ServiceID, string Note, int ProvinceID, int DistrictID, int VillageID, string Address)
        {
            if (client == null)
            {
                return -1;
            }
            OrderInputModel input = new OrderInputModel();
            input.ServiceID = ServiceID;
            input.token = client.Token;
            input.Note = Note;
            input.ProvinceID = ProvinceID;
            input.DistrictID = DistrictID;
            input.VillageID = VillageID;
            input.Address = Address;
            var data = orderBusiness.CreateOrder(input);

            return data.Status;
        }

        public ActionResult CusService()
        {
            return View("CusService");
        }

        public PartialViewResult SearchCusService(int Page, string SearchKey, string Code, int? Status, string Fromdate, string Todate)
        {
            try
            {

                if (client != null)
                {
                    var data = customerServicePlan.Myservice(client.Id, Page, SearchKey, Code, Status, Fromdate, Todate);
                    return PartialView("TableCusService", data);
                }
                return PartialView("TableCusService", new List<CustomerService>().ToPagedList(1, 1));

            }
            catch
            {
                return PartialView("TableCusService", new List<CustomerService>().ToPagedList(1, 1));
            }
        }
    }
}