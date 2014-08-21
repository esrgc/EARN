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
  /// <summary>
  /// Verifies if the current profile exists. 
  /// If profile does exist the filter sets ViewBag.currenProfile 
  /// to the profile associated with this account
  /// </summary>
  public class VerifyProfileAttribute : ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext filterContext) {
      if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowNonProfileAttribute),false).Any()) {
        // The controller action is decorated with the [AllowNonProfileAttribute]
        // custom attribute => don't do anything.
        return;
      }
      var requestEmail = filterContext.HttpContext.User.Identity.Name;
      var workUnit = (filterContext.Controller as BaseController).WorkUnit;
      try {
        var account = workUnit
          .AccountRepository
          .Entities
          .First(x => x.EmailAddress.ToLower() == requestEmail.ToLower());
        //check if there's pending request to join profile
        //..to be implemented
        var pendingRequests = account.SentProfileRequests.ToList();
        if (pendingRequests.Count() > 0) {
          filterContext.Controller.TempData["message"] = "You currently have a pending profile request. Please wait until your request is accepted!";
          filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary() { 
            {"controller", "Home"},
            {"action", "Index"}
          });
          return;
        }
        //profile hasn't been created or there's no pending request to join profile
        if (account.Profile == null) {
          filterContext.Controller.TempData["message"] = "You have not created an organizational profile. Please create one!";
          filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary() { 
            {"controller", "Profile"},
            {"action", "Index"}
          });
        }
        else
          filterContext.Controller.ViewBag.currentProfile = account.Profile;
      }
      catch (Exception) {
        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary() { 
          {"controller", "Home"},
          {"action", "Index"}
        });
      }
    }
  }
}