﻿using System.Web;
using System.Web.Optimization;

namespace APIProject
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/layout/js").Include(
                "~/Scripts/jquery.min.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/libscripts.bundle.js",
                      "~/Scripts/vendorscripts.bundle.js",
                      "~/Scripts/sweetalert.min.js",
                      "~/Scripts/mainscripts.bundle.js",
                      "~/Scripts/bootstrap-413.js",
                      "~/Scripts/index.js",
                       "~/Scripts/toastr.min.js",
                       "~/Scripts/dropify.min.js"
                      ));

            bundles.Add(new ScriptBundle("~/main/js").Include(
                     "~/Scripts/jquery.unobtrusive-ajax.min.js",
                     "~/Content/ckfinder/ckfinder.js",
                     "~/Content/ckeditor/ckeditor.js",
                     "~/Scripts/jquery-ui.js",
                     "~/Scripts/angular.min.js",
                     "~/Scripts/ready.js",
                     "~/Scripts/ajax.js",
                     "~/Scripts/jquery.validate.min.js"

                     ));




            bundles.Add(new ScriptBundle("~/bundle/js").Include(
                    "~/Scripts/jquery-ui.js"
                    ));
            bundles.Add(new StyleBundle("~/main/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/bootstrap-413.css",
                      "~/Content/sweetalert.css",
                      "~/Content/PagedList.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/chartist-plugin-tooltip.css",
                      "~/Content/main.css",
                      "~/Content/chatapp.css",
                      "~/Content/color_skins.css",
                      "~/Content/style.css",
                      "~/Content/jquery-ui.css", "~/Content/toastr.min.css", "~/Content/dropify.min.css"));

            BundleTable.EnableOptimizations = false;
        }
    }
}
