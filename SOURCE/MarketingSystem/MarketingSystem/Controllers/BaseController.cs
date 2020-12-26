using Data.Business;
using Data.DB;
using Data.Model.APIWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APIProject.Controllers
{
    public class BaseController : Controller
    {
        protected NEXUS_SystemEntities Context;
        public LoginBusiness loginBusiness;
        public UserBusiness userBusiness;
        public CategoryBusiness categoryBusiness;

        public BaseController() : base()
        {
            loginBusiness = new LoginBusiness(this.GetContext());
            userBusiness = new UserBusiness(this.GetContext());
            categoryBusiness = new CategoryBusiness(this.GetContext());
        }
        /// <summary>
        /// Create new context if null
        /// </summary>
        public NEXUS_SystemEntities GetContext()
        {
            if (Context == null)
            {
                Context = new NEXUS_SystemEntities();
            }
            return Context;
        }
    }
}