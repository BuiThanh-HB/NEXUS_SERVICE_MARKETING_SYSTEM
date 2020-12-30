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
    }
}