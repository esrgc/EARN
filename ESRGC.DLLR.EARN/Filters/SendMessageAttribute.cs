using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESRGC.DLLR.EARN.Controllers;
using ESRGC.DLLR.EARN.Domain.DAL;
using ESRGC.DLLR.EARN.Domain.DAL.Concrete;
using ESRGC.DLLR.EARN.Helpers;

namespace ESRGC.DLLR.EARN.Filters
{
  public class SendMessaageAttribute : ActionFilterAttribute
  {
    public override void OnActionExecuted(ActionExecutedContext filterContext) {
      try {
        var workUnit = (filterContext.Controller as BaseController).WorkUnit;
        var messages = workUnit.MessageRepository
          .Entities
          .Where(x => x.EmailSent == false && x.SenderID != x.ReceiverID)
          .ToList();
        foreach (var unsent in messages) {
          EmailHelper.SendEmailMessage(unsent);
          unsent.EmailSent = true;
          workUnit.MessageRepository.UpdateEntity(unsent);
          workUnit.saveChanges();
          //else { 
          //  //attempt to send the verification email
          //  EmailHelper.SendVerificationEmail(unsent.Account);
          //}
        }
      }
      catch (Exception) {

      }
    }
  }
}