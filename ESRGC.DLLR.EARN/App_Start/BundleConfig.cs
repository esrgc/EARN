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
        "~/Client/jsLib/jquery-1.x/jquery-1.9.1.js",
        "~/Client/jsLib/jquery-1.x/jquery-migrate-1.2.1.js",       
        "~/Client/jsLib/jquery.placeholder.js",
        "~/Client/jsLib/bootstrap-3.0.2/js/bootstrap*",
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
      bundles.Add(new ScriptBundle("~/bundles/js/customLibs").Include(
        "~/Client/js/dx.min.js",
        "~/Client/jsLib/typeahead.js/typeahead.js"
      ));
      bundles.Add(new ScriptBundle("~/bundles/js/map").Include(
        "~/Client/js/map/mapViewer.js",
        "~/Client/js/map/leafletViewer.js",
        "~/Client/jsLib/wicket-leaflet/wicket.js",
        "~/Client/jsLib/wicket-leaflet/wicket-leaflet.js"
      ));
      bundles.Add(new ScriptBundle("~/bundles/apps/search").Include(

        "~/Client/apps/search/controller/*.js",
        "~/Client/apps/search/store/*.js",
        //"~/Client/apps/search/model/*.js",
        //"~/Client/apps/search/view/*.js",
        "~/Client/apps/search/app.js"
      ));
      bundles.Add(new ScriptBundle("~/bundles/dllrjs").Include(
        "~/Client/dllrcontent/javascript/*.js",
        "~/Client/dllrcontent/scripts/*.js"
      ));

      //message center
      bundles.Add(new ScriptBundle("~/bundles/messageCenter").Include(
        "~/Client/apps/message/model/*.js",
        "~/Client/apps/message/collection/*.js",
        "~/Client/apps/message/router/*.js",
        "~/Client/apps/message/view/*.js",
        "~/Client/apps/message/*.js"
      ));
      //////////////////////////////////////////////////////////////////////////////
      //css bundles
      //////////////////////////////////////////////////////////////////////////////
      bundles.Add(new StyleBundle("~/Styles/css").Include(        
        "~/Client/css/less/*.css",
        "~/Client/css/*.css"
      ));
      
      //bundles.Add(new StyleBundle("~/Styles/mapcss").Include(
      //    "~/Client/jsLib/leaflet.awesome-markers/leaflet.awesome-markers.css"
      //));
      bundles.Add(new StyleBundle("~/Styles/dllrContent/css/dllrCSS").Include(
        "~/Client/dllrContent/css/dllrCSS.css",
        "~/Client/dllrContent/style/print-preview.css",
        "~/Client/dllrContent/style/print.css"        
      ));
    }
  }
}