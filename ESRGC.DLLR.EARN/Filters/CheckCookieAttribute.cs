using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ESRGC.DLLR.EARN.Filters
{
  public class CheckCookieAttribute: ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext filterContext) {
      var request = filterContext.HttpContext.Request;
      if (!request.Browser.Cookies) {
        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary() {
          { "controller", "Home" },
          { "action", "CookieNotEnabled" }
        });
      }
      if (request.Cookies[".EARNMDCONNECT-guestCookie"] == null) {
        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary() {
          { "controller", "Home" },
          { "action", "CookieNotEnabled" }
        });
      }
    }
  }
}