using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ESRGC.DLLR.EARN.Filters
{
  public class HasReturnUrlAttribute: ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext filterContext) {
      var parameters = filterContext.ActionParameters;
      if (parameters.ContainsKey("returnUrl")) {
        filterContext.Controller.ViewBag.returnUrl = parameters["returnUrl"];
      }
    }
    public override void OnActionExecuted(ActionExecutedContext filterContext) {
    }
  }
}