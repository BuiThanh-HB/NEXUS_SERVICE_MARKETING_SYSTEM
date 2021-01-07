using APIProject.App_Start;
using APIProject.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketingSystem.Controllers
{
    public class CustomerController : BaseController
    {
        // GET: Customer
        [UserAuthenticationFilter]
        public ActionResult Index()
        {
            return View();
        }
    }
}