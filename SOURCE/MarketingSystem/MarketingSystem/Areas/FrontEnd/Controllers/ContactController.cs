using APIProject.Controllers;
using Data.Business;
using Data.DB;
using Data.Model;
using Data.Model.APIWeb;
using Data.Utils;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketingSystem.Areas.FrontEnd.Controllers
{
    public class ContactController : BaseController
    {
        GenericBusiness gn = new GenericBusiness();
        // GET: FrontEnd/Contact
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public int CreateFeedBack(string Content)
        {
            try
            {
                LoginOutputModel cus = client;
                if (cus != null && !String.IsNullOrEmpty(Content))
                {
                    Feedback fd = new Feedback();
                    fd.CustomerID = cus.Id;
                    fd.Content = Content;
                    fd.Status = SystemParam.ACTIVE;
                    fd.IsActive = SystemParam.ACTIVE;
                    fd.CreatedDate = DateTime.Now;
                    fd.Type = 1;

                    gn.cnn.Feedbacks.Add(fd);
                    gn.cnn.SaveChanges();
                    return 1;
                }


                return -1;
            }
            catch(Exception e)
            {
                e.ToString();
                return 0;
            }

        }
    }
}