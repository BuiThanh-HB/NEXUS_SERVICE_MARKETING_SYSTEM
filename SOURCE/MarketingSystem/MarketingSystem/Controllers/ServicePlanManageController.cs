using APIProject.App_Start;
using APIProject.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketingSystem.Controllers
{
    public class ServicePlanManageController : BaseController
    {
        // GET: ServicePlanManage
        [UserAuthenticationFilter]
        public ActionResult Index()
        {
            return View();
        }

        //Tìm kiếm thông tin gói cước của khách hàng
        public PartialViewResult Search(int page, string searchKey, string code, int? status, int? cateID, string fromDate, string toDate)
        {
            ViewBag.searchKey = searchKey;
            ViewBag.status = status;
            ViewBag.cateID = cateID;
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            return PartialView("_TableCustomerServicePlan", customerServicePlan.Search(page, searchKey, code, status, fromDate, toDate));
        }

        //Lấy chi tiết gói cước của khách hàng
        [HttpGet]
        public PartialViewResult GetCustomerServicePlanDetail(int id)
        {
            return PartialView("_CustomerServePlanDetail", customerServicePlan.GetCustomerServicePlanDetail(id));
        }
    }
}