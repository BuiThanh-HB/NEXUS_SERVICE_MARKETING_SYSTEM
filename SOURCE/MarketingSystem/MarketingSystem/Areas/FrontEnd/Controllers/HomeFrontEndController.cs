using APIProject.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketingSystem.Areas.FrontEnd.Controllers
{
    public class HomeFrontEndController : BaseController
    {
        // GET: FrontEnd/HomeFrontEnd
        public ActionResult Index()
        {
            return View();
        }
    }
}