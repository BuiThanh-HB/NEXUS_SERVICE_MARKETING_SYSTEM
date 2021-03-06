﻿
using Data.Model.APIWeb;

using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Data.Utils;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace APIProject.App_Start
{
    public class UserAuthenticationFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            try
            {
                var routeValues = new RouteValueDictionary();
                LoginOutputModel session = (LoginOutputModel)filterContext.HttpContext.Session[SystemParam.ADMIN];
                if (session == null || session.Id == 0)
                {
                    //Chuyen ve trang dang nhap
                    routeValues["controller"] = "FrontEnd/HomeFrontEnd";
                    routeValues["action"] = "Index";
                    filterContext.Result = new RedirectToRouteResult(routeValues);


                }
            }
            catch
            {
                var routeValues = new RouteValueDictionary();
                routeValues["controller"] = "FrontEnd/HomeFrontEnd";
                routeValues["action"] = "Index";
                filterContext.Result = new RedirectToRouteResult(routeValues);
            }
        }

        //Runs after the OnAuthentication method  
        //------------//
        //OnAuthenticationChallenge:- if Method gets called when Authentication or Authorization is 
        //failed and this method is called after
        //Execution of Action Method but before rendering of View
        //------------//
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            //We are checking Result is null or Result is HttpUnauthorizedResult 
            // if yes then we are Redirect to Error View
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "Error"
                };
            }
        }
    }
}