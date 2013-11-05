using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Domain.DAL;
using ESRGC.DLLR.EARN.Domain.DAL.Concrete;
using ESRGC.DLLR.EARN.Helpers;

namespace ESRGC.DLLR.EARN.Filters
{
  public class SendNotificationAttribute: ActionFilterAttribute
  {
    public override void OnActionExecuted(ActionExecutedContext filterContext) {
      var requestEmail = filterContext.HttpContext.User.Identity.Name;
      var workUnit = new WorkUnit(new DomainContext());
      try {
        var account = workUnit
          .AccountRepository
          .Entities
          .First(x => x.EmailAddress.ToLower() == requestEmail.ToLower());
        var unsentNotifs = workUnit.NotificationRepository
          .Entities
          .Where(x => (!x.EmailSent))
          .ToList();
        foreach (var unsent in unsentNotifs) {
          EmailHelper.SendNotificationEmail(unsent);
          unsent.EmailSent = true;
          workUnit.NotificationRepository.UpdateEntity(unsent);
        }
        workUnit.saveChanges();
       
      }
      catch (Exception) {
       
      }
    }
  }
}