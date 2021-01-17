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
    public class NewsController : BaseController
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
                var data = newsBusiness.SearchNewsWeb(Page, Name);
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