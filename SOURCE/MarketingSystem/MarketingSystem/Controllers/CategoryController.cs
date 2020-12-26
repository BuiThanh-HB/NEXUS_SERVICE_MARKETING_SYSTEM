using APIProject.App_Start;
using APIProject.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketingSystem.Controllers
{
    public class CategoryController : BaseController
    {
        // GET: Category
        [UserAuthenticationFilter]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult Search(int page, string searchKey, string fromDate, string toDate, string type)
        {
            ViewBag.searchKey = searchKey;
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            ViewBag.type = type;
            return PartialView("_TableCate", categoryBusiness.Search(page, searchKey, fromDate, toDate, type));
        }

        //Thêm mới danh mục gói cước
        [HttpPost]
        public JsonResult AddCategory(string name, string type)
        {
            return Json(categoryBusiness.AddCategory(name, type), JsonRequestBehavior.AllowGet);
        }
        //Cập nhật thông tin danh mục
        [HttpPost]
        public JsonResult UpdateCategory(int id, string name, string type)
        {
            return Json(categoryBusiness.UpdateCategory(id, name, type), JsonRequestBehavior.AllowGet);
        }

        //Xóa danh mục
        [HttpPost]
        public JsonResult DelCate(int id)
        {
            return Json(categoryBusiness.DelCategory(id), JsonRequestBehavior.AllowGet);
        }
    }
}