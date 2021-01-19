using Data.Model.APIWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data.Business;
using Data.Utils;
using APIProject.App_Start;
using Newtonsoft.Json;
using APIProject.Models;
using Data.Model;

namespace APIProject.Controllers
{
    public class HomeController : BaseController
    {
        [UserAuthenticationFilter]
        public ActionResult Index()
        {
            ViewBag.countOrder = orderBusiness.GetCountOrder();
            ViewBag.countCus = customerBusiness.CountCus();
            ViewBag.countService = servicePlanBusiness.CountServicePlan();
            ViewBag.Title = "Trang chủ";
            return View();
        }
        public ActionResult Login()
        {
            ViewBag.Title = "Đăng nhập";
            return View();
        }


        public JsonResult UserLogin(string account, string password)
        {
            var result = loginBusiness.LoginWeb(account, password);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //lưu lại thông tin đối tượng vừa đăng nhập
        [UserAuthenticationFilter]
        public JsonResult GetUserLogin()
        {
            try
            {
                if (Session[SystemParam.ADMIN] != null)
                {
                    LoginOutputModel userLogin = (LoginOutputModel)Session[SystemParam.ADMIN];
                    int? userID = loginBusiness.checkTokenUser(userLogin.Token);
                    if (String.IsNullOrEmpty(userLogin.Token) || userID == 0)
                    {
                        Session[SystemParam.ADMIN] = null;
                        userLogin.Role = -1;
                    }
                    return Json(userLogin, JsonRequestBehavior.AllowGet);
                }
                return Json(new UserDetailOutputModel(), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new UserDetailOutputModel(), JsonRequestBehavior.AllowGet);
            }
        }

        ///đăng xuất
        public int Logout()
        {
            try
            {
                Session[SystemParam.ADMIN] = null;
                return SystemParam.SUCCESS;
            }
            catch
            {
                return SystemParam.ERROR;
            }
        }

    }
}
