﻿using System.Web.Mvc;

namespace MarketingSystem.Areas.FrontEnd
{
    public class FrontEndAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "FrontEnd";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "FrontEnd_default",
                "FrontEnd/{controller}/{action}/{id}",
                new { Controller="HomeFontEnd",action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}