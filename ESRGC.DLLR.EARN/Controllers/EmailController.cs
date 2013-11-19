using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActionMailer.Net.Mvc;
using ESRGC.DLLR.EARN.Domain.Model;

namespace ESRGC.DLLR.EARN.Controllers
{
  public class EmailController : MailerBase
  {
    public EmailController() {
      try {
        From = ConfigurationManager.AppSettings["senderEmail"].ToString();
      }
      catch (Exception) {
        From = "no-reply-EARNMDCONNECT@salisbury.edu";
      }
    }

    public EmailResult Notification(Notification model) {
      To.Add(model.Account.EmailAddress);
      Subject = "EARN MD CONNECT - " + model.Category;
      return Email("Notification", model);
    }

    public EmailResult EmailVerification(Account model) {
      To.Add(model.EmailAddress);
      Subject = "EARN MD CONNECT - Email Address Verification";
      return Email("EmailVerification", model);
    }
    public EmailResult ReVerificationEmail(Account model) {
      To.Add(model.EmailAddress);
      Subject = "EARN MD CONNECT - Email Address Verification";
      return Email("ReVerificationEmail", model);
    }
    public EmailResult ForgotPassword(Account model) {
      To.Add(model.EmailAddress);
      Subject = "EARN MD CONNECT - Your account password";
      return Email("ForgotPassword", model);
    }
    public EmailResult SendEmailMessage(Message model) {
      To.Add(model.Receiver.Contact.Email);
      CC.Add(model.Sender.Contact.Email);
      if (string.IsNullOrEmpty(model.Title))
        Subject = "EARN MD CONNECT - You have received a new message";
      else
        Subject = "EARN MD CONNECT - " + model.Title;
      return Email("SendEmailMessage", model);
    }
  }
}
