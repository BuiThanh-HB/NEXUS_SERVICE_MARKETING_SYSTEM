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
        //Tìm kiếm thông tin gói cước
        [HttpGet]

        public PartialViewResult Search(int page, string searchKey, Boolean? status, int? cateID, string fromDate, string toDate)
        {
            ViewBag.searchKey = searchKey;
            ViewBag.status = status;
            ViewBag.cateID = cateID;
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            return PartialView("_TableServicePlan", servicePlanBusiness.Search(page, searchKey, status, cateID, fromDate, toDate));
        }
    }
}