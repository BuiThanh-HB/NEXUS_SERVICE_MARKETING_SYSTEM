using APIProject.Controllers;
using Data.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketingSystem.Areas.FrontEnd.Controllers
{
    public class NewsFrontEndController : BaseController
    {
        // GET: FrontEnd/News
        public ActionResult Index()
        {
            ViewBag.ListNews = newsBusiness.ListNewsWeb(3);
            ViewBag.ListCategory = newsBusiness.GetListNewCategory();
            return View();
        }
        public PartialViewResult SearchNewsWeb(int Page = 1, string Name = "" ,int CateNewsID = 0)
        {
            try
            {
                ViewBag.ListNews = newsBusiness.ListNewsWeb(3);
                ViewBag.ListCategory = newsBusiness.GetListNewCategory();
                var data = newsBusiness.SearchNewsWeb(Page, Name, CateNewsID);
                return PartialView("_ListNewsWeb", data);
            }
            catch
            {
                return PartialView("_ListNewsWeb", new List<ListNewsOutputModel>().ToPagedList(1, 1));
            }
        }

        [HttpGet]
        public PartialViewResult NewsDetail(int ID)
        {

            var data = newsBusiness.GetNewsDetail(ID);
            return PartialView("_NewsDetailWeb", data);
        }
    }
}