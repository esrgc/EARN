using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ESRGC.DLLR.EARN.Filters
{
  public class SetGuestCookieAttribute: ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext filterContext) {
      var response = filterContext.HttpContext.Response;
      // make it expire a long time from now, that way there's no need for redirects in the future if it already exists
      var c = new HttpCookie(".EARNMDCONNECT-guestCookie", "true") { Expires = DateTime.Now.AddMinutes(600) };
      response.Cookies.Add(c);
    }
  }
}