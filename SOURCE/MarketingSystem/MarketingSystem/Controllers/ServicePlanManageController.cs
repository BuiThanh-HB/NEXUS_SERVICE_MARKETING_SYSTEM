using APIProject.App_Start;
using APIProject.Controllers;
using Data.Model;
using Data.Utils;
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
        public PartialViewResult Search(int page, string searchKey, string code, int? status,  string fromDate, string toDate)
        {
            ViewBag.searchKey = searchKey;
            ViewBag.code = code;
            ViewBag.status = status;
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

        //Cập nhật gói cước của khách hàng
        [HttpPost]
        public JsonResult UpdateCustomerServicePlan(CustomerServicePlanDetailModel input)
        {
            if (admin.Role.Equals(SystemParam.ROLE_TECHNICAL_STAFF))
            {
                HttpResponseBase response = Response;
                response.Redirect("/ServicePlanManage/Index");
            }
            input.UserID = admin.Id;
            return Json(customerServicePlan.UpdateCustomerServicePlan(input), JsonRequestBehavior.AllowGet);
        }
    }
}