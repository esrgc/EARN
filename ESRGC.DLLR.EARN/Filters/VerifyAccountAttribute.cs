using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ESRGC.DLLR.EARN.Controllers;
using ESRGC.DLLR.EARN.Domain.DAL;
using ESRGC.DLLR.EARN.Domain.DAL.Concrete;

namespace ESRGC.DLLR.EARN.Filters
{
  public class VerifyAccountAttribute : ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext filterContext) {
      try {
        System.Diagnostics.Debug.WriteLine(filterContext.ActionDescriptor.ActionName);
        var requestEmail = filterContext.HttpContext.User.Identity.Name;
        var workUnit = (filterContext.Controller as BaseController).WorkUnit;
        workUnit.AccountRepository.Entities.First(x => x.EmailAddress.ToLower() == requestEmail.ToLower());
      }
      catch (Exception) {
        if (filterContext.IsChildAction)
          filterContext.Result = new EmptyResult();
        else
          filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary() { 
            {"controller", "Home"},
            {"action", "Index"}
          });
      }
    }
  }
}