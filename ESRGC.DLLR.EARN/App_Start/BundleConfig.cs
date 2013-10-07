using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace ESRGC.DLLR.EARN
{
  public class BundleConfig
  {
    public static void RegisterBundles(BundleCollection bundles) {
      //scripts bundles
      bundles.Add(new ScriptBundle("~/bundles/jsLibs").Include(
        "~/Client/jsLib/jquery/jquery-{version}.js",
        "~/Client/jsLib/bootstrap-3.0/js/bootstrap*",
        "~/Client/js/modernizr*"
      ));

      bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
        "~/Client/jslib/jqueryValidate/jquery.validate.*",
        "~/Client/js/bootstrapValidation.js"
      ));

      bundles.Add(new ScriptBundle("~/bundles/jqueryval-unobtrusive").Include(
        "~/Client/jsLib/jqueryValidate/jquery.validate.*",
        "~/Client/jsLib/jqueryValidate/jquery.validate.unobtrusive*",
        "~/Client/js/bootstrapValidation.js"
      ));
      bundles.Add(new ScriptBundle("~/bundles/apps/search").Include(
        "~/Client/js/dx.min.js",
        "~/Client/jsLib/typeahead.js/typeahead.min.js",
        "~/Client/apps/search/controller/*.js",
        "~/Client/apps/search/store/*.js",
        "~/Client/apps/search/model/*.js",
        "~/Client/apps/search/view/*.js",
        "~/Client/apps/search/app.js"
      ));
      //css bundles
      bundles.Add(new StyleBundle("~/Styles/css").Include(       
        "~/Client/jsLib/bootstrap-3.0/css/bootstrap*",
        "~/Client/css/less/*.css",
        "~/Client/css/*.css"
      ));
    }
  }
}