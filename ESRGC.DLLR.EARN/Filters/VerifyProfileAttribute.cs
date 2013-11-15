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
      var requestEmail = filterContext.HttpContext.User.Identity.Name;
      var workUnit = new WorkUnit(new DomainContext());
      try {
        var account = workUnit
          .AccountRepository
          .Entities
          .First(x => x.EmailAddress.ToLower() == requestEmail.ToLower());

        if (account.Profile == null) {
          filterContext.Controller.TempData["message"] = "You have not created an organizational profile. Please create one!";
          filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary() { 
            {"controller", "Profile"},
            {"action", "Create"}
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