using APIProject.App_Start;
using APIProject.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Base.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        [UserAuthenticationFilter]
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult changepass(string OldPass, string NewPass)
        {
            var result = userBusiness.ChangePassUser(OldPass, NewPass);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult Search(int page, string searchKey, string fromDate, string toDate)
        {
            ViewBag.searchKey = searchKey;
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            return PartialView("_TableUser", userBusiness.Search(page, searchKey, fromDate, toDate));
        }

        public JsonResult AddUser(string userName, string userPhone, string password)
        {
            return Json(userBusiness.AddUser(userName, userPhone, password), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateUserInfo(int id, string userName, string userPhone, string password)
        {
            return Json(userBusiness.UpdateUserInfo(id, userName, userPhone, password), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteUser(int id)
        {
            return Json(userBusiness.DeleteUser(id), JsonRequestBehavior.AllowGet);
        }
    }
}