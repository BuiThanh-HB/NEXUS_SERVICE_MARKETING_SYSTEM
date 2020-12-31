using APIProject.App_Start;
using APIProject.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketingSystem.Controllers
{
    public class NewsController : BaseController
    {
        // GET: News
        [UserAuthenticationFilter]
        public ActionResult Index()
        {
            return View();
        }

        //Chuyển trang thêm mới bài viết
        [UserAuthenticationFilter]
        public ActionResult CreateNews()
        {
            return View();
        }

        //Tìm kiếm thông tin bài viết
        [HttpGet]
        public PartialViewResult Search(int page, string searchKey, Boolean? status, int? type, string fromDate, string toDate)
        {
            ViewBag.searchKey = searchKey;
            ViewBag.status = status;
            ViewBag.type = type;
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            return PartialView("_TableNews", newsBusiness.Search(page, searchKey, status, type, fromDate, toDate));
        }

        //Thêm bài viết
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult CreateNews(int type, bool status, string content, string title, string summary, string img)
        {
            return Json(newsBusiness.CreateNews(type, status, content, title, summary, img), JsonRequestBehavior.AllowGet);
        }

        //Xem chi tết bài viết
        [UserAuthenticationFilter]
        [HttpGet]
        public ActionResult GetNewsDetail(int id)
        {
            return View(newsBusiness.GetNewsDetail(id));
        }

        //Cập nhật bài viết
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult UpdateNews(int id, int type, bool status, string content, string title, string summary, string img)
        {
            return Json(newsBusiness.UpdateNews(id, type, status, content, title, summary, img), JsonRequestBehavior.AllowGet);
        }

        //Xóa bài viết
        [HttpPost]
        public JsonResult DelNews(int id)
        {
            return Json(newsBusiness.DelNews(id), JsonRequestBehavior.AllowGet);
        }
    }
}