﻿using System;
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
  /// This attribute verifies if the current profile is asscociated with the 
  /// partnership being requested
  /// </summary>
  public class VerifyProfilePartnershipAttribute : ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext filterContext) {
      var requestEmail = filterContext.HttpContext.User.Identity.Name;
      var workUnit = (filterContext.Controller as BaseController).WorkUnit;
      try {
        var account = workUnit
          .AccountRepository
          .Entities
          .First(x => x.EmailAddress.ToLower() == requestEmail.ToLower());
        var currentProfile = account.Profile;
        int partnershipID = (filterContext.ActionParameters["partnershipID"] as int?).Value;
        //check if the current profile is in the partnership partners list
        if (!currentProfile.PartnershipDetails.Select(x => x.PartnershipID).Contains(partnershipID)) {
          filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary() { 
            {"controller", "Partnership"},
            {"action", "InvalidAccessToPartnership"}
          });
        }
      }
      catch (Exception) {
        filterContext.Controller.TempData["Message"] = "Error verifying partnership with current partnership.";
        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary() { 
          {"controller", "Home"},
          {"action", "Index"}
        });
      }

    }

  }
}