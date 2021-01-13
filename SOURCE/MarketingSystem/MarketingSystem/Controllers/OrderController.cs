using APIProject.App_Start;
using APIProject.Controllers;
using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketingSystem.Controllers
{
    public class OrderController : BaseController
    {
        // GET: Order
        [UserAuthenticationFilter]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public PartialViewResult Search(int page, string searchKey, string fromDate, string toDate, int? status)
        {
            ViewBag.searchKey = searchKey;
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            ViewBag.status = status;
            return PartialView("_TableOrder", orderBusiness.Search(page, searchKey, fromDate, toDate, status));
        }

        //Lấy chi tiết đơn hàng
        [HttpGet]
        public PartialViewResult GetOrderDetail(int id)
        {
            return PartialView("_OrderDetail", orderBusiness.GetOrderDetail(id));
        }

        //Cập nhật trạng thái đơn hàng
        [HttpPost]
        public JsonResult UpdateBill(OrderDetailOuputModel input)
        {
            return Json(orderBusiness.UpdateBill(input, admin.Id, admin.Role));
        }

        //Xuất hóa đơn
        [UserAuthenticationFilter]
        public FileResult ExportBill(int id)
        {
            try
            {
                return File(orderBusiness.ExportBill(id).GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Bill.xlsx");

            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }
    }
}