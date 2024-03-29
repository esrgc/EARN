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
  /// Verifies if the current profile exists. 
  /// If profile does exist the filter sets ViewBag.currenProfile 
  /// to the profile associated with this account
  /// </summary>
  public class HasPendingProfileRequestAttribute : ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext filterContext) {
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
          var message = "You currently have a pending partnership request. Please wait until your request is accepted!";
          if(!account.EmailVerified){
            message += " You will be notified via email. Please verify your email address. You will not be able to receive email notifications until you do so.";
          }
          filterContext.Controller.TempData["message"] = message;
          filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary() { 
            {"controller", "Home"},
            {"action", "Index"}
          });
        }
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