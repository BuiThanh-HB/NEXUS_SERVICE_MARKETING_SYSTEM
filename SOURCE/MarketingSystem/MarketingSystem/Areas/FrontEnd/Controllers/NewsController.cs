using Data.Model;
using PagedList;
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
        public PartialViewResult SearchNewsWeb(int Page = 1, string Name = "")
        {
            try
            {
                //var data = servicePlanBusiness.SearchFontEnd(Page, Name, 1, CateID);
                return PartialView("_ListNewsWeb", new List<ListNewsOutputModel>().ToPagedList(1, 1));
            }
            catch
            {
                return PartialView("_ListNewsWeb", new List<ListNewsOutputModel>().ToPagedList(1, 1));
            }
        }
    }
}