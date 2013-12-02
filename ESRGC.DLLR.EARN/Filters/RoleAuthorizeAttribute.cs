using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ESRGC.DLLR.EARN.Controllers;

namespace ESRGC.DLLR.EARN.Filters
{
  public class RoleAuthorizeAttribute : AuthorizeAttribute
  {
    public override void OnAuthorization(AuthorizationContext filterContext) {
      if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowNonAdminAttribute), false).Any()) {
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
        
        //check if the current profile is in the partnership partners list
        if (!Roles.Contains(account.Role.ToLower())) {
          filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary() { 
            {"controller", "Home"},
            {"action", "UnauthorizedAccess"}
          });
        }
      }
      catch (Exception) {
        filterContext.Controller.TempData["Message"] = "Error verifying role for the current account";
        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary() { 
          {"controller", "Home"},
          {"action", "Index"}
        });
      }
    }
    
  }
}