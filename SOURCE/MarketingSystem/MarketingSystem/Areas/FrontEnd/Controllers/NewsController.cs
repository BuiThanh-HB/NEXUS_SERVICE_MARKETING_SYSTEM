using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketingSystem.Areas.FrontEnd.Controllers
{
    public class NewsController : Controller
    {
        // GET: FrontEnd/News
        public ActionResult Index()
        {
            return View();
        }
    }
}