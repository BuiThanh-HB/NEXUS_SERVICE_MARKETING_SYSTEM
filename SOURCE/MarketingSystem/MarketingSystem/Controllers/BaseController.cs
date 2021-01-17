using Data.Business;
using Data.DB;
using Data.Model;
using Data.Model.APIWeb;
using Data.Utils;
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
        public ServicePlanBusiness servicePlanBusiness;
        public NewsBusiness newsBusiness;
        public CustomerBusiness customerBusiness;
        public OrderBusiness orderBusiness;
        public CustomerServicePlanBusiness customerServicePlan;

        public BaseController() : base()
        {
            loginBusiness = new LoginBusiness(this.GetContext());
            userBusiness = new UserBusiness(this.GetContext());
            categoryBusiness = new CategoryBusiness(this.GetContext());
            servicePlanBusiness = new ServicePlanBusiness(this.GetContext());
            newsBusiness = new NewsBusiness(this.GetContext());
            customerBusiness = new CustomerBusiness(this.GetContext());
            orderBusiness = new OrderBusiness(this.GetContext());
            customerServicePlan = new CustomerServicePlanBusiness(this.GetContext());
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

        public LoginOutputModel admin
        {
            get
            {
                return Session[SystemParam.ADMIN] as LoginOutputModel;
            }
        }
        public LoginOutputModel client
        {
            get
            {
                return Session[SystemParam.CLIENT] as LoginOutputModel;
            }
        }
    }
}