using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketingSystem.Areas.FrontEnd.Controllers
{
    public class AboutUsController : Controller
    {
        // GET: FrontEnd/AboutUs
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult SearchAboutUS(int Page=1, string Name="")
        {
            try
            {
                return PartialView("_AboutUs", "");
            }
            catch
            {
                return PartialView("_AboutUs", /*new List<ListPostCategory>().ToPagedList(1, 1)*/ "");
            }
        }
    }
}